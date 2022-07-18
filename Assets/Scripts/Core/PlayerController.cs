using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    /*

    //Singleton instantiation - Removed due to Camera problems and other potential issues
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<PlayerController>();
            return instance;
        }
    }

    */

    [Header("Movement")]
    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    [Header("References")]
    [SerializeField] public CompanionController companion;
    [SerializeField] public WalkerController walker;
    [SerializeField] public CameraEffects cameraEffects;
    [SerializeField] public Animator animator;
    [SerializeField] public AnimatorFunctions animatorFunctions;
    [SerializeField] public GameObject attackBox;

    [Header("Attributes")]
    public int health = 100;
    public int energy = 100;
    [SerializeField] private float attackDuration = 0.5f; //how long is the attack box active when attacking
    public int attackPower = 25;
    public int deathDelay = 2;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSound;
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource ambienceAudioSource;

    /*

    private void Awake()
    {
        if (GameObject.Find("Primary Level Manager"))
        { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "Primary Player Controller";
    }

    */

    /* 
     void Awake() // Don't destroy on load set up
     {
         GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

         if (player.Length > 1)
         {
             Destroy(this.gameObject);
         }

         DontDestroyOnLoad(this.gameObject);
     }
     */

    private void Awake()
    {
        EstablishReferences();
        GameManager.Instance.LoadPlayerInformation();
    }

    private void Start()
    {
        //EstablishReferences();
        if (GameManager.Instance.isPlayerInWalker)
        {
            GameManager.Instance.PlayerEnterWalker();
        }
        else
        {
            SetSpawnPosition();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.frozen == false & GameManager.Instance.isPlayerInWalker == false)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            ControlKeyInput();
            ControlAttacking();
            ControlDeath();
        }

        else if (GameManager.Instance.frozen == false & GameManager.Instance.isPlayerInWalker == true)
        {
            this.transform.position = new Vector3(walker.transform.position.x, walker.transform.position.y, walker.transform.position.z); // Set position to walker when deployed.
        }
    }

    private void FixedUpdate() //Controlling movement
    {
        if (GameManager.Instance.frozen == false & GameManager.Instance.isPlayerInWalker == false)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
            jump = false;
        }
    }

    public void EstablishReferences()
    {
        GameManager.Instance.player = this;
        LevelManager.Instance.player = this;
        UIManager.Instance.player = this;
    }

    public void SetSpawnPosition()
    {
        string spawnName = (LevelManager.Instance.newSpawnName);
        GameObject spawnLocation = GameObject.Find(spawnName);
        transform.position = spawnLocation.transform.position;

        if (spawnLocation.GetComponent<DoorController>().facePlayerRight == true)
        {
            if (controller.m_FacingRight == false)
            { controller.Flip(); }
        }
        else
        {
            if (controller.m_FacingRight == true)
            { controller.Flip(); }
        }
    }

    private void ControlKeyInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            crouch = false;
        }

        if (Input.GetButtonDown("Crouch") && controller.m_Grounded == true)
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    private void ControlAttacking()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //animator.SetTrigger("attack"); TODO - Reimplement when animator is back in use.
        }
    }

    private void ControlDeath()
    {
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        GameManager.Instance.frozen = true;
        sfxAudioSource.PlayOneShot(deathSound);
        //animator.SetBool("dead", true); TODO - Reimplement when animator is back in use.
        //animatorFunctions.EmitParticlesDeath(); TODO - Reimplement when animator is back in use.
        yield return new WaitForSeconds(deathDelay);
        LevelManager.Instance.ReloadLevelAfterDeath(SceneManager.GetActiveScene().name); // Change "GetActiveScene" if implementing "bench-like" save system.
    }

    public IEnumerator FreezeEffect(float length, float timeScale)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSeconds(length);
        Time.timeScale = 1;
    }

    public void HurtPlayer(int attackPower, int targetSide)
    {
        StartCoroutine(FreezeEffect(0.1f, 0.5f));
        //animator.SetTrigger("hurt"); TODO - Reimplement when animator is back in use.
        //launch = -targetSide * launchPower.x; TODO - NEEDS SORTING OUT (STILL IN Newplayer.cs AT TIME OF WRITING)
        //velocity.y = launchPower.y; TODO - NEEDS SORTING OUT(STILL IN Newplayer.cs AT TIME OF WRITING)
        cameraEffects.Shake(5, 0.5f); // Will need fixing in NEW BUILD
        health -= attackPower;
        UIManager.Instance.UpdateUI();
    }

    public void IncreaseHealth(int healthIncrease)
    {
        health += healthIncrease;
        if (health > GameManager.Instance.maxHealth)
        { health = GameManager.Instance.maxHealth; }
        UIManager.Instance.UpdateUI();
    }

    public void IncreaseEnergy(int energyIncrease)
    {
        energy += energyIncrease;
        if (energy > GameManager.Instance.maxEnergy)
        { energy = GameManager.Instance.maxEnergy; }
        UIManager.Instance.UpdateUI();
    }
}
