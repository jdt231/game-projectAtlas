using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    //Singleton instantiation
    private static WorldManager instance;
    public static WorldManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<WorldManager>();
            return instance;
        }
    }

    public static Dictionary<string, bool> keyItems = new Dictionary<string, bool>();

    public Hashtable gatesOpened = new Hashtable();
    public Hashtable keysCollected = new Hashtable();

    public List<string> coinCollected = new List<string>();
    public List<string> healthCollected = new List<string>();
    public List<string> ammoCollected = new List<string>();
    public List<string> inventoryItemCollected = new List<string>();

    private void Awake() // Destroy duplicates or Add unique objects to Dictionary
    {
        if (GameObject.Find("Primary World Manager"))
        { Destroy(gameObject); }
        else
        {
            AddWorldEvents();
            AddUniqueItemListings(); 
        }
    }

    void Start() // Dont destroy on load
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "Primary World Manager";
    }

    private void AddUniqueItemListings() // Dictionary
    {
        keyItems.Add("testKey1", false);
    }

    private void AddWorldEvents() // Hashtable 
    {
        //Gates Opened
        gatesOpened.Add("Level1_GateOpened", false);

        //Other
    }

    public bool CheckItemCollected(string name) // Dictionary
    {
        return keyItems[name];
    }

    public void MakeBoolTrue(string name) // Dictionary
    {
        keyItems[name] = true;
    }

    public void AddItemToList(string itemType, string itemName) // List
    {
        if (itemType == "Coin")
        { coinCollected.Add(itemName); }

        else if (itemType == "Health")
        { healthCollected.Add(itemName); }

        else if (itemType == "Ammo")
        { ammoCollected.Add(itemName); }

        else if (itemType == "InventoryItem")
        { inventoryItemCollected.Add(itemName); }

        else
        { Debug.LogWarning("Item type not recognised, please check Collectable"); }
    }

    public bool DoesListContainItem(string itemType, string itemName) //List
    {
        /*
        string targetList = (itemType + "Collected");
        Debug.Log("Checking list named: " + targetList + " for object named: " + itemName);
        if (targetList.Contains(itemName))
        { return true; }

        else
        { return false; }
        */
        bool doesContain = false;

        if (itemType == "Coin")
        { doesContain = coinCollected.Contains(itemName); }

        else if (itemType == "Health")
        { doesContain = healthCollected.Contains(itemName); }

        else if (itemType == "Ammo")
        { doesContain = ammoCollected.Contains(itemName); }

        else if (itemType == "InventoryItem")
        { doesContain = inventoryItemCollected.Contains(itemName); }

        else
        { Debug.LogWarning("Item type not recognised, please check Collectable"); }

        if (doesContain)
        { return true; }

        else
        { return false; }
    }

    public bool HasEventOccured(string eventType, string eventName) //Hashtable
    {
        bool doesContain = false;

        if (eventType == "Gate")
        { doesContain = (bool)gatesOpened[eventName]; }

        else if (eventType == "Other1")
        { }

        else if (eventType == "Other2")
        { }

        else if (eventType == "Other3")
        { }

        else
        { Debug.LogWarning("Item type not recognised, please check Collectable"); }

        if (doesContain)
        { return true; }

        else
        { return false; }
    }

    public void UpdateEventToTrue(string eventType, string eventName) // Hashtable
    {
        if (eventType == "Gate")
        { gatesOpened[eventName] = true; }
    }
}
