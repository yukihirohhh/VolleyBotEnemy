using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Vector2[] relativePoints;
    private Vector2[] points;
    private Rigidbody2D rb;
    private Vector2 currentPoint;
    public float speed;
    public float chaseSpeedMultiplier = 2f;

    // Animation variables
    private SpriteRenderer sr;
    public Sprite[] anim_1_array;
    private float anime_time_1;
    public float anim_1_sec;
    private int anime_1_count;

    // Death animation variables
    public Sprite[] anim_2_array;
    private float anime_time_2;
    public float anim_2_sec;
    private int anime_2_count;
    private bool isAnimation2Playing = false;

    // Idle animation variables
    public Sprite[] anim_3_array;
    private float anime_time_3;
    public float anim_3_sec;
    private int anime_3_count;
    private bool isAnimation3Playing = false;
    private float animationSwitchTime;
    public float minAnimationSwitchInterval = 3f;
    public float maxAnimationSwitchInterval = 6f;

    // Player detection variables
    public float detectionRange = 5f;
    private Transform playerTransform;
    private bool isChasingPlayer = false;

    // Drop item variables
    public GameObject[] dropItems;

    // Audio variables
    public AudioClip deathSound;
    public AudioClip loopSound; // 新規追加: ループサウンドのAudioClip
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Initialize animation variables
        sr = GetComponent<SpriteRenderer>();
        anime_time_1 = Time.time;
        anime_1_count = 0;

        // Initialize points array based on relativePoints
        points = new Vector2[relativePoints.Length];
        for (int i = 0; i < relativePoints.Length; i++)
        {
            points[i] = (Vector2)transform.position + relativePoints[i];
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // AudioSourceがなければ追加
        }

        SetRandomAnimationSwitchTime();
        SetRandomPoint();
    }

    void Update()
    {
        if (isAnimation2Playing)
        {
            ANIMATION_2();
            return;
        }

        DetectPlayer();

        if (isChasingPlayer)
        {
            ChasePlayer();
            ANIMATION_1();
        }
        else
        {
            if (Time.time >= animationSwitchTime)
            {
                isAnimation3Playing = !isAnimation3Playing;
                SetRandomAnimationSwitchTime();
            }

            if (isAnimation3Playing)
            {
                ANIMATION_3();
                rb.velocity = Vector2.zero;
            }
            else
            {
                Patrol();
                ANIMATION_1();
            }
        }
    }

    private void SetRandomAnimationSwitchTime()
    {
        animationSwitchTime = Time.time + Random.Range(minAnimationSwitchInterval, maxAnimationSwitchInterval);
    }

    private void SetRandomPoint()
    {
        if (points.Length > 0)
        {
            currentPoint = points[Random.Range(0, points.Length)];
        }
    }

    private void Patrol()
    {
        Vector2 direction = (currentPoint - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;

        // Flip the sprite based on the direction
        if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
        {
            flip();
        }

        if (Vector2.Distance(transform.position, currentPoint) < 0.5f)
        {
            SetRandomPoint();
        }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        if (relativePoints != null)
        {
            foreach (var relativePoint in relativePoints)
            {
                Vector2 worldPoint = (Vector2)transform.position + relativePoint;
                Gizmos.DrawWireSphere(worldPoint, 0.5f);
            }
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            isAnimation2Playing = true;
            anime_time_2 = Time.time;
            anime_2_count = 0;
            PlaySound(deathSound); // Play death sound
        }
    }

    private void DetectPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerTransform = hit.transform;
                isChasingPlayer = true;
                return;
            }
        }
        isChasingPlayer = false;
    }

    private void ChasePlayer()
    {
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.velocity = direction * speed * chaseSpeedMultiplier;

            // Flip the sprite based on the direction
            if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
            {
                flip();
            }
        }
    }

    void ANIMATION_1()
    {
        if (Time.time - anime_time_1 > anim_1_sec)
        {
            anime_time_1 = Time.time;

            anime_1_count++;
            if (anime_1_count >= anim_1_array.Length) { anime_1_count = 0; }

            sr.sprite = anim_1_array[anime_1_count];

            // ループサウンドを再生
            PlayLoopingSound();
        }
    }

    void ANIMATION_2()
    {
        if (Time.time - anime_time_2 > anim_2_sec)
        {
            anime_time_2 = Time.time;

            anime_2_count++;
            if (anime_2_count >= anim_2_array.Length)
            {
                anime_2_count = 0;
                isAnimation2Playing = false;
                DropItem();
                Destroy(gameObject);
            }

            sr.sprite = anim_2_array[anime_2_count];

            // ループサウンドを再生
            PlayLoopingSound();
        }
    }

    void ANIMATION_3()
    {
        if (Time.time - anime_time_3 > anim_3_sec)
        {
            anime_time_3 = Time.time;

            anime_3_count++;
            if (anime_3_count >= anim_3_array.Length) { anime_3_count = 0; }

            sr.sprite = anim_3_array[anime_3_count];
        }
    }

    private void DropItem()
    {
        if (dropItems != null && dropItems.Length > 0)
        {
            int randomIndex = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[randomIndex], transform.position, Quaternion.identity);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayLoopingSound()
    {
        if (!audioSource.isPlaying && loopSound != null)
        {
            audioSource.loop = true;
            audioSource.clip = loopSound;
            audioSource.Play();
        }
    }

    private void StopLoopingSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
