using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターのスプライトを設定するクラス
/// </summary>
public class PlayerSprite : MonoBehaviour
{
    private PlayerController playerController; // アクター制御クラス
    private SpriteRenderer spriteRenderer; // アクターのSpriteRenderer

    // 画像素材参照
    public List<Sprite> walkAnimationRes; // 歩行アニメーション(装備別*コマ数)

    // 各種変数
    private float walkAnimationTime; // 歩行アニメーション経過時間
    private int walkAnimationFrame; // 歩行アニメーションの現在のコマ番号

    // 定数定義
    private const int WalkAnimationNum = 3; // 歩行アニメーションの1種類あたりの枚数
    private const float WalkAnimationSpan = 0.3f; // 歩行アニメーションのスプライト切り替え時間

    // 初期化関数(PlayerController.csから呼出)
    public void Init(PlayerController _playerController)
    {
        // 参照取得
        playerController = _playerController;
        spriteRenderer = playerController.GetComponent<SpriteRenderer>();
    }

    // Update
    void Update()
    {
        // 歩行アニメーション時間を経過(横移動している間のみ)
        if (Mathf.Abs(playerController.xSpeed) > 0.0f)
            walkAnimationTime += Time.deltaTime;
        // 歩行アニメーションコマ数を計算
        if (walkAnimationTime >= WalkAnimationSpan)
        {
            walkAnimationTime -= WalkAnimationSpan;
            // コマ数を増加
            walkAnimationFrame++;
            // コマ数が歩行アニメーション枚数を越えているなら0に戻す
            if (walkAnimationFrame >= WalkAnimationNum)
                walkAnimationFrame = 0;
        }

        // 歩行アニメーション更新
        spriteRenderer.sprite =
            walkAnimationRes[walkAnimationFrame];
    }
}