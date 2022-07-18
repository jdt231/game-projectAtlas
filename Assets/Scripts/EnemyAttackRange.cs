using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] Enemy parentEnemy;
    public bool isAttacking = false;
    public Collider2D attackRangeBox;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
 
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isAttacking)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        isAttacking = true;
        parentEnemy.isMoving = false;
        animator.SetTrigger("attack");
    }
}
