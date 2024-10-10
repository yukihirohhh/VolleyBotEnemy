using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f; // 敵の移動速度
    public float chaseDistance = 5f; // プレイヤーを追いかけ始める距離
    public GameObject player; // プレイヤーオブジェクト
    private bool facingRight = true; // 初期では右を向いている

    private void Update()
    {
        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // 距離が追跡範囲内かどうかを確認
        if (distanceToPlayer < chaseDistance)
        {
            // プレイヤーの方向を計算
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // 敵をプレイヤーの方向に移動させる
            transform.Translate(direction * speed * Time.deltaTime);

            // 進行方向に応じて向きを変える
            if (direction.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (direction.x < 0 && facingRight)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        // 向きを反転する
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; // x方向のスケールを反転
        transform.localScale = theScale;
    }
}
