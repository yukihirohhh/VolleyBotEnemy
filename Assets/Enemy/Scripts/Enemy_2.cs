using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f; // �G�̈ړ����x
    public float chaseDistance = 5f; // �v���C���[��ǂ������n�߂鋗��
    public GameObject player; // �v���C���[�I�u�W�F�N�g
    private bool facingRight = true; // �����ł͉E�������Ă���

    private void Update()
    {
        // �v���C���[�Ƃ̋������v�Z
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // �������ǐՔ͈͓����ǂ������m�F
        if (distanceToPlayer < chaseDistance)
        {
            // �v���C���[�̕������v�Z
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // �G���v���C���[�̕����Ɉړ�������
            transform.Translate(direction * speed * Time.deltaTime);

            // �i�s�����ɉ����Č�����ς���
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
        // �����𔽓]����
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; // x�����̃X�P�[���𔽓]
        transform.localScale = theScale;
    }
}
