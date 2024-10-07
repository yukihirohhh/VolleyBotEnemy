using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : MonoBehaviour
{
    private bool movingRight = true;
    private bool isAnimation2Playing = false;
    SpriteRenderer sr;

    [Tooltip("�����X�s�[�h")] public float speed;
    [Tooltip("Ray�̔�΂�����")] public float distance;
    [Tooltip("Ray��΂���̂���obj")] public Transform groundDetection;

    [Space(20)]

    [Header("�A�j���[�V�����p�z��_1 ----------------------------------------------------")]
    [Tooltip("�z��")] public Sprite[] anim_1_array;
    float anime_time_1;
    [Tooltip("�t���[�����ς��Ԋu�ݒ�")] public float anim_1_sec;
    [Tooltip("�X�v���C�g�̕ω��v���r���[")] public int anime_1_count;

    [Header("�A�j���[�V�����p�z��_2 ----------------------------------------------------")]
    [Tooltip("�z��")] public Sprite[] anim_2_array;
    float anime_time_2;
    [Tooltip("�t���[�����ς��Ԋu�ݒ�")] public float anim_2_sec;
    [Tooltip("�X�v���C�g�̕ω��v���r���[")] public int anime_2_count;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isAnimation2Playing) // ANIMATION_2���Đ����łȂ��ꍇ�̂ݓ����ANIMATION_1���X�V
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
            if (groundInfo.collider == false)
            {
                if (movingRight == true)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    movingRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    movingRight = true;
                }
            }

            ANIMATION_1();
        }
        else
        {
            ANIMATION_2();
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
                isAnimation2Playing = false; // ANIMATION_2���I���������Ƃ�����
                Destroy(gameObject);
            }

            sr.sprite = anim_2_array[anime_2_count];
        }
    }
}
