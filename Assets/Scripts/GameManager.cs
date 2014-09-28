using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;

public class GameManager : MonoBehaviour {
	[HideInInspector] public Vector2 dialogSize;
	[HideInInspector]	public bool showGameOver;
	[SerializeField] private string _AppID;
	[SerializeField] private string _AppKey;

	// Initialing KiiCloud Lib. 
	// You need AppID & AppKey input Inspector.
	void Awake()
	{
		//Kii.Initialize( _AppID, _AppKey, Kii.Site.JP );		
	}

	void Start()
	{
	}

	void Update()
	{
		if (showGameOver)
		{
			ScoreManager.refreshHighScore();
		}
	}

	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width/2 - 43.0f,20.0f,200.0f,200.0f ), ScoreManager.getCurrentScore().ToString());
		if(showGameOver) showGameoverDialog();
	}
	void showGameoverDialog() 
	{
		Rect currentGameOver = new Rect(Screen.width/2 - (dialogSize.x/2), Screen.height/2 - (dialogSize.y/2), dialogSize.x, dialogSize.y);
		GUI.Box (currentGameOver, "Game Over");//, skin.GetStyle("Game Over"));
		GUI.Label(new Rect(currentGameOver.x + 32.0f, currentGameOver.y + 48.0f, currentGameOver.width, currentGameOver.height), "Score: " + ScoreManager.getCurrentScore().ToString());
		GUI.Label(new Rect(currentGameOver.x + 32.0f, currentGameOver.y + 64.0f, currentGameOver.width, currentGameOver.height), "High score: " + ScoreManager.getHighScore().ToString());
		if (GUI.Button (new Rect(currentGameOver.x + (currentGameOver.width*0.25f), currentGameOver.y + (currentGameOver.height - 96.0f), currentGameOver.width*0.5f, 32.0f), "REPLAY" )){
			Application.LoadLevel("Game");
		}
		if (GUI.Button (new Rect(currentGameOver.x + (currentGameOver.width*0.25f), currentGameOver.y + (currentGameOver.height - 48.0f), currentGameOver.width*0.5f, 32.0f), "EXIT" )){
			Application.LoadLevel("Title");
		}
	}
}
