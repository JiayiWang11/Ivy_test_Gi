using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    public GameObject player; // 玩家对象
    public GameObject collectUI;
    public string itemName; // 物品的名称
    public float pickupDistance = 2f; // 允许拾取的距离
    public bool isWeapon = false; // 是否是武器

    private void Update()
    {
        
        // 计算玩家与物品之间的距离
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance <= pickupDistance)
        {
            collectUI.SetActive(true);
        }
        else if (distance > pickupDistance && distance < pickupDistance + 2)
        {
            collectUI.SetActive(false);
        }
        
        // 检查玩家是否在拾取范围内
        if (distance <= pickupDistance)
        {

            // 如果玩家按下 Q 键，尝试拾取物品
            if (Input.GetKeyDown(KeyCode.Q))
            {
                collectUI.SetActive(false);
                // 如果物品是药品，直接恢复玩家的生命值
                if (itemName == "medicine")
                {
                    player.GetComponent<PlayerHealth>().currentHealth = 200;
                    player.GetComponent<PlayerHealth>().healthBar.SetHealth(200);
                    Destroy(gameObject);
                    return;
                }
                PlayerPack pack = player.GetComponent<PlayerPack>();
                if (pack != null)
                {
                    if (!pack.IsFull())
                    {
                        // 将物品添加到玩家的背包
                        pack.AddItem(itemName, isWeapon);
                        
                        Destroy(gameObject); // 拾取后销毁物品
                    }
                }
            }
        }
    }
}
