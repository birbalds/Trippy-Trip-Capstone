using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCreature : MonoBehaviour {

	public int Speed= 20;
	public Vector3 wayPoint;
	public int Range= 10;

	void Start(){
		//initialise the target way point
		Wander();
	}

	void Update() 
	{
		//transform.Rotate (0,0,20*Time.deltaTime);
		// this is called every frame
		// do move code here
		transform.position += transform.TransformDirection(Vector3.forward)*Speed*Time.deltaTime;
		if((transform.position - wayPoint).magnitude < 1)
		{
			// when the distance between us and the target is less than 3
			// create a new way point target
			Wander();


		}
	}

	void Wander()
	{ 
		// does nothing except pick a new destination to go to

		wayPoint = new Vector3 (Random.Range(transform.position.x - Range, transform.position.x + Range), 0.5f, Random.Range(transform.position.z - Range, transform.position.z + Range));
		wayPoint.y = 0.5f;
		// don't need to change direction every frame seeing as you walk in a straight line only
		var targetRotation = Quaternion.LookRotation(wayPoint - transform.position);

		// Smoothly rotate towards the target point.
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Speed * Time.deltaTime);
		transform.LookAt(wayPoint);
		//Debug.Log(wayPoint + " and " + (transform.position - wayPoint).magnitude);
	}

	void OnCollisionEnter (Collision collider){

		if (collider.gameObject.tag == "Inactive") {
			wayPoint.x *= -1;
		}
	}

}
