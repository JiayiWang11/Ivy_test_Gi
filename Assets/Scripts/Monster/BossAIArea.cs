using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIArea : MonoBehaviour
{
    public BossAI bossAI;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bossAI.OnEnterArea();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossAI.OnExitArea();
        }
    }
}
