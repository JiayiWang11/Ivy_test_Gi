using System.Collections;
using TMPro;
using UnityEngine;

public class StoryTextController : MonoBehaviour
{
    public TextMeshProUGUI storyText;  // 关联 TextMeshPro 文本框
    public float minTypingSpeed = 0.02f;  // 打字速度的最小值
    public float maxTypingSpeed = 0.1f;   // 打字速度的最大值
    public float delayBetweenLines = 1f;  // 每行文字之间的延迟时间
    public string[] storyLines;  // 存储故事的每一行

    private void Start()
    {
        StartCoroutine(ShowStoryLineByLine());
    }

    IEnumerator ShowStoryLineByLine()
    {
        storyText.text = "";  // 清空初始文本框内容

        // 循环逐行显示文本
        foreach (string line in storyLines)
        {
            yield return StartCoroutine(TypeLine(line));  // 逐字显示当前行
            storyText.text += "\n\n";  // 添加换行，空出一行
            yield return new WaitForSeconds(delayBetweenLines);  // 等待设定的延迟时间再显示下一行
        }
    }

    IEnumerator TypeLine(string line)
    {
        // 清空当前行
        storyText.text = storyText.text.TrimEnd() + "\n"+ "\n"; // 保留前面的文本，加一个换行
        foreach (char letter in line.ToCharArray())
        {
            storyText.text += letter;  // 逐字添加到文本框中

            // 计算下一个字符的打字延迟时间
            float typingDelay = Random.Range(minTypingSpeed, maxTypingSpeed);  // 在最小和最大值之间随机
            yield return new WaitForSeconds(typingDelay);  // 等待动态的打字速度
        }
    }
}
