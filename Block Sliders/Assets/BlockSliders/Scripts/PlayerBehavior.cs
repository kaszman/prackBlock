using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{

	public SpriteRenderer player;
	private GameObject coco;
	private LevelSelect selector;
	protected bool ramming = false;
	public string tagCanCollide;
	public int timerLength;
	private int timerLeft;

	void FixedUpdate()
	{
		player.color = Color.white;

		//start ramming
		if (Input.GetKey(KeyCode.E))
		{	
			ramming = true;
			timerLeft = timerLength;
		}


		if (ramming == true)
		{
			player.color = Color.blue;
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
			}
		}
	}

	

void timerSeconds()
	{			
			if (timerLeft >= 0)
			{
				timerLeft--;
			}
			else
			{
				ramming = false;
			player.color = Color.white;
			}

	}
}
