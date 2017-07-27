using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RepairDoor : MonoBehaviour {

		private GameObject knob;
		private HingeJoint doorHinge;
		private JointLimits hingeLimits;
		private BoxCollider doorCollider;
		
		public GameObject bossCreature;
		public GameObject[] immediateDestroy;
		public Material bossSkybox;
		public bool isDoorFixed = false;
		public GameObject door;
		public GameObject brokenKnob;
		public CurrentPlayer status;
		public VRTK_SnapDropZone knobObjective;
	// Use this for initialization
	void Start () {
			doorCollider = door.GetComponent<BoxCollider> ();
			doorCollider.enabled = false;
			knob = GameObject.Find ("NormalDoor/Handles/Fixed Knob");
			doorHinge = door.GetComponent<HingeJoint> ();
			hingeLimits = doorHinge.limits;
		
			hingeLimits.max = 0;
			doorHinge.limits = hingeLimits;
			knobObjective.gameObject.SetActive (false);
		}
	
	// Update is called once per frame
	void Update () {
			if (knobObjective.GetCurrentSnappedObject() == brokenKnob && knobObjective) {
				DoorFixed ();
			}

			if (status.onDrugs && knobObjective) {
				knobObjective.gameObject.SetActive(true);
			}
		}


		void DoorFixed (){
			bossCreature.SetActive (true);
			doorCollider.enabled = true;
			RenderSettings.skybox = bossSkybox;
			
		foreach (GameObject objectDestroy in immediateDestroy) {
			Destroy (objectDestroy);
		}
			knob.SetActive (true);
			hingeLimits.max = 105;
			doorHinge.limits = hingeLimits;

			Destroy(brokenKnob);
			Destroy(knobObjective);
			isDoorFixed = true;
		}
}