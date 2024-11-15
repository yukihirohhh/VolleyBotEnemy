using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : MonoBehaviour
{
    private bool isAnimation2Playing = false;
    private bool isAttacking = false; // For attack animation
    private bool isVisible = false; // Track visibility state
    SpriteRenderer sr;

    [Tooltip("�A�j���[�V�����؂�ւ��Ԋu�̍ŏ��l")] public float minAnimationSwitchInterval = 1f;
    [Tooltip("�A�j���[�V�����؂�ւ��Ԋu�̍ő�l")] public float maxAnimationSwitchInterval = 3f;

    [Space(20)]

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

    [Header("�A�j���[�V�����p�z��_5_Attack ------------------------------------------------")]
    [Tooltip("�z��")] public Sprite[] anim_5_array;
    float anime_time_5;
    [Tooltip("�t���[�����ς��Ԋu�ݒ�")] public float anim_5_sec;
    [Tooltip("�X�v���C�g�̕ω��v���r���[")] public int anime_5_count;

    private float animationSwitchTime;

    // Drop item variables
    public GameObject[] dropItems;

    // Throw item variables
    public GameObject[] throwItems;
    [Tooltip("�����A�C�e���̔�΂�����")] public float throwDistance = 5f;
    [Tooltip("�����A�C�e���̔�΂�����")] public float throwPower = 5f;
    [Tooltip("�����A�C�e���̐�")] public int throwCount = 1;
    [Tooltip("�����A�C�e����Y�������̗�")] public float throwHeight = 1.0f; // ������ւ̗͂𒲐�

    // Player reference
    public Transform player;

    // Audio variables
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip dropSound;  // �V�K�ǉ�: �A�C�e���h���b�v���̃T�E���h
    private AudioSource audioSource;

    private Vector3 originalScale;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();

        // AudioSource�����݂��Ȃ��ꍇ�Ɏ����Œǉ�
        audioSource = this.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        originalScale = transform.localScale;

        SetRandomAnimationSwitchTime();
    }

    private void Update()
    {
        if (!isVisible) return; // Skip update if not visible

        if (isAnimation2Playing)
        {
            ANIMATION_2();
            return;
        }

        FacePlayer(); // ��Ƀv���C���[�̕���������

        if (isAttacking)
        {
            ANIMATION_5();
            return;
        }

        ANIMATION_3();

        if (Time.time >= animationSwitchTime)
        {
            if (Random.value < 0.5f) // 50% chance to attack
            {
                isAttacking = true;
                anime_time_5 = Time.time;
                anime_5_count = 0;
            }
            SetRandomAnimationSwitchTime();
        }
    }

    private void SetRandomAnimationSwitchTime()
    {
        animationSwitchTime = Time.time + Random.Range(minAnimationSwitchInterval, maxAnimationSwitchInterval);
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

    void ANIMATION_5()
    {
        if (Time.time - anime_time_5 > anim_5_sec)
        {
            anime_time_5 = Time.time;

            anime_5_count++;
            if (anime_5_count >= anim_5_array.Length)
            {
                anime_5_count = 0;
                isAttacking = false;
                ThrowItems();
            }

            sr.sprite = anim_5_array[anime_5_count];
            PlaySound(attackSound);
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

    private void ThrowItems()
    {
        if (throwItems != null && throwItems.Length > 0)
        {
            for (int i = 0; i < throwCount; i++)
            {
                int randomIndex = Random.Range(0, throwItems.Length);
                GameObject throwItem = Instantiate(throwItems[randomIndex], transform.position + Vector3.up, Quaternion.identity);
                Rigidbody2D rb = throwItem.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 throwDirection = (player.position - transform.position).normalized;
                    rb.velocity = new Vector2(throwDirection.x * throwPower, throwDirection.y * throwPower + throwHeight);
                }
            }
        }
    }

    private void FacePlayer()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Face left
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z); // Face right
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
}
