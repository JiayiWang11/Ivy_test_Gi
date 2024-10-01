using UnityEngine;

public class MagicShake : MonoBehaviour
{
    public float shakeIntensity = 1.0f;  // 抖动的强度（上下左右最大移动的距离）
    public float shakeSpeed = 2.0f;      // 抖动速度
    private Vector3 originalPosition;    // 记录对象的初始位置

    void Start()
    {
        // 记录对象的初始位置
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        // 随机生成抖动的偏移量
        float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0) * 2 - 1;  // 生成 -1 到 1 之间的随机值
        float offsetY = Mathf.PerlinNoise(0, Time.time * shakeSpeed) * 2 - 1;  // 生成 -1 到 1 之间的随机值

        // 设置新位置，保持原有位置的基础上加上抖动的偏移
        Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0) * shakeIntensity;
        transform.localPosition = originalPosition + shakeOffset;
    }
}
