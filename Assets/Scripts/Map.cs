using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {
	public GameManager manager;
	public GameObject pipeObject;
	public GameObject groundObject;
	private List<GameObject> pipes = new List<GameObject>();
	private List<GameObject> ground = new List<GameObject>();
 	public float pipeOffset;
	public int loadThreshold;
	public int initPipes;
	public float moveSpeed;

	void Start()
	{
		for(int i = 0; i < initPipes; i++)
		{
			GameObject InsObject = (GameObject)Instantiate(pipeObject, new Vector2((float)i * pipeOffset + 15.0f, (float)Random.Range(-2, -7)), Quaternion.identity);
			InsObject.transform.parent = this.transform;
			pipes.Add(InsObject);
		}
		for(int i = 0; i < 30; i++)
		{
			GameObject InsObject = (GameObject)Instantiate(groundObject, new Vector2((float)(i + pipeOffset) - 8.0f, -8.0f), Quaternion.identity);
			InsObject.transform.parent = this.transform;
			ground.Add(InsObject);
		}
		rigidbody2D.velocity = new Vector2(-moveSpeed, 0.0f);
	}
	
	public void Generate()
	{
		addPipe();
		addGround();
	}

	void addPipe()
	{
		GameObject InsObject = (GameObject)Instantiate(pipeObject, new Vector2(pipes[pipes.Count - 1].transform.position.x + pipeOffset, (float)Random.Range(-2, -7)), Quaternion.identity);
		InsObject.transform.parent = this.transform;
		pipes.Add (InsObject);
		if(ScoreManager.getCurrentScore() > 4)
		{
			Destroy(pipes[0]);
			pipes.Remove(pipes[0]);
		}
	}

	void addGround()
	{
		for(int i = 0; i < pipeOffset; i++)
		{
			GameObject InsObject = (GameObject)Instantiate(groundObject, new Vector2((float)(ground[ground.Count - 1].transform.position.x + 1), -8.0f), Quaternion.identity);
			InsObject.transform.parent = this.transform;
			ground.Add(InsObject);
			Destroy(ground[0]);
			ground.Remove(ground[0]);
		}

	}
}
