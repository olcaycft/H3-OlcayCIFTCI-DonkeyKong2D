using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Animator animator;
    //private string currentState;

    private new Rigidbody2D rigidbody2D;
    private Vector2 direction;

    private Collider2D[] results;
    private new Collider2D collider;
    
    private bool grounded;
    private bool climbing;
    private bool smashing;

    public float moveSpeed = 3f;
    public float jumpStr = 4f;
    
    //ANIMATION STATES
    private const string MARIO_IDLE = "Mario_idle";
    private const string MARIO_RUN = "Mario_run";
    private const string MARIO_JUMP = "Mario_jump";
    private const string MARIO_CLIMB = "Mario_climb";
    private const string MARIO_SMASH = "Mario_smashrun";

    [SerializeField] private float smashDelay = 6f;

    //private GameManager gm;

    private void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4]; //we can control 4 colliders2d in a time

        //gm = new GameManager();
    }
    
    private void OnEnable() //when mario enable
    {
        InvokeRepeating(nameof(AnimateSprite),1f/12f,1f/12f);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    
    private void ChangeAnimationState(string newState)
    {
        //if (currentState == newState) return;
        
        animator.Play(newState);
        //currentState = newState;
    }

   

    private void CollisonChecker()
    {
        grounded = false;
        climbing = false;
        Vector2 size = collider.bounds.size;  
        size.x /= 2f; //this will helps for more realistic, we will climb when our bodys half touch the ladder
        size.y += 0.1f; //this will helps for overlapping
        
        
        int amount = Physics2D.OverlapBoxNonAlloc(transform.position,size,0f,results); //this overlapbox method no consume ram alloc
        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;
            if (hit.layer==LayerMask.NameToLayer("Ground"))
            {
                grounded = (hit.transform.position.y < (this.transform.position.y - 0.5f)) && !climbing; //grounded will be true if ground y position lower than mario's half size. and mario dont climbing
                
                Physics2D.IgnoreCollision(collider,results[i],!grounded); // if mario jump we are ignore collision bcs of for dont hit mario's head to top.
            }
            if (hit.layer==LayerMask.NameToLayer("Ladder") && !smashing)
            {
                climbing = true;
            }
            if (hit.layer==LayerMask.NameToLayer("LadderDown") && !smashing)
            {
                if(Input.GetAxis("Vertical")<0f) //if when player on ladderdown collider and if hits the down button.
                {
                    climbing = true;
                }
                
            }
            if (hit.layer==LayerMask.NameToLayer("Hammer"))
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                smashing = true;
                size.x /= 2f;
                Invoke(nameof(SmashingComplete),smashDelay);
            }
         
        }
    }
    private void Update()
    {
        CollisonChecker();
        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical")*moveSpeed;
        }
        else if (grounded && Input.GetButtonDown("Jump") && !smashing)
        {
            direction = Vector2.up * jumpStr;
        } 
        else
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

    private void AnimateSprite()
    {
        if (climbing)
        {
            ChangeAnimationState(MARIO_CLIMB);
        }
        else if (direction.x != 0f && grounded && !smashing) //if mario moving
        {
            ChangeAnimationState(MARIO_RUN);
        }
        else if (!grounded) //jumping animation
        {
            ChangeAnimationState(MARIO_JUMP);
        }
        else if (smashing)
        {
                ChangeAnimationState(MARIO_SMASH);
        }
        else//idle animation
        {
            ChangeAnimationState(MARIO_IDLE);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Objective"))
        {
            enabled = false;
            //gm.LevelComplete(); //i think i couldnt use like this bcs of GameManager in(related) GameObject or bcs of MonoBehaviour.
            FindObjectOfType<GameManager>().LevelComplete();    //****** this is not a big deal bcs of this is a little game but what can i use instead of this.******
        }
        else if (col.gameObject.CompareTag("Obstacle"))
        {
            enabled = false;
            //gm.LevelFailed(); //i think i couldnt use like this bcs of GameManager in(related) GameObject or bcs of MonoBehaviour.
            FindObjectOfType<GameManager>().LevelFailed();      //****** this is not a big deal bcs of this is a little game but what can i use instead of this.******
        }
    }

    private void SmashingComplete()
    {
        smashing = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

}
