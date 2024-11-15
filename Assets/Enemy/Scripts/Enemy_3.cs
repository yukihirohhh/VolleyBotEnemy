using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : MonoBehaviour
{
    private bool isAnimation2Playing = false;
    private bool isAttacking = false; // For attack animation
    private bool isVisible = false; // Track visibility state
    SpriteRenderer sr;

    [Tooltip("アニメーション切り替え間隔の最小値")] public float minAnimationSwitchInterval = 1f;
    [Tooltip("アニメーション切り替え間隔の最大値")] public float maxAnimationSwitchInterval = 3f;

    [Space(20)]

    [Header("アニメーション用配列_2_Dead ------------------------------------------------")]
    [Tooltip("配列数")] public Sprite[] anim_2_array;
    float anime_time_2;
    [Tooltip("フレームが変わる間隔設定")] public float anim_2_sec;
    [Tooltip("スプライトの変化プレビュー")] public int anime_2_count;

    [Header("アニメーション用配列_3_Stay ------------------------------------------------")]
    [Tooltip("配列数")] public Sprite[] anim_3_array;
    float anime_time_3;
    [Tooltip("フレームが変わる間隔設定")] public float anim_3_sec;
    [Tooltip("スプライトの変化プレビュー")] public int anime_3_count;

    [Header("アニメーション用配列_5_Attack ------------------------------------------------")]
    [Tooltip("配列数")] public Sprite[] anim_5_array;
    float anime_time_5;
    [Tooltip("フレームが変わる間隔設定")] public float anim_5_sec;
    [Tooltip("スプライトの変化プレビュー")] public int anime_5_count;

    private float animationSwitchTime;

    // Drop item variables
    public GameObject[] dropItems;

    // Throw item variables
    public GameObject[] throwItems;
    [Tooltip("投擲アイテムの飛ばす距離")] public float throwDistance = 5f;
    [Tooltip("投擲アイテムの飛ばす強さ")] public float throwPower = 5f;
    [Tooltip("投擲アイテムの数")] public int throwCount = 1;
    [Tooltip("投擲アイテムのY軸方向の力")] public float throwHeight = 1.0f; // 上方向への力を調整

    // Player reference
    public Transform player;

    // Audio variables
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip dropSound;  // 新規追加: アイテムドロップ時のサウンド
    private AudioSource audioSource;

    private Vector3 originalScale;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();

        // AudioSourceが存在しない場合に自動で追加
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

        FacePlayer(); // 常にプレイヤーの方向を向く

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

            // ドロップサウンドを再生
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
