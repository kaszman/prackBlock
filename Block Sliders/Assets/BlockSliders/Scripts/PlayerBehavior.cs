using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{

	public SpriteRenderer player;
	GameObject coco;
	private LevelSelect selector;
	protected bool ramming = false;
	public string tagCanCollide;
	public int timerLength;
	private int timerLeft;
	protected bool timerRunning = false;

	void FixedUpdate()
	{

		//start ramming
		if (Input.GetKey(KeyCode.E))
		{	
			ramming = true;
			timerRunning = true;
			timerLeft = timerLength;
		}


		if (ramming == true)
		{
			player.color = Color.blue;
		}
		else
		{
			player.color = Color.white;
		}

		handleCollisionEvents();
		timerSeconds();
	}

void OnCollisionEnter2D(Collision2D col) 
	{			
		coco = new GameObject();

		if (ramming)
		{
			coco = col.gameObject;
		}
	}

void handleCollisionEvents()
	{
		if (ramming)
		{
			if (coco.tag == tagCanCollide)
			{
			Destroy(coco);
			ramming = false;
			timerRunning = false;
			}
		}
	}

	

void timerSeconds()
	{			
		if (timerRunning)
		{
			if (timerLeft > 0)
			{
				timerLeft--;
			}
			else
			{
				ramming = false;
				timerRunning = false;
			}
		}
	}
}
