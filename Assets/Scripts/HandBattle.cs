using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBattle : MonoBehaviour
{
    public Animator animator;  // 指向包含Animator组件的对象
    public string currentWeapon;

    void Update()
    {
        currentWeapon = GetComponent<WeaponAttack>().currentWeapon;
        if (Input.GetMouseButtonDown(0))  // 当鼠标左键被按下
        {
            Attack();
        }
    }

    void Attack()

    {
        if (currentWeapon == "fist") {
            animator.SetBool("hit1", true);
        }
        else if (currentWeapon == "Hatchet") {
            animator.SetTrigger("Axe");
        } else if (currentWeapon == "magicBook") {
            Debug.Log("magic");
            animator.SetTrigger("Magic");
        }
        animator.SetBool("hit2", true);// 设置Animator的attack1参数为true，开始攻击动画
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return null;  // 等待直到下一帧
        animator.SetBool("hit1", false);
        animator.SetBool("hit2", false);// 将attack1重置为false，确保动画可以再次触发
    }
}
