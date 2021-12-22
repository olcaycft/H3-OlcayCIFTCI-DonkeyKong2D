using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private Vector2 direction;
    public float moveSpeed = 3f;

    public float jumpStr = 4f;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStr;
        } else
        {
            direction += Physics2D.gravity * Time.deltaTime; //if im not jumping use gravity on me in every seconds
        }
        direction.x = Input.GetAxis("Horizontal")*moveSpeed;
        direction.y = Math.Max(direction.y,-1f); //the issue is when im not jumping gravity is applying every second. with that i cant move. i need to declare max arrange of gravity when im at ground

        
        if (direction.x>0f)
        {
            transform.eulerAngles = Vector3.zero;
        }else if (direction.x<0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f); //if mario going to -x direction turning his rotation
        }
    }

    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + direction * Time.fixedDeltaTime); //when fixed update work apply position.
    }
}
