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
            damage = 10;
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].name == currentWeapon)
                {
                    weapons[i].SetActive(true);
                    damage = weapons[i].GetComponent<weaponAttribute>().damage;
                }
            }
            Debug.Log("Current weapon is " + currentWeapon);
        }
    }
}
