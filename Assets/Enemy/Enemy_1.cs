using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zako1 : MonoBehaviour
{
    #region//インスペクターで設定する
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("方向転換の間隔（秒）")] public float changeDirectionInterval = 2f;
    #endregion

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private bool rightTleftF = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeDirectionRoutine());
    }

    void FixedUpdate()
    {
        if (sr.isVisible || nonVisibleAct)
        {
            int xVector = rightTleftF ? 1 : -1;
            transform.localScale = new Vector3(rightTleftF ? -1 : 1, 1, 1);
            rb.velocity = new Vector2(xVector * speed, -gravity);
        }
        else
        {
            rb.Sleep();
        }
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDirectionInterval);
            rightTleftF = !rightTleftF;
        }
    }
}
