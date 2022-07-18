using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Singleton instantiation
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    [Header("References")]
    public PlayerController player;
    public WalkerController walker;

    [Header("Player")]
    public int currentHealth = 0; // When transitioning between levels, the player tells the game manager what its current health is. The new level loads and the game manager resets the players health to this level
    public int currentEnergy = 0; // When transitioning between levels, the player tells the game manager what its current energy is. The new level loads and the game manager resets the players energy to this level
    public int maxHealth = 100;
    public int maxEnergy = 100;

    [Header("Walker")]
    public Vector2 walkerCurrentPosition;
    public string walkerCurrentScene = "Level 1";

    [SerializeField] public bool isPlayerInWalker = false;
    [SerializeField] public bool isPlayerNearWalker = false;
    [SerializeField] public bool isWalkerObstructed = false;
    [SerializeField] public bool walkerPositionStored = false;
    [SerializeField] public bool walkerFacingRight = true;

    public bool frozen = false;

    private void Awake() // Don't destroy on load.
    {
        if (GameObject.Find("Primary Game Manager"))
        { Destroy(gameObject); }
    } 

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "Primary Game Manager";
        SaveWalkerPosition();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessKeyPress();
    }

    private void ProcessKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            string currentScene = LevelManager.Instance.GetCurrentScene();
            if (currentScene == walkerCurrentScene)
            {
                if (isPlayerInWalker == false)
                {
                    AttemptEnterWalker();
                }
                else
                {
                    AttemptExitWalker();
                }
            }
            else { Debug.Log("The walker is not in this scene"); }
        }
    }

    private void AttemptEnterWalker()
    {
        if (player.controller.m_Grounded)
        {
            isWalkerObstructed = walker.deployPosition.GetComponent<DeployController>().CheckforObstruction();

            if (isWalkerObstructed)
            {
                Debug.Log("Walker deploy space obstructed, cannot deploy");
            }
            else
            {
                isPlayerNearWalker = walker.deployPosition.GetComponent<DeployController>().CheckforPlayer();

                if (isPlayerNearWalker)
                {
                    PlayerEnterWalker();
                    isPlayerNearWalker = false;
                }
                else
                { Debug.Log("Out of range of walker, cannot deploy"); }
            }
        }
        else
        { Debug.Log("I am not grounded, cannot deploy"); }
    }

    private void AttemptExitWalker()
    {
        if (walker.controller.m_Grounded)
        {
            //walker.deployPosition.gameObject.SetActive(true);
            isWalkerObstructed = walker.deployPosition.GetComponent<DeployController>().CheckforObstruction();
            //walker.deployPosition.gameObject.SetActive(false);

            if (isWalkerObstructed)
            {
                Debug.Log("Walker deploy space obstructed, cannot deploy");
            }
            else
            {
                isWalkerObstructed = false;
                PlayerExitWalker();
            }
        }
        else
        {
            Debug.Log("I am not grounded, cannot deploy");
        }
    }

    public void PlayerEnterWalker()
    {
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<BoxCollider2D>().enabled = false;
        player.GetComponent<CircleCollider2D>().enabled = false;

        FindObjectOfType<SceneCamera>().FollowWalker();
        walker.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        isPlayerInWalker = true;
        ClearWalkerPosition();
    }

    private void ClearWalkerPosition()
    {
        walkerPositionStored = false;
    }

    private void PlayerExitWalker()
    {
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<BoxCollider2D>().enabled = true;
        player.GetComponent<CircleCollider2D>().enabled = true;

        isPlayerInWalker = false;

        FindObjectOfType<SceneCamera>().FollowPlayer();
        walker.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        player.transform.position = walker.deployPosition.transform.position;

        isPlayerNearWalker = false;
        SaveWalkerPosition();
        SetPlayerFacingDirection();
    }

    private void SaveWalkerPosition()
    {
        if (isPlayerInWalker == false && (walkerPositionStored == false))
        {
            walkerCurrentPosition = walker.gameObject.transform.position;
            walkerFacingRight = walker.GetComponent<CharacterController2D>().m_FacingRight;
            walkerCurrentScene = LevelManager.Instance.GetCurrentScene();
            walkerPositionStored = true;
        }
    }

    private void SetPlayerFacingDirection()
    {
        if (walkerFacingRight)
        {
            if (player.GetComponent<CharacterController2D>().m_FacingRight != true)
            { player.GetComponent<CharacterController2D>().Flip(); }
        }
        else if (!walkerFacingRight)
        {
            if (player.GetComponent<CharacterController2D>().m_FacingRight == true)
            { player.GetComponent<CharacterController2D>().Flip(); }
        }
    }

    public void SavePlayerInformation() // Called from LevelManager
    {
        currentHealth = player.health;
        currentEnergy = player.energy;
    }

    public void LoadPlayerInformation() // Called from PlayerController
    {
        player.health = currentHealth;
        player.energy = currentEnergy;
    }
}
