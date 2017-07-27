using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureShredder : MonoBehaviour {

	public Transform[] spawnPoints;

	void OnCollisionEnter (Collision collider){
		if (collider.gameObject.layer == 9) {
			Debug.Log (collider.gameObject.name);
			int spawnPointIndex = Random.Range (0, spawnPoints.Length);
			collider.gameObject.transform.position = spawnPoints [spawnPointIndex].position;
			collider.gameObject.transform.rotation = spawnPoints [spawnPointIndex].rotation; 
		}

	}

}
