using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWobble : MonoBehaviour
{
    public float amplitude = 0.5f; // 揺れの幅
    public float frequency = 1.0f; // 揺れの速さ

    private Vector3 initialPosition;

    void Start()
    {
        // 初期位置を保存
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // テキストのY座標を揺らす
        float newY = initialPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
