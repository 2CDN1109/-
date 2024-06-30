using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWobble : MonoBehaviour
{
    public float amplitude = 0.5f; // �h��̕�
    public float frequency = 1.0f; // �h��̑���

    private Vector3 initialPosition;

    void Start()
    {
        // �����ʒu��ۑ�
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // �e�L�X�g��Y���W��h�炷
        float newY = initialPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
