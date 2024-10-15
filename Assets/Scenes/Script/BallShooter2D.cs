using UnityEngine;

public class BallShooter2D : MonoBehaviour
{
    public GameObject ballPrefab;  // ボールのプレハブ
    public Transform spawnPoint;   // ボールの発射位置
    public float shootForce = 500f; // 発射力
    private GameObject currentBall; // 現在発射されたボールの参照
    public float stopThreshold = 0.1f;
    public float destroyDelay = 2f;  // 停止後消えるまでの時間

    private bool ballDestroying = false; // ボールが削除中かどうかのフラグ

    void Update()
    {
        // マウス左クリックでボールを発射
        if (Input.GetMouseButtonDown(0) && currentBall == null && !ballDestroying)
        {
            ShootBall();
        }

        // 発射されたボールがある場合
        if (currentBall != null)
        {
            Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();
            if (rb != null && rb.velocity.magnitude < stopThreshold)
            {
                // ボールが停止したら、削除フラグを立て削除
                ballDestroying = true;
                Destroy(currentBall, destroyDelay);
                Invoke(nameof(ResetCurrentBall), destroyDelay); 
            }
        }
    }

    void ShootBall()
    {
        // ボールを発射位置に生成
        currentBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(spawnPoint.right * shootForce);
        }

        // ボールが画面外に出たときに削除される処理を追加
        BallDisappearOnExit disappear = currentBall.AddComponent<BallDisappearOnExit>();
        disappear.destroyDelay = destroyDelay;

        ballDestroying = false; // 新しいボールが発射されたので、削除フラグをリセット
    }

    void ResetCurrentBall()
    {
        currentBall = null;
        ballDestroying = false; 
    }
}
// ボールが画面外に出たときに削除
public class BallDisappearOnExit : MonoBehaviour
{
    public float destroyDelay = 0f;  // 消えるまでの時間
     void OnBecameInvisible()
    {
        Destroy(gameObject, destroyDelay);
    }
}