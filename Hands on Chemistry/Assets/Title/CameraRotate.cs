using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float rotationSpeed = 10f; // ‰ñ“]‘¬“x

    void Update()
    {
        // AnchorPoint‚ÌˆÊ’u‚ğŠî€‚É‰ñ“]
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
