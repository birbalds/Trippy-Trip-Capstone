﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class psychosplooge: MonoBehaviour {

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

		wayPoint = new Vector3 (Random.Range(transform.position.x - Range, transform.position.x + Range), Random.Range(transform.position.y - Range, transform.position.y + Range), Random.Range(transform.position.z - Range, transform.position.z + Range));
		//wayPoint.y = 1;
		// don't need to change direction every frame seeing as you walk in a straight line only
		transform.LookAt(wayPoint);
	}

	void OnCollisionEnter (Collision collider){

		Debug.Log (collider.gameObject.name);

		if (collider.gameObject.name == "Floor") {
			wayPoint.y *= 1;
		}

		if (collider.gameObject.tag == "Inactive") {
			if (collider.gameObject.name == "Roof") {
				wayPoint.y *= -1;
			} else {
				wayPoint.x *= -1;
			}
		}
			
	}

}
