using UnityEngine;

public class BallShooter2D : MonoBehaviour
{
    public GameObject ballPrefab;  // �{�[���̃v���n�u
    public Transform spawnPoint;   // �{�[���̔��ˈʒu
    public float shootForce = 500f; // ���˗�
    private GameObject currentBall; // ���ݔ��˂��ꂽ�{�[���̎Q��
    public float stopThreshold = 0.1f;
    public float destroyDelay = 2f;  // ��~�������܂ł̎���

    private bool ballDestroying = false; // �{�[�����폜�����ǂ����̃t���O

    void Update()
    {
        // �}�E�X���N���b�N�Ń{�[���𔭎�
        if (Input.GetMouseButtonDown(0) && currentBall == null && !ballDestroying)
        {
            ShootBall();
        }

        // ���˂��ꂽ�{�[��������ꍇ
        if (currentBall != null)
        {
            Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();
            if (rb != null && rb.velocity.magnitude < stopThreshold)
            {
                // �{�[������~������A�폜�t���O�𗧂č폜
                ballDestroying = true;
                Destroy(currentBall, destroyDelay);
                Invoke(nameof(ResetCurrentBall), destroyDelay); 
            }
        }
    }

    void ShootBall()
    {
        // �{�[���𔭎ˈʒu�ɐ���
        currentBall = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(spawnPoint.right * shootForce);
        }

        // �{�[������ʊO�ɏo���Ƃ��ɍ폜����鏈����ǉ�
        BallDisappearOnExit disappear = currentBall.AddComponent<BallDisappearOnExit>();
        disappear.destroyDelay = destroyDelay;

        ballDestroying = false; // �V�����{�[�������˂��ꂽ�̂ŁA�폜�t���O�����Z�b�g
    }

    void ResetCurrentBall()
    {
        currentBall = null;
        ballDestroying = false; 
    }
}
// �{�[������ʊO�ɏo���Ƃ��ɍ폜
public class BallDisappearOnExit : MonoBehaviour
{
    public float destroyDelay = 0f;  // ������܂ł̎���
     void OnBecameInvisible()
    {
        Destroy(gameObject, destroyDelay);
    }
}