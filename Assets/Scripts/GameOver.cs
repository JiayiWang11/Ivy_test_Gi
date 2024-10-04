using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOver : MonoBehaviour
{
	
	Button restartBtn;
	Button finishBtn;
	private void Awake()
	{
		Cursor.lockState = CursorLockMode.None;
		string prefix = "";
		if (GameManager.isVictory)
		{
			transform.Find("victory").gameObject.SetActive(true);
			transform.Find("fail").gameObject.SetActive(false);
			prefix = "victory/";
			finishBtn = transform.Find(prefix+"FinishBtn").GetComponent<Button>();
			finishBtn.onClick.AddListener(RestartGame);
		}
		else
		{
			prefix = "fail/";
			transform.Find("fail").gameObject.SetActive(true);
			transform.Find("victory").gameObject.SetActive(false);
			restartBtn = transform.Find(prefix+"RestartBtn").GetComponent<Button>();
			finishBtn = transform.Find(prefix+"FinishBtn").GetComponent<Button>();
			restartBtn.onClick.AddListener(RestartGame);
			finishBtn.onClick.AddListener(RestartGame);
		}
	}
	private void RestartGame()
	{
		SceneManager.LoadScene("GameScene");
	}
}
