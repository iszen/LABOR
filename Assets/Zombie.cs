﻿using UnityEngine;
using NewtonVR;

public class Zombie : MonoBehaviour, Enemy
{
    public float walkSpeed = 3f;
    public float beginHitDistance = 3f;
    public Transform playerPosition;
    public HealthBarController playerHealth;

    public int damage = 20;
    public float secondsBetweenHits = 2;

    private Animator animator;
    private Transform moveTarget;
    private bool walking = false;
    private bool attackingPlayer = false;
    private bool playerWasAttacked = false;

    private float timeCounter = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (walking)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, walkSpeed * Time.deltaTime);
            transform.LookAt(moveTarget);
        }

        if (attackingPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPosition.position.x, transform.position.y, playerPosition.position.z), walkSpeed * Time.deltaTime);
            transform.LookAt(new Vector3(playerPosition.position.x, transform.position.y, playerPosition.position.z));
        }
        if (attackingPlayer && Vector3.Distance(transform.position, playerPosition.position) < beginHitDistance)
        {
            //Begin hitting the player
            animator.SetTrigger("attack");
            walking = false;
            attackingPlayer = false;
            transform.LookAt(new Vector3(playerPosition.position.x, transform.position.y, playerPosition.position.z));

            timeCounter += Time.deltaTime;
            if (timeCounter > secondsBetweenHits)
            {
                timeCounter = 0f;
                playerHealth.decreaseHealth(damage);
            }
        }
        else if (attackingPlayer == false && walking == false && playerWasAttacked == true)
        {
            Attack();
        }
    }

    public void WalkTo(Transform target)
    {
        animator.SetTrigger("walk");
        walking = true;
        moveTarget = target;
    }

    public void Die()
    {
        animator.SetTrigger("die");
        Destroy(gameObject, 10f);
        walking = false;
        attackingPlayer = false;
        playerWasAttacked = false;
    }

    public void Attack()
    {
        playerWasAttacked = true;
        animator.SetTrigger("walk");
        walking = false;
        attackingPlayer = true;
    }
}