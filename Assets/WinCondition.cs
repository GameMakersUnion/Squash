using UnityEngine;
using System.Collections;

public class WinCondition : MonoBehaviour {

	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(Vector3.Distance(player.transform.position, gameObject.transform.position)) < .25f){
			Application.LoadLevel (Application.loadedLevel + 1);
		}
	}
}
