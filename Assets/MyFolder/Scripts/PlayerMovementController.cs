using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float forwardSpeed;
    public float turningSpeed;

    public bool backwardEnable = false;
    // Use this for initialization
    void Start()
    {
        IsUseOar();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void IsUseOar()
    {
        backwardEnable = true;
    }
    void FixedUpdate()
    {
        MakeMovementWithMouse();
        MakeMovement();
    }

    void MakeMovement()
    {
        float moveForward = Input.GetAxis("Vertical");
        float turnDirection = Input.GetAxis("Horizontal");

        //forward
        if (moveForward >= 0 || backwardEnable)
        {
            Vector3 movement = moveForward * forwardSpeed * transform.forward;
            transform.GetComponent<Rigidbody>().velocity = movement;
        }

        //turning
        transform.Rotate(0, turnDirection * turningSpeed, 0);
        //transform.GetComponent<Rigidbody>().MoveRotation
    }

    void MakeMovementWithMouse()
    {

    }
}
