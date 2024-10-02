using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public GameObject deathEffectPrefab; // 死亡粒子效果预制件
    public HealthBar healthBar;

    public GameObject weaponPrefab;  // NPC持有的武器预制体

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
        // 播放死亡粒子效果
        if (deathEffectPrefab != null)
        {
            GameObject obj = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(obj, 5f);  // 5 秒后销毁粒子效果
        }

        DropWeapon();

        Destroy(gameObject);
    }

    void DropWeapon()
    {
        if (weaponPrefab != null)
        {
            // 实例化武器
            GameObject droppedWeapon = Instantiate(weaponPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

            droppedWeapon.transform.localScale = Vector3.one * 0.7f;
        
            // 获取武器的 Rigidbody
            Rigidbody weaponRigidbody = droppedWeapon.GetComponent<Rigidbody>();
            if (weaponRigidbody != null)
            {
                // 确保武器使用重力
                weaponRigidbody.useGravity = true;

                // 随机施加一些旋转或扭矩
                Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 10f;
                weaponRigidbody.AddTorque(randomTorque, ForceMode.Impulse);

                // 可选：可以施加一个向外的轻微力，使武器有掉落的效果
                Vector3 dropForce = transform.forward * 2f + transform.up * 1f; // 可以根据需要调整力度
                weaponRigidbody.AddForce(dropForce, ForceMode.Impulse);
            }
        
            Debug.Log("Dropped weapon at " + transform.position);
        }
    }   

}
