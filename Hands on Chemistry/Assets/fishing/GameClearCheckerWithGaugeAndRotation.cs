using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearCheckerWithGaugeAndRotation : MonoBehaviour
{
    public GameObject myBox; // �������������I�u�W�F�N�g
    public GameObject point; // �Q�[���N���A�̖ڕW�ƂȂ�I�u�W�F�N�g
    public GameObject forceps; // Y��]�𑝉�������I�u�W�F�N�g
    public GameObject ProgressSlider;
    public Slider progressSlider; // �Q�[�W�p�̃X���C�_�[
    public float overlapTimeNeeded = 5f; // �Q�[���N���A�ɕK�v�ȏd�Ȃ莞�ԁi�b�j
    public float rotationSpeed = 10f; // Y��]�̑������x

    private float overlapTime = 0f;

    public JudgeMovieCtrl judgeMovieCtrl;
    private bool gameCleared = false; // �Q�[���N���A�̃t���O

    void Start()
    {
        ProgressSlider.SetActive(true);
    }

    void Update()
    {
        // myBox��point�̏d�Ȃ�𔻒�
        if (myBox != null && point != null && !gameCleared)
        {
            if (myBox.GetComponent<Renderer>().bounds.Intersects(point.GetComponent<Renderer>().bounds))
            {
                // �d�Ȃ��Ă���ꍇ
                overlapTime += Time.deltaTime;

                // forceps��Y��]�𑝉�������
                RotateForceps();

                if (overlapTime >= overlapTimeNeeded)
                {
                    // �Q�[���N���A����
                    gameCleared = true; // �Q�[���N���A�̃t���O�𗧂Ă�
                    Debug.Log("Game Clear!");
                    // �Q�[���N���A���̏����������ɒǉ�
                    judgeMovieCtrl.JudgmentObj[0].SetActive(true);
                    judgeMovieCtrl.videoPlayers[0].Play();
                    Invoke("ShowFinish", 2.5f);
                    Invoke("GoTitle", 21.5f);
                }
            }
            else
            {
                // �d�Ȃ��Ă��Ȃ��ꍇ
                if (overlapTime > 0)
                {
                    overlapTime -= Time.deltaTime *2;
                }
                else
                {
                    overlapTime = 0f;
                }
                
            }

            // �Q�[�W���X�V
            UpdateGauge();
        }
    }

    // �Q�[�W���X�V���郁�\�b�h
    void UpdateGauge()
    {
        if (progressSlider != null)
        {
            progressSlider.value = overlapTime / overlapTimeNeeded;
        }
    }

    // forceps��Y��]�𑝉������郁�\�b�h
    void RotateForceps()
    {
        if (forceps != null)
        {
            Vector3 rotation = forceps.transform.rotation.eulerAngles;
            rotation.y += rotationSpeed * Time.deltaTime;
            forceps.transform.rotation = Quaternion.Euler(rotation);
        }
    }

    void ShowFinish() 
    {
        ProgressSlider.SetActive(false);
        judgeMovieCtrl.JudgmentObj[1].SetActive(true);
        judgeMovieCtrl.videoPlayers[1].Play();
    }

    void GoTitle()
    {
        SceneManager.LoadScene("title");
    }
}
