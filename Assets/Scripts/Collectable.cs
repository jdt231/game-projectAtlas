using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    enum ItemType { Coin, Health, Ammo, InventoryItem} //Creats an ItemType enum (drop down box to select specific option). Currently not using "Ammo"
    // If adding additional ItemTypes above, don't forget to add it where appropriate in the World Manager so the Collectable can be tracked.

    [Header("Attributes")]
    [SerializeField] private ItemType itemType;
    [SerializeField] private int healthValue = 5;

    [Header("Audio")]
    [SerializeField] private AudioClip soundCollection;
    [SerializeField] private float soundCollectionVolume = 1;

    [Header("References")]
    //[SerializeField] private string inventoryStringName; - TODO - Obsolete, now just using gameobject.name. Delete when certain no longer needed
    //[SerializeField] private string uniqueCollectableName; - TODO - Obsolete, now just using gameobject.name. Delete when certain no longer needed
    [SerializeField] private Sprite inventorySprite;
    [SerializeField] private ParticleSystem particleCollectableSpark;

    private bool touched = false;
    private bool isCollected = true;

    [SerializeField] private bool isUnique = false;

    private void Start()
    {
        CheckIfItemShouldBeDestroyed();
    }

    /*
    private void CheckIfItemShouldBeDestroyed() // Dictionary variant
    {
        if (itemType == ItemType.InventoryItem)
        {
            isCollected = WorldManager.Instance.CheckItemCollected(this.gameObject.name);
            if (isCollected == true)
            {
                Destroy(gameObject);
            }
        }
    }
    */

    private void CheckIfItemShouldBeDestroyed() // List Variant
    {
        if (isUnique)
        {
            isCollected = WorldManager.Instance.DoesListContainItem(itemType.ToString(), this.gameObject.name);
            if (isCollected)
            { Destroy(gameObject); }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If I have collided with the player, check what item-type I am and then carry out that item-type's function.
        //Then update the UI and destroy me. 
        if (!touched)
        {
            if (collision.name == "Player")
            {
                touched = true;
                ProcessCollectable();
                UIManager.Instance.UpdateUI();
                Destroy(gameObject);
            }

            else if ((collision.name == "Walker") && GameManager.Instance.isPlayerInWalker == true)
            {
                touched = true;
                ProcessCollectable();
                UIManager.Instance.UpdateUI();
                Destroy(gameObject);
            }

            else
            { Debug.Log("I have been touched by " + collision.name); }
        }
    }

    private void ProcessCollectable()
    {
        PlayerController player = FindObjectOfType<PlayerController>();

        if (particleCollectableSpark)
        {
            particleCollectableSpark.transform.parent = null;
            particleCollectableSpark.gameObject.SetActive(true);
            Destroy(particleCollectableSpark.gameObject, particleCollectableSpark.main.duration);
        }

        if (itemType == ItemType.Coin)
        {
            InventoryManager.Instance.coinsCollected += 1;
            player.sfxAudioSource.PlayOneShot(soundCollection, soundCollectionVolume);
        }

        else if (itemType == ItemType.Health)
        {
            player.IncreaseHealth(healthValue);
            player.sfxAudioSource.PlayOneShot(soundCollection, soundCollectionVolume);
        }

        else if (itemType == ItemType.Ammo)
        { }

        else if (itemType == ItemType.InventoryItem)
        {
            InventoryManager.Instance.AddInventoryItem(this.gameObject.name, inventorySprite);
            player.sfxAudioSource.PlayOneShot(soundCollection, soundCollectionVolume);
        }

        else { Debug.LogWarning("Item type not recognised, please check collectable"); }

        if (isUnique)
        { RemoveFromGame(); }
    }

    /*
    private void RemoveFromGame() // Dictionary variant
    {
        isCollected = WorldManager.Instance.CheckItemCollected(this.gameObject.name);

        if (isCollected == false)
        { WorldManager.Instance.MakeBoolTrue(this.gameObject.name); }

        else { Debug.LogWarning(this.gameObject.name + " should not have appered as it was marked as *Collected*!"); }
    }
    */

    private void RemoveFromGame() // List Variant
    {
        WorldManager.Instance.AddItemToList(itemType.ToString(), this.gameObject.name);
    }
}
