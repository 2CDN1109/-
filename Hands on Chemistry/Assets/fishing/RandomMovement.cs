using UnityEngine;

public class SmoothRandomMovement : MonoBehaviour
{
    public float minX = 4.75f;          // X���W�̍ŏ��l
    public float maxX = 5.65f;          // X���W�̍ő�l
    public float moveDuration = 2.0f;   // �ړ��ɂ����鎞�ԁi�b�j

    private Vector3 startPosition;      // �ړ��J�n�ʒu
    private Vector3 targetPosition;     // �ړ��ڕW�ʒu
    private float startTime;            // �ړ��J�n����

    void Start()
    {
        // �����ʒu�̐ݒ�
        startPosition = transform.position;
        // �ړ��J�n�����̋L�^
        startTime = Time.time;
        // �����_���ȖڕW�ʒu�̐ݒ�
        float randomX = Random.Range(minX, maxX);
        targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        // ���݂̎��Ԃɑ΂���ړ��̐i���x�����i0����1�͈̔́j
        float progress = (Time.time - startTime) / moveDuration;
        // �ړ����������Ă��Ȃ��ꍇ
        if (progress < 1.0f)
        {
            // ���݂̈ʒu���Ԃ��Čv�Z
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
        }
        else
        {
            // �ړ������������玟�̖ڕW�ʒu��ݒ肵�čēx�ړ����J�n����
            float randomX = Random.Range(minX, maxX);
            startPosition = transform.position;
            targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);
            startTime = Time.time;
        }
    }
}
