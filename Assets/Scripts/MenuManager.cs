using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void QuitGame()
	{
		Application.Quit();
	}

	public void StartTimeAttack()
	{
        var script = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        script.gameMode = GameManager.GameMode.TimeAttack;
        SceneManager.LoadScene("Main");
	}

    public void StartScoreAttack()
    {
        var script = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        script.gameMode = GameManager.GameMode.ScoreAttack;
        SceneManager.LoadScene("Main");
    }

    public void BackToMain()
	{
		SceneManager.LoadScene("TitleScreen");
	}

	public void LoadCredits()
	{
		SceneManager.LoadScene("Credits");
	}


}
