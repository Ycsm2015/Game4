using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{

	public GameObject CannonPrefab;
	public GameObject Prefab_FireEffect;

	public int cannonNum;
	public float cannonSpeed;

	public float fireCooling;
	public float fireReloadTime;

	public float fireAngle = 0.1f;
	public Transform LeftSideCannonPoint;
	public Transform RightSideCannonPoint;


	public CannonInitPositionLimit limit;
	[System.Serializable]
	public class CannonInitPositionLimit
	{
		public float maxY;
		public float minY;
		public float maxZ;
		public float minZ;
	}
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey (KeyCode.Space) && fireCooling <= 0) {
			fireCooling = fireReloadTime;
			fire ();
		}
		fireCooling -= Time.deltaTime;
	}

	void fire ()
	{
		for (int i = 0; i < cannonNum; i++) {
			GameObject cannonGO = Instantiate (CannonPrefab);
			GameObject fireEffect = Instantiate (Prefab_FireEffect);
			Transform cannon = cannonGO.transform;
			Transform effect = fireEffect.transform;
            cannonGO.GetComponent<CannonController>().attacker = gameObject;
			//position

			float y = Random.Range (.05f, .25f);
			float z = Random.Range (-.8f, .8f);
			Vector3 fireDirection = transform.right;

			switch (i % 2) {
			case 0:
				cannon.position = LeftSideCannonPoint.position + LeftSideCannonPoint.rotation * new Vector3 (0, y, z);
				fireDirection = -fireDirection;
				break;
			case 1:
				cannon.position = RightSideCannonPoint.position + RightSideCannonPoint.rotation * new Vector3 (0, y, z);
				break;
			}

			effect.position = cannon.position;
			fireDirection += transform.up * fireAngle;
			cannon.GetComponent<Rigidbody> ().AddForce (fireDirection * cannonSpeed);
		}

	}

    public void attack(GameObject obj)
    {
        float damage = 1f;
        obj.GetComponent<EnemyController>().takeDamage(damage);
    }
}

