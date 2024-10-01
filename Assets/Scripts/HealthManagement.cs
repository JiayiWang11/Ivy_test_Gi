using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 200;
    public int currentHealth;
    public GameObject deathEffectPrefab; // 死亡粒子效果预制件
    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.SetHealth(currentHealth); // 更新血条UI

        if (currentHealth <= 0)
        {
            Die(); // 玩家死亡时调用
        }
        Debug.Log("current health is :" + currentHealth);
    }

    void Die()
    {
        // 移除玩家标签，避免其他物体继续检测
        gameObject.tag = "Untagged";

        // 通知 GameManager 玩家已经死亡
        GameManager gameManager = FindObjectOfType<GameManager>();  // 查找场景中的 GameManager
        if (gameManager != null)
        {
            gameManager.PlayerDied();  // 通知 GameManager 进行游戏结束逻辑
        }

        // 播放死亡粒子效果
        if (deathEffectPrefab != null)
        {
            GameObject obj = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(obj, 5f);  // 5 秒后销毁粒子效果
        }

        // 停止玩家行为（可以选择销毁或禁用玩家对象）
        gameObject.SetActive(false); // 将玩家对象设置为不可见和禁用
    }
}
