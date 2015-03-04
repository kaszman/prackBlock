using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour
{

	public SpriteRenderer player;
	public string tagCanCollide;
	public int timerLength;

	protected bool ramming = false;

	private GameObject coco;
	private LevelSelect selector;
	private float timerLeft;

	void FixedUpdate()
	{		
		timerLeft -= Time.deltaTime;

		if (timerLeft <= 0)
		{
			ramming = false;
			player.color = Color.white;
		}

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
	}

void OnCollisionEnter2D(Collision2D col) 
	{			
		if (ramming)
		{
			coco = col.gameObject;
		}
		//Destroy(col.gameObject);
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
}
