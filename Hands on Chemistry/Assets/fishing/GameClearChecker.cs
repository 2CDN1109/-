using UnityEngine;

public class GameClearChecker : MonoBehaviour
{
    public GameObject myBox; // �������������I�u�W�F�N�g
    public GameObject point; // �Q�[���N���A�̖ڕW�ƂȂ�I�u�W�F�N�g
    public float overlapTimeNeeded = 5f; // �Q�[���N���A�ɕK�v�ȏd�Ȃ莞�ԁi�b�j

    private float overlapTime = 0f;
    private bool isOverlapping = false;

    void Update()
    {
        // myBox��point�̏d�Ȃ�𔻒�
        if (myBox != null && point != null)
        {
            if (myBox.GetComponent<Renderer>().bounds.Intersects(point.GetComponent<Renderer>().bounds))
            {
                // �d�Ȃ��Ă���ꍇ
                overlapTime += Time.deltaTime;
                if (overlapTime >= overlapTimeNeeded && !isOverlapping)
                {
                    // �Q�[���N���A����
                    Debug.Log("Game Clear!");
                    isOverlapping = true;
                    // �����ɃQ�[���N���A���̏�����ǉ�����i��F���̃V�[���ɑJ�ڂ���A���b�Z�[�W��\������Ȃǁj
                }
            }
            else
            {
                // �d�Ȃ��Ă��Ȃ��ꍇ
                overlapTime = 0f;
                isOverlapping = false;
            }
        }
    }
}
