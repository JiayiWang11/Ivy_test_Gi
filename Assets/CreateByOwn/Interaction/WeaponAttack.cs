using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public Animator animator;  // 指向包含Animator组件的对象
    public List<string> weaponsList;
    public List<GameObject> weapons;
    public string currentWeapon;
    int currentWeaponIndex = 0;
    public int damage = 10;

    public float attackRange = 1f; // 动态获取武器的攻击范围
    public LayerMask npcLayer; // 用来过滤NPC的层

    public GameObject bloodEffectPrefab; // 溅血粒子效果的预制体

    private float attackCooldown = 1.5f; // 动态获取武器的冷却时间
    private float lastAttackTime = 0f;  // 记录上次攻击时间

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(false); // 初始化时隐藏所有武器
        }
        currentWeapon = "fist";
    }

    // Update is called once per frame
    void Update()
    {
        weaponsList = GetComponent<PlayerPack>().weaponsList;
        if (Input.GetKeyDown(KeyCode.E))
        {
            switchWeapon();
        }

        // 如果按下攻击键 (比如鼠标左键)，并且冷却时间已结束
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
        }
    }

    void switchWeapon()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(false);
        }
        if (weaponsList.Count > 1)
        {
            currentWeaponIndex++;
            if (currentWeaponIndex >= weaponsList.Count)
            {
                currentWeaponIndex = 0;
            }
            currentWeapon = weaponsList[currentWeaponIndex];
            damage = 30;
            attackRange = 2f; // 默认值
            attackCooldown = 1.5f; // 默认冷却时间

            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].name == currentWeapon)
                {
                    weapons[i].SetActive(true);
                    weaponAttribute weaponAttr = weapons[i].GetComponent<weaponAttribute>();
                    if (weaponAttr != null)
                    {
                        damage = weaponAttr.damage;
                        attackRange = weaponAttr.attackDistance;  // 获取武器的攻击距离
                        attackCooldown = weaponAttr.cooldownTime;  // 获取武器的冷却时间
                    }
                }
            }
            Debug.Log("Current weapon is " + currentWeapon + ", Attack range: " + attackRange + ", Cooldown time: " + attackCooldown);
        }
    }

    void PerformAttack()
    {
        // 以玩家为中心进行物理检测，transform.position 是玩家的当前位置
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, npcLayer);

        foreach (Collider hitCollider in hitColliders)
        {
            // 获取NPC的HealthManager组件
            EnemyHealth healthManager = hitCollider.GetComponent<EnemyHealth>();
            if (healthManager != null)
            {
                // 对NPC进行伤害
                healthManager.TakeDamage(damage);
                Debug.Log("Hit NPC: " + hitCollider.name + ", Dealt " + damage + " damage.");

                // 找到NPC的骨骼特定位置
                Transform spineTransform = hitCollider.transform.Find("Root/Hips/Spine_01/Spine_02");
                if (spineTransform != null)
                {
                    // 在骨骼位置播放溅血粒子效果
                    Instantiate(bloodEffectPrefab, spineTransform.position, Quaternion.identity);
                    Debug.Log("Played blood effect at " + spineTransform.name);
                }
                else
                {
                    Debug.LogWarning("Spine transform not found on " + hitCollider.name);
                }
            }
        }

        // 记录本次攻击的时间
        lastAttackTime = Time.time;
    }
}
