using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject attacker = GetComponent<CannonController>().attacker;
        string tag = "Enemy";
        if (attacker.tag == "Enemy")
            tag = "Player";
        if (other.tag == tag)
        {
            if (attacker == null)
                Debug.Log("error:cannon have no attacker");
            else
            {
                if (attacker == GameObject.FindGameObjectWithTag("Player"))
                    attacker.GetComponent<PlayerAttackController>().attack(other.gameObject);
                else
                    attacker.GetComponent<AttackController>().attack(other.gameObject);
            }
            Destroy(gameObject);
        }

    }

}
