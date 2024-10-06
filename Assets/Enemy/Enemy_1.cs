using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zako1 : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ肷��
    [Header("�ړ����x")] public float speed;
    [Header("�d��")] public float gravity;
    [Header("��ʊO�ł��s������")] public bool nonVisibleAct;
    [Header("�����]���̊Ԋu�i�b�j")] public float changeDirectionInterval = 2f;
    #endregion

    #region//�v���C�x�[�g�ϐ�
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
