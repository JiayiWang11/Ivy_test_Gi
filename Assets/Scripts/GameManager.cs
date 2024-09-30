using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 确保在场景切换时不会销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerDied()
    {
        // 切换到游戏结束场景
        SceneManager.LoadScene("GameOverScene");
    }
}
