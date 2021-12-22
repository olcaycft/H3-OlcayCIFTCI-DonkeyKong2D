using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private Vector2 direction;

    private Collider2D[] results;
    private new Collider2D collider;
    private bool grounded;
    
    public float moveSpeed = 3f;

    public float jumpStr = 4f;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4]; //we can control 4 colliders2d in a time
    }

    private void ColliderChecker()
    {
        grounded = false;
        Vector2 size = collider.bounds.size;  
        size.x /= 2f; //this will helps for more realistic, we will climb when our bodys half touch the ladder
        size.y += 0.1f; //this will helps for overlapping
        
        
        int amount = Physics2D.OverlapBoxNonAlloc(transform.position,size,0f,results); //this overlapbox method no consume ram alloc
        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;
            if (hit.layer==LayerMask.NameToLayer("Ground"))
            {
                grounded = hit.transform.position.y < (this.transform.position.y - 0.5f); //grounded will be true if ground y position lower than mario's half size.
                
                Physics2D.IgnoreCollision(collider,results[i],!grounded); // if mario jump we are ignore collision bcs of for dont hit mario's head to top.
            }
                
            
        }
    }
    private void Update()
    {
        ColliderChecker();
        
        if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStr;
        } else
        {
            direction += Physics2D.gravity * Time.deltaTime; //if im not jumping use gravity on me in every seconds
        }
        direction.x = Input.GetAxis("Horizontal")*moveSpeed;
        if (grounded) 
        {
            direction.y = Math.Max(direction.y,-1f); //the issue is when im not jumping gravity is applying every second. with that i cant move. i need to declare max arrange of gravity when im at ground
        }
        
        
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
