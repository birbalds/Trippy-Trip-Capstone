using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windows : MonoBehaviour {

	public CurrentPlayer status;
	public AudioSource vacuumSound;
	public Transform[] sliders;
	public float openPosition = -0.03f;
	bool anyWindowOpen = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!status.onDrugs) {
			anyWindowOpen = false;

			foreach (Transform slider in sliders) {
				if (slider && slider.localPosition.x < openPosition && !status.onDrugs) {
					anyWindowOpen = true;
				}
			}

			if (anyWindowOpen) {
				vacuumSound.volume = 0.05f;
			} else {
				vacuumSound.volume = 0.009f;
			}
		}
	}
}
