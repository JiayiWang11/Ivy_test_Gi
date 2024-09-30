using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryScreen : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown) // 检测任意按键或鼠标点击
        {
            SceneManager.LoadScene("MainGameScene"); // 加载主游戏场景
        }
    }
}
