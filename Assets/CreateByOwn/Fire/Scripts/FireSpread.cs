using System.Collections;
using UnityEngine;

public class FireSpread : MonoBehaviour
{
    public Transform[] firePoint;          // 火焰点位置数组
    public float spreadInterval = 5f;      // 火焰扩散的时间间隔
    public Transform player;               // 玩家对象
    public Transform castle;               // 城堡对象（可以是中心点）
    public float disableDistance = 50f;    // 玩家离开城堡后火焰停止的距离
    public float damageRadius = 3f;        // 火焰影响的半径
    public int fireDamage = 20;            // 火焰对玩家造成的伤害
    public float damageInterval = 1f;      // 掉血的时间间隔
    private float damageTimer = 0f;        // 计时器
    private int index = 0;
    private bool isSpreading = false;      // 判断火焰是否正在扩散
    private HealthManager playerHealth;    // 引用玩家的HealthManager脚本

    private void Start()
    {
        // 获取玩家身上的 HealthManager 脚本
        playerHealth = player.GetComponent<HealthManager>();
        if (playerHealth == null)
        {
            Debug.LogError("玩家缺少 HealthManager 组件！");
        }

        // 可以选择在Start方法中开始扩散
        // FireSpreadStart();
    }

    /// <summary>
    /// 火焰扩散
    /// </summary>
    void FireSpreadStart()
    {
        if (!isSpreading)   // 防止重复启动扩散协程
        {
            isSpreading = true;
            StartCoroutine(FireSpreadCoroutine());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FireSpreadStart();
        }
    }

    private IEnumerator FireSpreadCoroutine()
    {
        while (true)
        {
            // 检查玩家是否离城堡太远
            float distanceToCastle = Vector3.Distance(player.position, castle.position);

            if (distanceToCastle > disableDistance)
            {
                StopFireSpread();
                yield break;   // 退出协程，停止火焰扩散
            }

            if (index >= firePoint.Length)
            {
                yield break;   // 如果所有火焰点都激活，则退出
            }

            // 激活下一个火焰点
            firePoint[index].gameObject.SetActive(true);

            // 每帧检查玩家与火焰点的距离并对玩家造成伤害
            CheckPlayerDistanceAndDamage(firePoint[index]);

            index++;

            // 等待设定的扩散间隔时间
            yield return new WaitForSeconds(spreadInterval);
        }
    }

    // 停止火焰扩散和火焰点的渲染
    private void StopFireSpread()
    {
        StopAllCoroutines();  // 停止所有正在进行的协程

        // 禁用所有火焰点
        foreach (Transform point in firePoint)
        {
            point.gameObject.SetActive(false);
        }

        index = 0;   // 重置火焰扩散索引
        isSpreading = false;
    }

    private void Update()
    {
        // 实时监控玩家与城堡的距离，自动停止火焰扩散
        float distanceToCastle = Vector3.Distance(player.position, castle.position);
        if (distanceToCastle > disableDistance)
        {
            StopFireSpread();
        }
    }

    // 检查玩家与火焰点的距离，并对玩家造成伤害
    private void CheckPlayerDistanceAndDamage(Transform fire)
    {
        // 检查玩家与当前火焰点的距离
        float distance = Vector3.Distance(player.position, fire.position);
        if (distance <= damageRadius)
        {
            // 当玩家在火焰范围内时，开始掉血
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                playerHealth.TakeDamage(fireDamage);
                damageTimer = 0f; // 重置计时器
                Debug.Log("玩家受到了火焰伤害：" + fireDamage);
            }
        }
        else
        {
            // 如果玩家离开火焰范围，重置计时器
            damageTimer = 0f;
        }
    }
}
