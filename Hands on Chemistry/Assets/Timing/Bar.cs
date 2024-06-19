using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingGame : MonoBehaviour
{
    private float moveSpeed = 2.0f;
    private int direction = 1;
    private bool isGameActive = false;
    private Vector3 initialPosition;

    public delegate void GameEndedCallback(bool success);
    public GameEndedCallback OnGameEnded;

    // �ǉ��F�֘A����I�u�W�F�N�g
    public GameObject meter;
    public GameObject hitZone;

    // �ǉ��F�������̃I�[�f�B�I�\�[�X
    public AudioSource successAudioSource;

    // Game2��Game3�p�̃t���O
    public bool isGame2 = false;
    public bool isGame3 = false;

    // �q�b�g����͈̔́i��Ƃ��ď����l�� -0.7f ���� 0.7f �ɐݒ�j
    private float hitRangeMin = -0.7f;
    private float hitRangeMax = 0.7f;

    void Start()
    {
        initialPosition = transform.position; // �����ʒu���L��

        // Game2�̏ꍇ�A�ړ����x�ƕ����𒲐�
        if (isGame2)
        {
            moveSpeed = 3.0f; // ��Ƃ��Ĉړ����x��3.0�ɐݒ�
            direction = -1; // ��Ƃ��ċt�����Ɉړ�
            // Game2�p�̃q�b�g����͈͂�ݒ肷��ꍇ
            hitRangeMin = -2.4f;
            hitRangeMax = -1.0f;
        }
        // Game3�̏ꍇ�A�ړ����x�ƃq�b�g����͈͂𒲐�
        else if (isGame3)
        {
            moveSpeed = 1.5f; // ��Ƃ��Ĉړ����x��1.5�ɐݒ�
            direction = 1; // ��Ƃ��Ēʏ�����Ɉړ�
            // Game3�p�̃q�b�g����͈͂�ݒ肷��ꍇ
            hitRangeMin = 1.0f;
            hitRangeMax = 2.0f;
        }
    }

    void FixedUpdate()
    {
        if (isGameActive)
        {
            if (transform.position.y >= 2.0f)
                direction = -1;
            if (transform.position.y <= -2.0f)
                direction = 1;
            transform.position = new Vector3(initialPosition.x,
                transform.position.y + moveSpeed * Time.fixedDeltaTime * direction, initialPosition.z);
        }
    }

    private void Update()
    {
        if (isGameActive && Input.GetKeyUp(KeyCode.Space))
        {
            if (transform.position.y >= hitRangeMin && transform.position.y <= hitRangeMax)
            {
                Debug.Log("Hit");
                successAudioSource.Play(); // ���������Đ�
                EndGame(true);
            }
            else
            {
                Debug.Log("Miss");
                EndGame(false);
            }
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        transform.position = initialPosition; // �����ʒu�Ƀ��Z�b�g
        SetGameObjectsActive(true); // �Q�[���֘A�̃I�u�W�F�N�g���A�N�e�B�u��
        Debug.Log(gameObject.name + " has started.");
    }

    private void EndGame(bool success)
    {
        isGameActive = false;
        Debug.Log(gameObject.name + " is ending.");
        SetGameObjectsActive(false); // �Q�[���֘A�̃I�u�W�F�N�g���\����
        OnGameEnded?.Invoke(success);
    }

    private void RetryGame()
    {
        // �Q�[�������g���C���鏈��
        StartGame(); // �Q�[�����ĊJ����
    }

    public void SetGameObjectsActive(bool isActive) // ���\�b�h��public�ɕύX
    {
        gameObject.SetActive(isActive);
        if (meter != null) meter.SetActive(isActive);
        if (hitZone != null) hitZone.SetActive(isActive);
    }
}
