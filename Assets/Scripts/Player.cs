using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float tapForce;
	public Map map;
	public GameManager manager;
	private bool bHalt;

	// Tap to Hop
	// Adding vector UpRight.
	void Update () 
	{
		if(Input.GetMouseButtonDown(0) && !bHalt && !(Camera.main.WorldToViewportPoint(transform.position).y > 1.0f))
		{
			rigidbody2D.velocity = new Vector2(0f, tapForce);
			transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(0f,0f,0f), Quaternion.Euler(0f,0f,90f), rigidbody2D.velocity.y);
		}else if(rigidbody2D.velocity.y < -.05)
		{
			transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(0f,0f,0f), Quaternion.Euler(0f,0f, -100.0f), -rigidbody2D.velocity.y * 4.0f);
		}
	}

	void OnCollisionEnter2D(Collision2D other) 
	{
		if( !bHalt ) Die();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		other.collider2D.enabled = false;
		ScoreManager.addCurrentScore(1);
		map.Generate();
	}

	void Die()
	{
		bHalt = true;
		map.rigidbody2D.velocity = Vector2.zero;
 		manager.showGameOver = true;
		ScoreManager.sendHighScore(ScoreManager.getCurrentScore());
	}
}
