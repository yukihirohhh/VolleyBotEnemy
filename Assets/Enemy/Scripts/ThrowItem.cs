using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : MonoBehaviour
{
    private bool hasCollided = false;
    private SpriteRenderer sr;

    [Tooltip("削除までの遅延時間")] public float destroyDelay = 1.0f;

    [Header("アニメーション用配列 ------------------------------------------------")]
    [Tooltip("アニメーション1の配列数")] public Sprite[] loopAnimArray;
    [Tooltip("アニメーション2の配列数")] public Sprite[] collisionAnimArray;
    private float animTime;
    [Tooltip("アニメーション1のフレームが変わる間隔設定")] public float loopAnimSec;
    [Tooltip("アニメーション2のフレームが変わる間隔設定")] public float collisionAnimSec;
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
