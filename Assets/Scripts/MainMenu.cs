using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public Button endGame;
	public Button startGame;
	// Use this for initialization
	void Start () {
		endGame.onClick.AddListener (End);
		startGame.onClick.AddListener (Begin);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void End()
	{
		Application.Quit ();
	}

	void Begin()
	{
        int index = SceneManager.GetActiveScene().buildIndex + 1;

        if (index >= SceneManager.sceneCountInBuildSettings)
        {
            index = 0;
        }

		SceneManager.LoadScene (index);
	}
}
