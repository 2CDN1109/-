using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float rotationSpeed = 10f; // ��]���x

    void Update()
    {
        // AnchorPoint�̈ʒu����ɉ�]
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
