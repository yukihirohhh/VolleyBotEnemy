using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{
    private bool movingRight = true;
    private bool isAnimation2Playing = false;
    SpriteRenderer sr;

    [Tooltip("動くスピード")] public float speed;
    [Tooltip("Rayの飛ばす距離")] public float distance;
    [Tooltip("Ray飛ばすやつのもとobj")] public Transform groundDetection;
    [Tooltip("Ball検出用トランスフォーム")] public Transform ballDetection;
    [Tooltip("吹き飛ばす力")] public float thrust = 10f;
    [Tooltip("右方向に吹き飛ばす力")] public Vector2 forceDirectionRight = new Vector2(0.3f, 1f);
    [Tooltip("左方向に吹き飛ばす力")] public Vector2 forceDirectionLeft = new Vector2(-0.3f, 1f);

    [Space(20)]

    [Header("アニメーション用配列_1 ----------------------------------------------------")]
    [Tooltip("配列数")] public Sprite[] anim_1_array;
    float anime_time_1;
    [Tooltip("フレームが変わる間隔設定")] public float anim_1_sec;
    [Tooltip("スプライトの変化プレビュー")] public int anime_1_count;

    [Header("アニメーション用配列_2 ----------------------------------------------------")]
    [Tooltip("配列数")] public Sprite[] anim_2_array;
    float anime_time_2;
    [Tooltip("フレームが変わる間隔設定")] public float anim_2_sec;
    [Tooltip("スプライトの変化プレビュー")] public int anime_2_count;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isAnimation2Playing)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
            Debug.DrawRay(groundDetection.position, Vector2.down * distance, Color.red);

            if (groundInfo.collider == null)
            {
                FlipDirection();
            }

            ANIMATION_1();
        }
        else
        {
            ANIMATION_2();
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
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 forceDirection = movingRight ? forceDirectionRight : forceDirectionLeft;
                rb.AddForce(forceDirection.normalized * thrust, ForceMode2D.Impulse);
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
                Destroy(gameObject);
            }

            sr.sprite = anim_2_array[anime_2_count];
        }
    }
}
