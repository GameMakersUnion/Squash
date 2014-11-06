using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public enum State{
		Cube,
		Horiz,
		Vert
	}
	
	bool orientation;
	bool isTurning = false;
	bool isLanded = true;
	Vector3 direction;
	private float tempSpeed = 5f;
	GameObject player;
	GameObject spr;
	State state = State.Cube;




	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		spr = GameObject.Find("Sprite");
	}
	
	// Update is called once per frame
	void Update () {

		if(isLanded){
			if (player.rigidbody2D.velocity.magnitude > .005)
			{
				isLanded = false;
				Invoke ("checkLanded" , .25f);
			}

		}

		if (!isTurning)
		{
			if(isLanded){
				if (Input.GetKeyDown (KeyCode.LeftArrow))
				{
					direction = new Vector3(0,0,90);
					isTurning = true;
					player.rigidbody2D.gravityScale=0;

				}
				
				if (Input.GetKeyDown (KeyCode.RightArrow))
				{
					direction = new Vector3(0,0,-90);
					isTurning = true;
					player.rigidbody2D.gravityScale=0;
				}
			}

		}

		else {
						
			float TimesYouWillRotate = 100f/tempSpeed;
			float RotIncrement = 90f / TimesYouWillRotate;
			transform.Rotate(direction, RotIncrement,Space.World);
			float z = gameObject.transform.rotation.eulerAngles.z;

			if (Mathf.Abs(z -0) <1 || Mathf.Abs(z -90) <1 || Mathf.Abs(z - 180) <1 || Mathf.Abs(z - 270) <1)
			{
				player.rigidbody2D.gravityScale=1;
				isTurning = false;
				if (Mathf.Abs(z -0) <1  || Mathf.Abs(z - 180) <1 ){
					orientation = true;
				}
				else{
					orientation = false;
				}
				if (state == State.Horiz && orientation){
					player.layer = 9;
					Debug.Log ("setting to vertical");
					
				}
				else if (state == State.Vert && !orientation){
					player.layer = 8;
					Debug.Log ("setting to horiz");
					
				}
				else{
					player.layer = 0;
					Debug.Log ("setting to null");
					
				}
			}

			//Debug.Log(gameObject.transform.rotation.eulerAngles);
		}

	}

	void checkLanded()
	{
		if (player.rigidbody2D.velocity.magnitude < .005)
		{

			float z = gameObject.transform.rotation.eulerAngles.z;
			
			if (Mathf.Abs(z -0) <1  || Mathf.Abs(z - 180) <1 ){
				isLanded = true;
				if (state == State.Cube){
					state = State.Vert;
					spr.transform.localScale = new Vector3(1,0.5f,1);
				}
				if (state == State.Horiz){
					state = State.Cube;
					spr.transform.localScale = Vector3.one;
					
				}
				return;
			}

			if	(Mathf.Abs(z -90) <1 || Mathf.Abs(z - 270) <1)
			{
				isLanded = true;
				if (state == State.Cube){
					state = State.Horiz;
					spr.transform.localScale = new Vector3(0.5f,1,1);

				}
				if (state == State.Vert){
					state = State.Cube;
					spr.transform.localScale = Vector3.one;
					
				}
				return;
			}



		}
			Invoke ("checkLanded", .25f);
	}

}
