using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : MonoBehaviour
{
    private bool hasCollided = false;
    private SpriteRenderer sr;

    [Tooltip("�폜�܂ł̒x������")] public float destroyDelay = 1.0f;

    [Header("�A�j���[�V�����p�z�� ------------------------------------------------")]
    [Tooltip("�A�j���[�V����1�̔z��")] public Sprite[] loopAnimArray;
    [Tooltip("�A�j���[�V����2�̔z��")] public Sprite[] collisionAnimArray;
    private float animTime;
    [Tooltip("�A�j���[�V����1�̃t���[�����ς��Ԋu�ݒ�")] public float loopAnimSec;
    [Tooltip("�A�j���[�V����2�̃t���[�����ς��Ԋu�ݒ�")] public float collisionAnimSec;
    private int animCount;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(PlayLoopAnimation());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCollided && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground")))
        {
            hasCollided = true;
            StopCoroutine(PlayLoopAnimation());
            StartCoroutine(PlayCollisionAnimation());
        }
    }

    private IEnumerator PlayLoopAnimation()
    {
        while (!hasCollided)
        {
            sr.sprite = loopAnimArray[animCount];
            animCount = (animCount + 1) % loopAnimArray.Length;
            yield return new WaitForSeconds(loopAnimSec);
        }
    }

    private IEnumerator PlayCollisionAnimation()
    {
        animCount = 0;
        while (animCount < collisionAnimArray.Length)
        {
            sr.sprite = collisionAnimArray[animCount];
            animCount++;
            yield return new WaitForSeconds(collisionAnimSec);
        }
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
