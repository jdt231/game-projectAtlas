using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PhysicsObject
{
    [Header("Attributes")]
    [SerializeField] private int attackPower = 10;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float trackingDistance = 10;
    public float currentSpeed;
    public bool isMoving = true;
    public bool isTrackingPlayer = false;
    public int health = 100;
    //private int maxHealth = 100;
    private int direction = 1;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyAttackRange enemyAttackRange;
    [SerializeField] private ParticleSystem particleEnemyExplosion;
    private Vector2 playerPosition;

    [Header("Raycasts")]
    [SerializeField] private LayerMask rayCastLayerMask; //which layer do we want the raycast to NOT interact with.
    [SerializeField] private Vector2 rayCastRightLedgeOffset; //Offset from the center of the raycast origin.
    [SerializeField] private Vector2 rayCastleftLedgeOffset; //Offset from the center of the raycast origin.
    [SerializeField] private Vector2 rayCastRightWallOffset; //Offset from the center of the raycast origin.
    [SerializeField] private Vector2 rayCastLeftWallOffset; //Offset from the center of the raycast origin.
    [SerializeField] private float rayCastLength = 2;
    private RaycastHit2D rightLedgeRaycastHit;
    private RaycastHit2D leftLedgeRaycastHit;
    private RaycastHit2D rightWallRaycastHit;
    private RaycastHit2D leftWallRaycastHit;

    [Header("Audio")]
    [SerializeField] private AudioClip soundHurt;
    [SerializeField] private float soundHurtVolume = 1;
    [SerializeField] private AudioClip soundDeath;
    [SerializeField] private float soundDeathVolume = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ControlLocalScale();
        ControlVelocity(); 
        ControlDeath();
        DrawRaycasts();
        TrackPlayer();
    }

    private void TrackPlayer()
    {
        playerPosition = new Vector2(NewPlayer.Instance.transform.position.x, NewPlayer.Instance.transform.position.y);
        //Debug.Log("Player can be found at: " + playerPosition);
        float distanceToPlayer = Vector2.Distance(playerPosition, transform.position);
        //Debug.Log("Player is: " + distanceToPlayer + " units far away from me");
        if (distanceToPlayer <= trackingDistance)
        {
            isTrackingPlayer = true;
            if (playerPosition.x < transform.position.x) 
            { direction = -1; }
            else
            { direction = 1; }
        }
        else
        {
            isTrackingPlayer = false;
        }
    }

    private void ControlLocalScale()
    {
        //flip the player's localscale.x if the move speed is greater than .01 of less than -01
        if (targetVelocity.x < -0.01)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (targetVelocity.x > 0.01)
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    private void ControlVelocity()
    {
        if (isMoving)
        {
            if (isTrackingPlayer)
            {
                currentSpeed = maxSpeed;
            }
            else
            {
                currentSpeed = maxSpeed / 2;
            }
        }
        else 
        {
            currentSpeed = 0;
        }

        targetVelocity = new Vector2(currentSpeed * direction, 0);
    }

    private void ControlDeath()
    {
        if (health <= 0)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            player.StartCoroutine(player.FreezeEffect(0.1f, 0.5f));
            player.sfxAudioSource.PlayOneShot(soundDeath, soundDeathVolume); // Not sure if correct, I'm playing a death sound from the player when the enemy is dying?
            player.cameraEffects.Shake(5, 0.2f); // Will need fixing in NEW BUILD
            particleEnemyExplosion.transform.parent = null;
            particleEnemyExplosion.gameObject.SetActive(true);
            Destroy(particleEnemyExplosion.gameObject, particleEnemyExplosion.main.duration);
            Destroy(gameObject);
        }
    }

    private void DrawRaycasts()
    {
        CheckRightLedge();
        CheckLeftLedge();
        CheckRightWall();
        CheckLeftWall();
    }

    private void CheckRightLedge()
    {
        //Check for right ledge
        rightLedgeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x + rayCastRightLedgeOffset.x, transform.position.y + rayCastRightLedgeOffset.y), Vector2.down, rayCastLength);

        Debug.DrawRay(new Vector2(transform.position.x + rayCastRightLedgeOffset.x, transform.position.y + rayCastRightLedgeOffset.y), Vector2.down * rayCastLength, Color.blue);

        if (rightLedgeRaycastHit.collider == null)
        { direction = -1; }
    }

    private void CheckLeftLedge()
    {
        //Check for left ledge
        leftLedgeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x - rayCastleftLedgeOffset.x, transform.position.y + rayCastleftLedgeOffset.y), Vector2.down, rayCastLength);

        Debug.DrawRay(new Vector2(transform.position.x - rayCastleftLedgeOffset.x, transform.position.y + rayCastleftLedgeOffset.y), Vector2.down * rayCastLength, Color.red);

        if (leftLedgeRaycastHit.collider == null)
        { direction = 1; }
    }

    private void CheckRightWall()
    {
        //Check for right wall
        rightWallRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x + rayCastRightWallOffset.x, transform.position.y + rayCastRightWallOffset.y), Vector2.right, rayCastLength, rayCastLayerMask);

        Debug.DrawRay(new Vector2(transform.position.x + rayCastRightWallOffset.x, transform.position.y + rayCastRightWallOffset.y), Vector2.right * rayCastLength, Color.green);

        if (rightWallRaycastHit.collider != null)
        {
            //Debug.Log("I'm touching something on my right: " + rightWallRaycastHit.collider.gameObject);
            direction = -1;
        }
    }

    private void CheckLeftWall()
    {
        //Check for left wall
        leftWallRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x + rayCastLeftWallOffset.x, transform.position.y + rayCastLeftWallOffset.y), Vector2.left, rayCastLength, rayCastLayerMask);

        Debug.DrawRay(new Vector2(transform.position.x + rayCastLeftWallOffset.x, transform.position.y + rayCastLeftWallOffset.y), Vector2.left * rayCastLength, Color.black);

        if (leftWallRaycastHit.collider != null)
        {
            //Debug.Log("I'm touching something on my left: " + leftWallRaycastHit.collider.gameObject);
            direction = 1;
        }
    }

    public void HurtEnemy(int attackPower)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        health -= attackPower;
        animator.SetTrigger("hurt");
        player.sfxAudioSource.PlayOneShot(soundHurt, soundHurtVolume);
    }

    public void StopAttack()
    {
        enemyAttackRange.isAttacking = false;
        isMoving = true;
    }
}
