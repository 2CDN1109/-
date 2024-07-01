using UnityEngine;

public class CameraAutoRotate : MonoBehaviour
{
    public float rotationSpeed = 10f; // ��]���x
    public Transform anchorPoint; // �A���J�[�|�C���g

    void Update()
    {
        if (anchorPoint != null)
        {
            // �A���J�[�|�C���g�𒆐S�ɃJ��������]
            transform.RotateAround(anchorPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}

