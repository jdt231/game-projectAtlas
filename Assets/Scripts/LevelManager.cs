using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Singleton instantiation
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<LevelManager>();
            return instance;
        }
    }

    [SerializeField] public PlayerController player; 
    [SerializeField] public WalkerController walker;

    [SerializeField] public string newSpawnName = "Start";

    private void Awake()
    {
        if (GameObject.Find("Primary Level Manager"))
        { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "Primary Level Manager";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(string loadSceneString, string spawnPosition)
    {
        //player.animator.SetBool("dead", false); TODO - Reinstate when finished with animator
        newSpawnName = spawnPosition;

        if(GameManager.Instance.isPlayerInWalker)
        {
            GameManager.Instance.walkerCurrentScene = loadSceneString;
        }

        GameManager.Instance.SavePlayerInformation();

        //InventoryManager.Instance.RemoveInventoryItem("none", true); TODO - Empties inventory. Remove later when certain is not required.
        GameManager.Instance.frozen = false;
        SceneManager.LoadScene(loadSceneString);

        UIManager.Instance.UpdateUI();
    }



    public void ReloadLevelAfterDeath(string loadSceneString) // TODO - This section will need re-witing entirely
    {
        //player.animator.SetBool("dead", false); TODO - Reinstate when finished with animator
        player.health = 100;
        InventoryManager.Instance.coinsCollected = 0;
        InventoryManager.Instance.RemoveInventoryItem("none", true);
        GameManager.Instance.frozen = false;
        SceneManager.LoadScene(loadSceneString);
        UIManager.Instance.UpdateUI();
    }

    public string GetCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        return currentScene;
    }
}
