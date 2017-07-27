using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlayer : MonoBehaviour {

	public bool alreadyOpened = false;
    public bool onDrugs = false;
	public bool tookPill = false;
	public GameObject door;
	public GameObject wall;
	public string[] allTags;
	public AudioClip[] allMusic;

	public SphereCollider steamCameraCollider;


	private Shader floatShader;
	private List<GameObject> allObjects;
	private AudioSource onMusic;
	public int pillCounter = 0;

	Hashtable ht = new Hashtable ();

	void Awake(){
		ht.Add ("alpha", 0);
		ht.Add ("time", 80.0f);
	}

	// Use this for initialization
	void Start () {
		onMusic = GetComponent<AudioSource> ();
		onMusic.clip = allMusic [0];
		onMusic.volume = 0.009f;
		floatShader = Shader.Find("Transparent/Diffuse");
		Shader.EnableKeyword("TBT_LINEAR_TARGET");
		allObjects = FindGameObjectsWithTags (allTags);
	}
	
	// Update is called once per frame
	void Update () { 
		if (steamCameraCollider.isTrigger) {
			steamCameraCollider.isTrigger = false;
		}

		if (!alreadyOpened && door.transform.eulerAngles.y > 45 + 270) {
			Debug.Log (door.transform.eulerAngles.y);
			beginBossBattle ();
		}
			
		if (tookPill) {
			onMusic.clip = allMusic [1];
			onMusic.volume = 0.5f;
			onMusic.Play();
		}
	}

	List<GameObject> FindGameObjectsWithTags (string[] tags){
		List<GameObject> gameObjectList = new List<GameObject>();
		for (int i=0; i < tags.Length; i++){
			GameObject[] taggedObjectList = GameObject.FindGameObjectsWithTag(tags[i]);
			gameObjectList.AddRange(taggedObjectList);
		}
		return gameObjectList;
	}

	void beginBossBattle(){
		
		onMusic.PlayOneShot(allMusic [2], 0.5f);

		onMusic.clip = allMusic [3];
		onMusic.volume = 0.1f;
		onMusic.Play();

		alreadyOpened = true;

		foreach (GameObject floatObject in allObjects) {
			if (floatObject) {
				Rigidbody floatRigidbody = floatObject.GetComponent<Rigidbody> ();
				Renderer rend = floatObject.GetComponent<Renderer> ();
				HingeJoint joint = floatObject.GetComponent<HingeJoint> ();

				if (floatRigidbody == null && rend) {
					floatObject.AddComponent<Rigidbody> ();
					floatRigidbody = floatObject.GetComponent<Rigidbody> ();
					floatRigidbody.useGravity = false;
					floatRigidbody.AddForce (0, 41, 44);
					rend.material.shader = floatShader;
					iTween.FadeTo (floatObject, ht);
				} else if (rend) {
					if (joint) {
						Destroy (joint);
					}
					floatRigidbody.constraints = RigidbodyConstraints.None;
					floatRigidbody.useGravity = false;
					floatRigidbody.AddForce (0, 34, 32);
					rend = floatObject.GetComponent<Renderer> ();
					rend.material.shader = floatShader;
					iTween.FadeTo (floatObject, ht);
				}
			}
		}

			StartCoroutine (MyMethod ());
	}

	IEnumerator MyMethod() {
		yield return new WaitForSeconds(30);
		foreach (GameObject floatObject in allObjects) {
			Destroy (floatObject);
		}
	}
}
