using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{

	public SpriteRenderer player;
	public string tagCanCollide;
	public int timerLengthSec;
	public Text ramDisplay;

	protected bool ramming = false;

	private GameObject coco;
	private LevelSelect selector;
	private float timerRam;
	private int ramAmount;

	void Start()
	{
		ramAmount = 5;
		if (Application.isMobilePlatform)
		{
			ramDisplay.canvas.scaleFactor = 3f;
		}
	}

	void FixedUpdate()
	{		
		timerRam -= Time.deltaTime;

		if (timerRam <= 0)
		{
			ramming = false;
			player.color = Color.white;
		}

		player.color = Color.white;

		//start ramming
		if (Input.GetKeyDown(KeyCode.E) && timerRam <= 0 && ramAmount > 0)
		{	
			ramming = true;
			timerRam = timerLengthSec;
			ramAmount -= 1;
		}

		if (ramming == true)
		{
			player.color = Color.blue;
		}

		handleCollisionEvents();
		ramDisplay.text = "Rams: " + ramAmount;
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
