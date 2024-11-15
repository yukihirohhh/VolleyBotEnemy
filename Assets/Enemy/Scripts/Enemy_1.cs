using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : MonoBehaviour
{
    private bool movingRight = true;
    private bool isAnimation2Playing = false;
    private bool isAnimation3Playing = false;
    private bool isAnimation4Playing = false; // New variable for Animation 4
    SpriteRenderer sr;

    [Tooltip("�����X�s�[�h")] public float speed;
    [Tooltip("Ray�̔�΂�����")] public float distance;
    [Tooltip("Ray��΂���̂���obj")] public Transform groundDetection;
    [Tooltip("Ball���o�p�g�����X�t�H�[��")] public Transform ballDetection;
    [Tooltip("������΂���")] public float thrust = 10f;
    [Tooltip("�E�����ɐ�����΂���")] public Vector2 forceDirectionRight = new Vector2(0.3f, 1f);
    [Tooltip("�������ɐ�����΂���")] public Vector2 forceDirectionLeft = new Vector2(-0.3f, 1f);
    [Tooltip("�A�j���[�V�����؂�ւ��Ԋu�̍ŏ��l")] public float minAnimationSwitchInterval = 1f;
    [Tooltip("�A�j���[�V�����؂�ւ��Ԋu�̍ő�l")] public float maxAnimationSwitchInterval = 3f;

    [Space(20)]

    [Header("�A�j���[�V�����z��_1_Walk ------------------------------------------------")]
    [Tooltip("�z��")] public Sprite[] anim_1_array;
    float anime_time_1;
    [Tooltip("�t���[�����ς��Ԋu�ݒ�")] public float anim_1_sec;
    [Tooltip("�X�v���C�g�̕ω��v���r���[")] public int anime_1_count;

    [Header("�A�j���[�V�����p�z��_2_Dead ------------------------------------------------")]
    [Tooltip("�z��")] public Sprite[] anim_2_array;
    float anime_time_2;
    [Tooltip("�t���[�����ς��Ԋu�ݒ�")] public float anim_2_sec;
    [Tooltip("�X�v���C�g�̕ω��v���r���[")] public int anime_2_count;

    [Header("�A�j���[�V�����p�z��_3_Stay ------------------------------------------------")]
    [Tooltip("�z��")] public Sprite[] anim_3_array;
    float anime_time_3;
    [Tooltip("�t���[�����ς��Ԋu�ݒ�")] public float anim_3_sec;
    [Tooltip("�X�v���C�g�̕ω��v���r���[")] public int anime_3_count;

    [Header("�A�j���[�V�����p�z��_4_Deflect ------------------------------------------------")]
    [Tooltip("�z��")] public Sprite[] anim_4_array; // New array for Animation 4
    float anime_time_4;
    [Tooltip("�t���[�����ς��Ԋu�ݒ�")] public float anim_4_sec;
    [Tooltip("�X�v���C�g�̕ω��v���r���[")] public int anime_4_count;

    private float animationSwitchTime;

    // Drop item variables
    public GameObject[] dropItems;

    // Audio variables
    public AudioClip deflectSound;
    public AudioClip deathSound;
    public AudioClip dropSound;  // �V�K�ǉ�: �A�C�e���h���b�v���̃T�E���h
    public AudioClip walkSound;
    private AudioSource audioSource;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();

        // AudioSource�����݂��Ȃ��ꍇ�Ɏ����Œǉ�
        audioSource = this.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        SetRandomAnimationSwitchTime();
    }

    private void Update()
    {
        if (isAnimation2Playing)
        {
            ANIMATION_2();
            return;
        }

        if (isAnimation4Playing)
        {
            ANIMATION_4();
            return;
        }

        if (Time.time >= animationSwitchTime)
        {
            isAnimation3Playing = !isAnimation3Playing;
            SetRandomAnimationSwitchTime();
        }

        if (isAnimation3Playing)
        {
            ANIMATION_3();
        }
        else
        {
            ANIMATION_1();
            Move();
        }
    }

    private void SetRandomAnimationSwitchTime()
    {
        animationSwitchTime = Time.time + Random.Range(minAnimationSwitchInterval, maxAnimationSwitchInterval);
    }

    private void Move()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        Debug.DrawRay(groundDetection.position, Vector2.down * distance, Color.red);

        if (groundInfo.collider == null)
        {
            FlipDirection();
        }
    }

    private void FlipDirection()
    {
        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            isAnimation2Playing = true;
            anime_time_2 = Time.time;
            anime_2_count = 0;
            PlaySound(deathSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            StartCoroutine(DeflectBallAfterDelay(other));
        }
    }

    private IEnumerator DeflectBallAfterDelay(Collider2D ball)
    {
        yield return new WaitForSeconds(0.08f); // 0.01�b�x�点��

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 forceDirection = movingRight ? forceDirectionRight : forceDirectionLeft;
            rb.AddForce(forceDirection.normalized * thrust, ForceMode2D.Impulse);
            PlaySound(deflectSound);

            // Trigger Animation 4
            isAnimation4Playing = true;
            anime_time_4 = Time.time;
            anime_4_count = 0;
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

    void ANIMATION_4()
    {
        if (Time.time - anime_time_4 > anim_4_sec)
        {
            anime_time_4 = Time.time;

            anime_4_count++;
            if (anime_4_count >= anim_4_array.Length)
            {
                anime_4_count = 0;
                isAnimation4Playing = false;
            }

            sr.sprite = anim_4_array[anime_4_count];
        }
    }

    private void DropItem()
    {
        if (dropItems != null && dropItems.Length > 0)
        {
            int randomIndex = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[randomIndex], transform.position, Quaternion.identity);

            // �h���b�v�T�E���h���Đ�
            PlaySound(dropSound);
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
        if (!audioSource.isPlaying && walkSound != null)
        {
            audioSource.loop = true;
            audioSource.clip = walkSound;
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
