using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundSensor : MonoBehaviour
{
    // コンポーネント参照
    private PlayerController playerCtrl;

    // 接地判定用変数
    // 接地中はtrueが入る
    [HideInInspector] public bool isGround = false;

    // Start（オブジェクト有効化時に1度実行）
    void Start()
    {
        // コンポーネント参照取得
        playerCtrl = GetComponentInParent<PlayerController>();
    }

    // 各トリガー呼び出し処理
    // トリガー滞在時に呼出
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 接地判定オン "Ground"はタグ
        if (collision.tag == "Ground")
            isGround = true;
    }
    // トリガーから離れた時に呼出
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 接地判定オフ
        if (collision.tag == "Ground")
        {
            isGround = false;
        }
    }
}