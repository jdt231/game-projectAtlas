using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployController : MonoBehaviour
{
    public Collider2D collider2d;
    public PlayerController player;
    public LayerMask layerMask;

    public bool CheckforObstruction()
    {
        if (collider2d.IsTouchingLayers(LayerMask.GetMask("Ground"))) // Can add further layers: ("Ground", "Other", "Walls" etc)
        {
            //Debug.Log("Deploy point is obstructed");
            return true;
        }
        else
        {
            //Debug.Log("Deploy point is not obstructed");
            return false;
        }
    }
   
    public bool CheckforPlayer()
    {
        if (collider2d.IsTouching(player.GetComponent<Collider2D>()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}