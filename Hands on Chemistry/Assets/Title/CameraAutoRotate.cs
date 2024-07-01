using UnityEngine;

public class CameraAutoRotate : MonoBehaviour
{
    public float rotationSpeed = 10f; // 回転速度
    public Transform anchorPoint; // アンカーポイント

    void Update()
    {
        if (anchorPoint != null)
        {
            // アンカーポイントを中心にカメラを回転
            transform.RotateAround(anchorPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}

