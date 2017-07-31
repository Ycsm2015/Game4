using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitWave : MonoBehaviour {
	public float initCycle;
	public GameObject wavePrefabs;
	private float currentTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		if (currentTime > initCycle) {
			currentTime = 0f;
			GameObject wave = Instantiate (wavePrefabs);
			wave.transform.position = transform.position;
			wave.transform.rotation = transform.rotation;
			//wave.transform.parent = transform;
		}
	}
}
