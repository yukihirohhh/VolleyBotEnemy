using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player����E����N���X
/// </summary>
public class PlayerController : MonoBehaviour
{
    // �I�u�W�F�N�g�E�R���|�[�l���g�Q��
    private Rigidbody2D rigidbody2D; // Rigidbody2D�R���|�[�l���g�ւ̎Q��
    private SpriteRenderer spriteRenderer;
    private PlayerGroundSensor groundSensor; // �v���C���[�ڒn����N���X
    private PlayerSprite playerSprite; // �v���C���[�X�v���C�g�ݒ�N���X

    // �ړ��֘A�ϐ�
    [HideInInspector] public float xSpeed; // X�����ړ����x
    [HideInInspector] public bool rightFacing; // �����Ă������(true.�E���� false:������)

    // Start�i�I�u�W�F�N�g�L��������1�x���s�j
    void Start()
    {
        // �R���|�[�l���g�Q�Ǝ擾
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundSensor = GetComponentInChildren<PlayerGroundSensor>();
        playerSprite = GetComponent<PlayerSprite>();

        // �z���R���|�[�l���g������
        playerSprite.Init(this);

        // �ϐ�������
        rightFacing = true; // �ŏ��͉E����
    }

    // Update�i1�t���[�����Ƃ�1�x�����s�j
    void Update()
    {
        // ���E�ړ�����
        MoveUpdate();
        // �W�����v���͏���
        JumpUpdate();

        // �⓹�Ŋ���Ȃ����鏈��
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation; // Rigidbody�̋@�\�̂�����]�����͏�ɒ�~
        if (groundSensor.isGround && !Input.GetKey(KeyCode.UpArrow))
        {
            // �⓹��o���Ă��鎞�㏸�͂������Ȃ��悤�ɂ��鏈��
            if (rigidbody2D.velocity.y > 0.0f)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.0f);
            }
            // �⓹�ɗ����Ă��鎞���藎���Ȃ��悤�ɂ��鏈��
            if (Mathf.Abs(xSpeed) < 0.1f)
            {
                // Rigidbody�̋@�\�̂����ړ��Ɖ�]���~
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

    /// <summary>
    /// Update����Ăяo����鍶�E�ړ����͏���
    /// </summary>
    private void MoveUpdate()
    {
        // X�����ړ�����
        if (Input.GetKey(KeyCode.RightArrow))
        {// �E�����̈ړ�����
         // X�����ړ����x���v���X�ɐݒ�
            xSpeed = 6.0f;

            // �E�����t���Oon
            rightFacing = true;

            // �X�v���C�g��ʏ�̌����ŕ\��
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {// �������̈ړ�����
         // X�����ړ����x���}�C�i�X�ɐݒ�
            xSpeed = -6.0f;

            // �E�����t���Ooff
            rightFacing = false;

            //�X�v���C�g�����E���]���������ŕ\��
            //spriteRenderer.flipY = true;
        }
        else
        {// ���͂Ȃ�
         // X�����̈ړ����~
            xSpeed = 0.0f;
        }
    }

    /// <summary>
    /// Update����Ăяo�����W�����v���͏���
    /// </summary>
    private void JumpUpdate()
    {
        // �W�����v����
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {// �W�����v�J�n
            //�ڒn���Ă��Ȃ��Ȃ�I��
            if (!groundSensor.isGround)
                return;

         // �W�����v�͂��v�Z
            float jumpPower = 10.0f;
            // �W�����v�͂�K�p
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpPower);
        }
    }

    // FixedUpdate�i��莞�Ԃ��Ƃ�1�x�����s�E�������Z�p�j
    private void FixedUpdate()
    {
        // �ړ����x�x�N�g�������ݒl����擾
        Vector2 velocity = rigidbody2D.velocity;
        // X�����̑��x����͂��猈��
        velocity.x = xSpeed;

        // �v�Z�����ړ����x�x�N�g����Rigidbody2D�ɔ��f
        rigidbody2D.velocity = velocity;
    }
}