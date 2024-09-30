using UnityEngine;

public class RockingChair : MonoBehaviour
{
    public enum RockDirection { ForwardBackward, LeftRight } // 摇晃方向枚举
    public RockDirection rockingDirection = RockDirection.ForwardBackward; // 摇晃方向

    public float speed = 1.0f; // 摇晃的速度
    public float maxAngle = 15.0f; // 最大摇晃角度

    public Transform player; // 玩家角色的Transform
    public float hearingDistance = 5.0f; // 玩家听到声音的距离
    private AudioSource rockingSound; // 椅子的音频源

    private float time;
    private Quaternion initialRotation; // 记录初始旋转

    void Start()
    {
        // 保存初始的旋转角度
        initialRotation = transform.localRotation;

        // 获取AudioSource组件
        rockingSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 计算摇晃角度
        float angle = Mathf.Sin(time * speed) * maxAngle;

        // 根据摇晃方向调整椅子的旋转
        if (rockingDirection == RockDirection.ForwardBackward)
        {
            // 前后摇晃，影响X轴
            transform.localRotation = initialRotation * Quaternion.Euler(angle, 0, 0);
        }
        else if (rockingDirection == RockDirection.LeftRight)
        {
            // 左右摇晃，影响Z轴
            transform.localRotation = initialRotation * Quaternion.Euler(0, 0, angle);
        }

        // 计算玩家与椅子之间的距离
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 如果玩家在可听距离内，开始播放或调整音量
        if (distanceToPlayer <= hearingDistance)
        {
            if (!rockingSound.isPlaying)
            {
                rockingSound.Play(); // 播放摇晃声音
            }

            // 根据距离调整音量，距离越近音量越大
            rockingSound.volume = 1.0f - (distanceToPlayer / hearingDistance);
        }
        else
        {
            // 如果玩家超出范围，停止播放声音
            rockingSound.Stop();
        }

        // 更新时间
        time += Time.deltaTime;
    }
}
