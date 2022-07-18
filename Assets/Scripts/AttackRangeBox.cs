using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeBox : MonoBehaviour
{
    [SerializeField] public PlayerAttackBox attackBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If i'm at an X position than what I'm attacking, then I'm on the left side (-1).
        //Otherwise I'm on the right side (1).


        //if I touch an enemy, hurt the enemy.
       /*
        if (enemyParent.GetComponent<> == AttacksWhat.Enemy)
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                collision.gameObject.GetComponent<Enemy>().HurtEnemy(NewPlayer.Instance.attackPower);
            }
        }
        else if (attacksWhat == AttacksWhat.Player)
        {
            if (collision.gameObject == NewPlayer.Instance.gameObject)
            {
                AttackPlayer();
                NewPlayer.Instance.HurtPlayer(attackPower, targetSide);
            }
        }

 */
    }

}
