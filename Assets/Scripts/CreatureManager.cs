using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : MonoBehaviour {

	public GameObject[] creatures;
	public CurrentPlayer status;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (status.tookPill) {
			SummonCreatures ();
		}

		if (status.alreadyOpened) {
			foreach (GameObject creature in creatures) {
				Destroy (creature);
			}
		}
	}

	void LateUpdate(){
		status.tookPill = false;
	}

	void SummonCreatures(){
		StartCoroutine(MyMethod());
	}

	IEnumerator MyMethod() {
		foreach (GameObject creature in creatures) {
			yield return new WaitForSeconds(3);
			creature.SetActive (true);
		}
	}

}
