using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxHealth;
    public float maxMorale;
    public float health;
    public float morale;

    public int cannonNum = 20;
    public float cannonSpeed = 500f;
    
    public float fireCooling = 0f;
    public float fireReloadTime = 2f;

    public float fireAngle = 0.1f;
    public float fireRange = 500f;

    public GameObject Player;
    public GameObject CannonPrefab;
    public GameObject Prefab_FireEffect;

    public Transform LeftSideCannonPoint;
    public Transform RightSideCannonPoint;
    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        move();

        fireCooling -= Time.deltaTime;
        if (fireCooling <= 0)
        {
            float distance = Vector3.Distance(Player.transform.position, transform.position);
            float angle;
            
            if(distance <= fireRange)
            {
                fire();
                fireCooling = fireReloadTime;
            }
     

        }
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        updateUI();
    }
    public void fire()
    {


        for (int i = 0; i < cannonNum; i++)
        {
            GameObject cannonGO = Instantiate(CannonPrefab);
            GameObject fireEffect = Instantiate(Prefab_FireEffect);
            Transform cannon = cannonGO.transform;
            Transform effect = fireEffect.transform;
            cannonGO.GetComponent<CannonController>().attacker = gameObject;
            //position

            float y = Random.Range(.05f, .25f);
            float z = Random.Range(-.8f, .8f);
            Vector3 fireDirection = transform.right;

            switch (i % 2)
            {
                case 0:
                    cannon.position = LeftSideCannonPoint.position + LeftSideCannonPoint.rotation * new Vector3(0, y, z);
                    fireDirection = -fireDirection;
                    break;
                case 1:
                    cannon.position = RightSideCannonPoint.position + RightSideCannonPoint.rotation * new Vector3(0, y, z);
                    break;
            }

            effect.position = cannon.position;
            fireDirection += transform.up * fireAngle;
            cannon.GetComponent<Rigidbody>().AddForce(fireDirection * cannonSpeed);
        }


    }
    public void move()
    {

    }
    void updateUI()
    {
        PlayerUIController uic = GetComponent<PlayerUIController>();
        uic.setHealth(health / maxHealth);
        uic.setMorale(morale / maxMorale);
    }
}
