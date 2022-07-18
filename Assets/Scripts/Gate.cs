using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private string requiredInventoryItemString;
    [SerializeField] private Animator animator;


    //enum ItemType { Gate, Other1, Other2, Other3 }
    //[SerializeField] private ItemType itemType;

    [SerializeField] private bool isEventTracked = false;
    private bool isGateAlreadyOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        if (isEventTracked)
        {
            CheckIfGateIsAlreadyOpened();
        }
    }

    private void CheckIfGateIsAlreadyOpened()
    {
        string hashtableString = (this.gameObject.name + "Opened");
        isGateAlreadyOpen = WorldManager.Instance.HasEventOccured("Gate", hashtableString);
        if (isGateAlreadyOpen)
        { Destroy(gameObject); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Walker")
        {
            //If the player inventory has key1, remove item from inventory then destroy me.
            if (InventoryManager.Instance.inventory.ContainsKey(requiredInventoryItemString)) // TODO - Not sure what happened here. appears to be missing method within the inventory that potentially returns true/false as to whether the "requiredInventoryItemString" is in the inventory
            {
                InventoryManager.Instance.RemoveInventoryItem(requiredInventoryItemString);
                animator.SetBool("opened", true);

                if (isEventTracked)
                { SetEventToTrue(); }
            }
        }

        else
        { Debug.Log("I have collided with " + collision.name); }
    }

    private void SetEventToTrue()
    {
        string hashtableString = (this.gameObject.name + "Opened");
        WorldManager.Instance.UpdateEventToTrue("Gate", hashtableString);
    }
}
