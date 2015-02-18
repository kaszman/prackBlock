using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{

	public GameObject player;
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

		//change color while timer is still running
		if (timerRunning)
		{
			player.light.color = Color.blue;
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
