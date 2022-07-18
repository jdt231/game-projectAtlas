using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBox : MonoBehaviour
{
    private enum AttacksWhat { Enemy, Player };
    [SerializeField] public PlayerController player;
    [SerializeField] private AttacksWhat attacksWhat;
    [SerializeField] private int attackPower = 10;
    [SerializeField] private int targetSide;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If i'm at an X position than what I'm attacking, then I'm on the left side (-1).
        // Otherwise I'm on the right side (1).

        if (transform.parent.transform.position.x < collision.transform.position.x)
        { targetSide = -1; }
        else
        { targetSide = 1; }

        // if the player attacks an enemy, hurt the enemy.
        if (attacksWhat == AttacksWhat.Enemy)
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                collision.gameObject.GetComponent<Enemy>().HurtEnemy(player.attackPower);
            }
        }
        // else if the enemy attacks the player, hurt the player.
        else if (attacksWhat == AttacksWhat.Player)
        {
            if (collision.gameObject == player.gameObject)
            {
                player.HurtPlayer(attackPower, targetSide);
            }
        }
    }
}
