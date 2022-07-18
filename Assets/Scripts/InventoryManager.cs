using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //Singleton instantiation
    private static InventoryManager instance;
    public static InventoryManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<InventoryManager>();
            return instance;
        }
    }

    [Header("Player")]
    public int ammo;
    public int coinsCollected;

    [Header("Walker")]
    public int batteriesCollected = 0;

    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>(); //dictionary storing all inventory item strings

    [Header("Inventory")]
    [Tooltip("The default inventory item slot")]
    public Sprite inventoryItemBlank;
    [Tooltip("The default (white) key item")]
    public Sprite keySprite;
    [Tooltip("The gem (coloured) key item")]
    public Sprite keyGemSprite;

    private void Awake()
    {
        if (GameObject.Find("Primary Inventory Manager"))
        { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "Primary Inventory Manager";
    }

    //Function triggered from Collectables
    public void AddInventoryItem(string inventoryName, Sprite image)
    {
        inventory.Add(inventoryName, image);
        //the blank sprite should now swap with the key sprite
        UIManager.Instance.inventoryItemImage.sprite = inventory[inventoryName];
    }

    //Function triggered from Collectables
    public void RemoveInventoryItem(string inventoryName, bool removeAll = false)
    {
        if (!removeAll)
        { inventory.Remove(inventoryName);}
        else
        {
            inventory.Clear();
        }
        UIManager.Instance.inventoryItemImage.sprite = inventoryItemBlank;
    }
}
