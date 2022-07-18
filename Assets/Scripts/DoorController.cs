using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public enum CanEnter { PlayerOnly, WalkerOnly, Both };
    public CanEnter canEnter;

    public bool isPlayerInDoor = false;
    public bool isWalkerInDoor = false;
    public bool isAllowedToEnter = false;
    public bool facePlayerRight = true;

    [SerializeField] SpriteRenderer myRenderer;
    [SerializeField] string levelTarget;
    [SerializeField] string spawnTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInDoor || isWalkerInDoor)
        {
            ProcessKeyPress();
        }
    }

    private void ProcessKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isAllowedToEnter)
            {
                LevelManager.Instance.LoadLevel(levelTarget, spawnTarget);
            }
            else
            {
                if (canEnter == CanEnter.PlayerOnly)
                {
                    Debug.Log("Only the player can enter through this door");
                }
                else if (canEnter == CanEnter.WalkerOnly)
                {
                    Debug.Log("You must be in the Walker to enter this door");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (canEnter == CanEnter.PlayerOnly || canEnter == CanEnter.Both)
            {
                if (myRenderer) 
                { 
                    myRenderer.enabled = true;
                    myRenderer.color = Color.green;
                }
                isPlayerInDoor = true;
                isWalkerInDoor = false;
                isAllowedToEnter = true;
            }
            else
            {
                if (myRenderer)
                {
                    myRenderer.enabled = true;
                    myRenderer.color = Color.red;
                }
                isPlayerInDoor = true;
                isWalkerInDoor = false;
                isAllowedToEnter = false;

            }
        }
        else if (collision.tag == "Walker")
        {
            if (canEnter == CanEnter.WalkerOnly || canEnter == CanEnter.Both)
            {
                if (myRenderer)
                {
                    //myRenderer.enabled = true;
                    myRenderer.color = Color.green;
                }
                isPlayerInDoor = false;
                isWalkerInDoor = true;
                isAllowedToEnter = true;
            }
            else
            {
                if (myRenderer)
                {
                    //myRenderer.enabled = true;
                    myRenderer.color = Color.red;
                }
                isPlayerInDoor = false;
                isWalkerInDoor = true;
                isAllowedToEnter = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.tag == "Player") || (collision.tag == "Walker"))
        {
            if (myRenderer) { myRenderer.color = Color.white; }
            isPlayerInDoor = false;
            isWalkerInDoor = false;
            isAllowedToEnter = false;
        }
    }

}
