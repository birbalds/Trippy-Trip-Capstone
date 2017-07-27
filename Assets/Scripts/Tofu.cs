using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tofu : MonoBehaviour {

	public GameObject tofu; 
	public float speed = 0.05f;

	private Vector3 startPosition;
	private Vector3 endPosition;
	private AudioSource tofuSound;
	// Use this for initialization
	void Start () {
		startPosition = tofu.transform.position;
		tofuSound = tofu.GetComponent<AudioSource> ();
		endPosition = startPosition;
		endPosition.y += 2;

	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localPosition.x > -0.03f) {
			tofu.transform.position = Vector3.Lerp (startPosition, endPosition, speed * Time.deltaTime);
			TofuSurprise (false);
		} else {
			tofu.transform.position = Vector3.Lerp (endPosition, startPosition, speed * Time.deltaTime);
			TofuSurprise (true);
		}
	}

	void TofuSurprise (bool openStatus){
		if (openStatus && !tofuSound.isPlaying) {
			tofuSound.Play ();
		} else if (!openStatus && tofuSound.isPlaying) {
			tofuSound.Stop ();
		}
	}
}
