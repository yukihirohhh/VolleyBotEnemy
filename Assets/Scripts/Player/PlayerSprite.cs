using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�N�^�[�̃X�v���C�g��ݒ肷��N���X
/// </summary>
public class PlayerSprite : MonoBehaviour
{
    private PlayerController playerController; // �A�N�^�[����N���X
    private SpriteRenderer spriteRenderer; // �A�N�^�[��SpriteRenderer

    // �摜�f�ގQ��
    public List<Sprite> walkAnimationRes; // ���s�A�j���[�V����(������*�R�}��)

    // �e��ϐ�
    private float walkAnimationTime; // ���s�A�j���[�V�����o�ߎ���
    private int walkAnimationFrame; // ���s�A�j���[�V�����̌��݂̃R�}�ԍ�

    // �萔��`
    private const int WalkAnimationNum = 3; // ���s�A�j���[�V������1��ނ�����̖���
    private const float WalkAnimationSpan = 0.3f; // ���s�A�j���[�V�����̃X�v���C�g�؂�ւ�����

    // �������֐�(PlayerController.cs����ďo)
    public void Init(PlayerController _playerController)
    {
        // �Q�Ǝ擾
        playerController = _playerController;
        spriteRenderer = playerController.GetComponent<SpriteRenderer>();
    }

    // Update
    void Update()
    {
        // ���s�A�j���[�V�������Ԃ��o��(���ړ����Ă���Ԃ̂�)
        if (Mathf.Abs(playerController.xSpeed) > 0.0f)
            walkAnimationTime += Time.deltaTime;
        // ���s�A�j���[�V�����R�}�����v�Z
        if (walkAnimationTime >= WalkAnimationSpan)
        {
            walkAnimationTime -= WalkAnimationSpan;
            // �R�}���𑝉�
            walkAnimationFrame++;
            // �R�}�������s�A�j���[�V�����������z���Ă���Ȃ�0�ɖ߂�
            if (walkAnimationFrame >= WalkAnimationNum)
                walkAnimationFrame = 0;
        }

        // ���s�A�j���[�V�����X�V
        spriteRenderer.sprite =
            walkAnimationRes[walkAnimationFrame];
    }
}