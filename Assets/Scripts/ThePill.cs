using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThePill : MonoBehaviour {


	public CurrentPlayer player;
	// Use this for initialization

	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter(Collision collider) {

		if (collider.gameObject.name == "[CameraRig]" || collider.gameObject.name == "CenterEyeAnchor") {
			player.onDrugs = true;
			player.tookPill = true;
			player.pillCounter += 1;
			Destroy (gameObject);
			Debug.Log ("I'm on Drugs!!!");

		}
    }


}
