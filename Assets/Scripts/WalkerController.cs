using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerController : MonoBehaviour
{
    /*

    //Singleton instantiation - Removed due to Camera problems and other potential issues
    private static WalkerController instance;
    public static WalkerController Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<WalkerController>();
            return instance;
        }
    }

    */

    /* Original - Before changes
    [Header("Attributes")]
    [SerializeField] private float attackDuration = 0.5f; //how long is the attack box active when attacking
    public int attackPower = 25;
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private AudioClip deathSound;

    [SerializeField] private float fallForgiveness = 1; // This is the amount of seconds the player has after falling from a ledge to jump.
    [SerializeField] private float fallForgivenessCounter; // This is the simple counter that will begin the moment the player falls from a ledge.
    private float launch;
    [SerializeField] private float launchRecovery;
    [SerializeField] private Vector2 launchPower;

    [Header("References")]
    [SerializeField] private NewPlayer newPlayer;
    [SerializeField] public GameObject followPosition;
    [SerializeField] public GameObject repairPosition;
    [SerializeField] public GameObject deployPosition;
    [SerializeField] private Animator animator;

    public bool isPlayerNear;
    */

    [Header("Movement")]
    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    [Header("References")]
    [SerializeField] public CompanionController companion;
    [SerializeField] public PlayerController player;
    [SerializeField] public CameraEffects cameraEffects;
    [SerializeField] public Animator animator;
    [SerializeField] public AnimatorFunctions animatorFunctions;
    [SerializeField] public GameObject attackBox;

    [SerializeField] public GameObject followPosition;
    [SerializeField] public GameObject repairPosition;
    [SerializeField] public GameObject deployPosition;

    [Header("Attributes")]
    //public int health = 100; TODO - Decide whether the player and the walker need seperate health/energy/stamina etc.
    //public int energy = 100; TODO - Decide whether the player and the walker need seperate health/energy/stamina etc.
    [SerializeField] private float attackDuration = 0.5f; //how long is the attack box active when attacking
    public int attackPower = 25;
    public int deathDelay = 2;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSound;
    //public AudioSource sfxAudioSource;    Unsure if walker requires seperate usage
    //public AudioSource musicAudioSource;  Unsure if walker requires seperate usage
    //public AudioSource ambienceAudioSource;   Unsure if walker requires seperate usage

    /*
    private void Awake()
    {
        if (GameObject.Find("Primary Walker Controller"))
        { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "Primary Walker Controller";
    }
    */

    /*
    void Awake() // Don't destroy on load set up
    {
        WalkerController[] walker = GameObject.FindObjectsOfType<WalkerController>(true);

        if (walker.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    */

    /*
    private void Awake() // Check if the walker is in the current scene. SetActive to true if yes, false if no. 
    {
        string newScene = LevelManager.Instance.GetCurrentScene();

        if (newScene == GameManager.Instance.walkerCurrentScene)
        {
            Debug.Log("The walker is in this scene, activating");
            this.gameObject.SetActive(true); 
        }
        else
        {
            Debug.Log("The walker is not in this scene, deactivating");
            this.gameObject.SetActive(false); 
        }
    }

    private void Start()
    {
        EstablishReferences();
        SetSpawnPosition();
    }
    */

    private void Awake()
    {
        EstablishReferences();
    }

    private void Start()
    {
        string newScene = LevelManager.Instance.GetCurrentScene();

        if (newScene == GameManager.Instance.walkerCurrentScene)
        {
            this.gameObject.SetActive(true);
            SetSpawnPosition();
            companion.gameObject.transform.position = this.followPosition.transform.position;
        }
        else
        { this.gameObject.SetActive(false); }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.frozen == false & GameManager.Instance.isPlayerInWalker == true)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            ControlKeyInput();
            //ControlAttacking(); TODO - Add later once update is finished and working
            //ControlDeath(); TODO - Add later once update is finished and working
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.frozen == false & GameManager.Instance.isPlayerInWalker == true)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
            jump = false;
        }
    }

    public void EstablishReferences()
    {
        GameManager.Instance.walker = this;
        LevelManager.Instance.walker = this;
        UIManager.Instance.walker = this;
    }

    public void SetSpawnPosition()
    {
        if (GameManager.Instance.isPlayerInWalker) // Currently not doing anything as there is no instance yet where the player is in the walker at the start of the scene
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
        else // TODO - This section probably needs re-writing as well as moving to the GameManager where the spawn position can be called from there to here.
        {
            string newScene = LevelManager.Instance.GetCurrentScene();

            if (newScene == GameManager.Instance.walkerCurrentScene)
            {
                //Debug.Log("We have entered the same scene as the walker");
                if (GameManager.Instance.walkerPositionStored == true)
                {
                    transform.position = GameManager.Instance.walkerCurrentPosition;
                    if (GameManager.Instance.walkerFacingRight == false)
                    {
                        GetComponent<CharacterController2D>().Flip();
                    }
                }
            }
            else
            {
                //Debug.Log("This is a different scene from the walker");
            }
        }
    }

    private void ControlKeyInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            //crouch = true; // Currently I do not want the walker to be able to crouch.
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            //crouch = false; // Currently I do not want the walker to be able to crouch.
        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.frozen == false & GameManager.Instance.isPlayerInWalker == true)
        {
            //ControlVelocity();
            //ControlLocalScale();
        }
        else
        {
            //targetVelocity = new Vector2(0, 0);

        }
        //Set each animator float, bool & trigger so it knows which animation to fire
        //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        //animator.SetFloat("velocityY", velocity.y);
        //animator.SetBool("grounded", grounded);
        //animator.SetFloat("attackDirectionY", Input.GetAxis("Vertical"));
    }
    */

    /*
    private void ControlVelocity()
    {
        //Lerp (ease) the launch value back to zero at all times.
        launch += (0 - launch) * Time.deltaTime * launchRecovery;

        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed + launch, 0);

        //if the player is no longer grounded, begin counting the fallForgivenessCounter
        if (!grounded)
        {
            fallForgivenessCounter += Time.deltaTime;
        }
        else
        {
            fallForgivenessCounter = 0;
        }
        

        //if the player presses "Jump" and we are grounded, set the velocity to a jump power value.
        if (Input.GetButtonDown("Jump") && fallForgivenessCounter < fallForgiveness)
        {
            Debug.Log("Powerloader is trying to jump");
            animator.SetTrigger("jump"); // New jump functionality attempt

            velocity.y = jumpPower;
            grounded = false;
            fallForgivenessCounter = fallForgiveness;
        }
    }
    */

    /*
    private void ControlLocalScale()
    {
        //flip the player's localscale.x if the move speed is greater than .01 of less than -01
        if (targetVelocity.x < -0.01)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (targetVelocity.x > 0.01)
        {
            transform.localScale = new Vector2(1, 1);
        }
    }
    */
}
