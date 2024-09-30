using UnityEngine;

public class FadeInGhostFace : MonoBehaviour
{
    public Transform player; // 玩家对象的Transform
    public float triggerDistance = 5.0f; // 触发淡入的距离
    public float fadeDuration = 2.0f; // 渐变的时间

    private Material material;
    private Color originalColor;
    private float fadeTimer = 0.0f;
    private bool hasFadedIn = false; // 标记淡入是否已完成

    void Start()
    {
        // 获取物体的材质实例（克隆材质，避免影响其他对象）
        material = GetComponent<Renderer>().material; // 自动实例化材质

        // 将材质的Rendering Mode设置为Fade
        SetMaterialToFadeMode(material);

        // 获取材质的初始颜色
        originalColor = material.color;

        // 将材质的透明度设为0（完全透明）
        Color transparentColor = originalColor;
        transparentColor.a = 0;
        material.color = transparentColor;
    }

    void Update()
    {
        // 如果已经淡入完成，则不再更新透明度
        if (hasFadedIn) return;

        // 检查玩家与鬼脸之间的距离
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 检查鬼脸是否在摄像机的视角内
        bool isInCameraView = GetComponent<Renderer>().isVisible;

        // 如果玩家接近并且鬼脸在摄像机视角内，开始淡入
        if (distanceToPlayer <= triggerDistance && isInCameraView)
        {
            // 如果淡入还没完成
            if (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime; // 更新时间
                float alpha = Mathf.Lerp(0, originalColor.a, fadeTimer / fadeDuration); // 计算当前透明度

                // 设置材质颜色的透明度
                Color currentColor = material.color;
                currentColor.a = alpha;
                material.color = currentColor;
            }
            else
            {
                // 一旦淡入完成，标记为已完成
                hasFadedIn = true;

                // 确保透明度设置为完全不透明
                Color finalColor = material.color;
                finalColor.a = originalColor.a;
                material.color = finalColor;

                // 将材质的Rendering Mode设置为Opaque（如果不需要再保持透明）
                SetMaterialToOpaqueMode(material);
            }
        }
    }

    // 设置材质的Rendering Mode为Fade
    void SetMaterialToFadeMode(Material mat)
    {
        mat.SetFloat("_Mode", 2); // 设置为Fade模式 (2 是Fade，3 是Transparent)
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }

    // 设置材质的Rendering Mode为Opaque（不透明）
    void SetMaterialToOpaqueMode(Material mat)
    {
        mat.SetFloat("_Mode", 0); // 设置为Opaque模式
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = -1;
    }
}
