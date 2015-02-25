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
	private float timerLeft = 3;

	void FixedUpdate()
	{		
		timerLeft -= Time.deltaTime*10;
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
}
