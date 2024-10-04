using UnityEngine;
using UnityEngine.SceneManagement;  // 用于场景管理

public class GameManager : MonoBehaviour
{
    public static bool isVictory = false;
    public void PlayerDied()
    {
        // 切换到游戏结束场景
        Debug.Log("PlayerDied() called, switching to GameOverScene.");
        SceneManager.LoadScene("GameOverScene");
        isVictory = false;
    }
}
