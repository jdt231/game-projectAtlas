using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class NewPlayer : PhysicsObject
{
    //Singleton instantiation
    private static NewPlayer instance;
    public static NewPlayer Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<NewPlayer>();
            return instance;
        }
    }

    [Header("Attributes")]
    //[SerializeField] private float attackDuration = 0.5f; TODO - Now controlled by "PlayerMovement". Remove when satisfied.
    //public int attackPower = 25; TODO - Now controlled by "PlayerController". Remove when satisfied.
    //[SerializeField] private float jumpPower = 10f; TODO - Now controlled by "CharacterController2D". Remove when satisfied. 
    //[SerializeField] private float maxSpeed = 1f; TODO - Now controlled by "PlayerMovement". Remove when satisfied. 
    //[SerializeField] private AudioClip deathSound; TODO - Now controlled by "PlayerController". Remove when satisfied.
    //public bool frozen = false; // Moved to GameManager. Delete once sure not needed here.

    // Controls fall forgiveness. Unsure of whether to move to CharacterController2D, PlayerMovement or PlayerController.
    [SerializeField] private float fallForgiveness = 1; // This is the amount of seconds the player has after falling from a ledge to jump.
    [SerializeField] private float fallForgivenessCounter; // This is the simple counter that will begin the moment the player falls from a ledge.
    private float launch;
    [SerializeField] private float launchRecovery;
    [SerializeField] private Vector2 launchPower;

    //public int health = 100; TODO - Now controlled by "PlayerController". Remove when satisfied.
    //public int energy = 100; TODO - Now controlled by "PlayerController". Remove when satisfied.

    // Possibly move to a new "Inventory Manager"?
    //[Header("Inventory")]
    //public int ammo; TODO - Now controlled by "InventoryManager". Remove when satisfied.
    //public int coinsCollected; TODO - Now controlled by "InventoryManager". Remove when satisfied.
    //public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>(); TODO - Now controlled by "InventoryController". Remove when satisfied.
    //[Tooltip("The default inventory item slot")] TODO - Now controlled by "InventoryController". Remove when satisfied.
    //public Sprite inventoryItemBlank;TODO - Now controlled by "InventoryController". Remove when satisfied.
    //[Tooltip("The default (white) key item")]TODO - Now controlled by "InventoryController". Remove when satisfied.
    //public Sprite keySprite;TODO - Now controlled by "InventoryController". Remove when satisfied.
    //[Tooltip("The gem (coloured) key item")]TODO - Now controlled by "InventoryController". Remove when satisfied.
    //public Sprite keyGemSprite;TODO - Now controlled by "InventoryController". Remove when satisfied.

    //private int maxHealth = 100; TODO - Now controlled by "GameManager". Remove when satisfied.
    //private int maxEnergy = 100; TODO - Now controlled by "GameManager". Remove when satisfied.

    //[Header("References")]
    //[SerializeField] private Animator animator; TODO - Now controlled by "PlayerController". Remove when satisfied.
    //[SerializeField] private AnimatorFunctions animatorFunctions; TODO - Now controlled by "PlayerController". Remove when satisfied.
    //[SerializeField] private GameObject attackBox; TODO - Now controlled by "PlayerController". Remove when satisfied.
    //[SerializeField] public CameraEffects cameraEffects; TODO - Now controlled by "PlayerController". Remove when satisfied.

    //private Vector2 healthBarOrigSize; TODO - Now controlled by "UIManager". Remove when satisfied.
    //private Vector2 energyBarOrigSize; TODO - Now controlled by "UIManager". Remove when satisfied.

    //public AudioSource sfxAudioSource; TODO - Now controlled by "PlayerController". Remove when satisfied.
    //public AudioSource musicAudioSource; TODO - Now controlled by "PlayerController". Remove when satisfied.
    //public AudioSource ambienceAudioSource; TODO - Now controlled by "PlayerController". Remove when satisfied.

    //public CompanionController companion; TODO - Now controlled by "PlayerController". Remove when satisfied.
    //public GameObject powerLoader; TODO - IMPORTANT - RENAMED TO "walker". Now controlled by "PlayerController". Remove when satisfied.

    /* No longer required with new player controller - No longer persistant
    private void Awake()
    {
        if (GameObject.Find("New Player"))
        { Destroy(gameObject); }
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        /* No longer required with new player controller - No longer persistant
        DontDestroyOnLoad(gameObject);
        gameObject.name = "New Player";
        */

        // SetSpawnPosition(); TODO - Now controlled by "PlayerController". Remove when satisfied.

        //healthBarOrigSize = GameManager.Instance.healthBar.rectTransform.sizeDelta; TODO - Now controlled by "UIManager". Remove when satisfied.
        //energyBarOrigSize = GameManager.Instance.energyBar.rectTransform.sizeDelta; TODO - Now controlled by "UIManager". Remove when satisfied.

        // UpdateUI(); TODO - Now controlled by "UIManager". Remove when satisfied.
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.frozen == false & GameManager.Instance.isPlayerInWalker == false)
        {
            //ControlVelocity(); TODO - IMPORTANT - Migration not yet complete
            //ControlLocalScale(); TODO - Now controlled by "CharacterController2D". Remove when satisfied.
            //ControlAttacking(); TODO - Now controlled by "PlayerController". Remove when satisfied.
            //ControlDeath(); TODO - Now controlled by "PlayerController". Remove when satisfied.
        }
        else
        {
            //targetVelocity = new Vector2(0, 0); - Attempt to stop forward momentum when "deploying" causing the walker to turn around before moving. 
            //SetPositionToPowerloader(); TODO - Now controlled by "PlayerController". Remove when satisfied.
        }

        //Set each animator float, bool & trigger so it knows which animation to fire
        //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        //animator.SetFloat("velocityY", velocity.y);
        //animator.SetBool("grounded", grounded); // IMPORTANT - Currently unsure of how to integrate with "CharacterController2D"
        //animator.SetFloat("attackDirectionY", Input.GetAxis("Vertical"));
    }

    /*  TODO - Now controlled by "PlayerController". Remove when satisfied.
    private void SetPositionToPowerloader()
    {
        if (GameManager.Instance.isPlayerInPowerloader)
        {
            this.transform.position = new Vector3(powerLoader.transform.position.x, powerLoader.transform.position.y, powerLoader.transform.position.z);
        }
    }
    */

    /* TODO - Now controlled by "PlayerController". Remove when satisfied.
    public void SetSpawnPosition()
    {
        transform.position = GameObject.Find("Spawn Location").transform.position;
    }
    */

    /* TODO - Now controlled by "UIManager". Remove when satisfied.
    public void UpdateUI()
    {
        //if the healthbar's original size has not been set yet, match it to the healthbar rectTransform size.
        if (healthBarOrigSize == Vector2.zero) healthBarOrigSize = GameManager.Instance.healthBar.rectTransform.sizeDelta;
        if (energyBarOrigSize == Vector2.zero) energyBarOrigSize = GameManager.Instance.energyBar.rectTransform.sizeDelta;
        GameManager.Instance.coinsText.text = ("COINS: " + coinsCollected.ToString());
        GameManager.Instance.batteriesText.text = ("BATTS: " + companion.inventoryBatteries.ToString());

        //Set the health bar width to a percentage of its original value
        GameManager.Instance.healthBar.rectTransform.sizeDelta = new Vector2(healthBarOrigSize.x * ((float)health / (float)maxHealth), GameManager.Instance.healthBar.rectTransform.sizeDelta.y);
        GameManager.Instance.energyBar.rectTransform.sizeDelta = new Vector2(energyBarOrigSize.x * ((float)energy / (float)maxEnergy), GameManager.Instance.energyBar.rectTransform.sizeDelta.y);
    }
    */

    /* TODO - IMPORTANT - NOT FULLY INTEGRATED INTO NEW FUNCTIONALITY. REQUIRES FURTHER INVESTIGATION. 
    private void ControlVelocity()
    {
        //Lerp (ease) the launch value back to zero at all times.
        launch += (0 - launch) * Time.deltaTime * launchRecovery;

        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed + launch, 0);

        //if the player is no longer grounded, begin counting the fallForgivenessCounter
        if (!grounded)
        { fallForgivenessCounter += Time.deltaTime; }
        else
        { fallForgivenessCounter = 0; }

        //if the player presses "Jump" and we are grounded, set the velocity to a jump power value.
        if (Input.GetButtonDown("Jump") && fallForgivenessCounter < fallForgiveness)
        {
            Debug.Log("Player is trying to jump");
            animator.SetTrigger("jump"); // New jump functionality attempt

            velocity.y = jumpPower;
            grounded = false;
            fallForgivenessCounter = fallForgiveness;
        }
    }
    */

    /* TODO - Now controlled by "CharacterController2D". Remove when satisfied.
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

    /* TODO - Now controlled by "PlayerController". Remove when satisfied.
    private void ControlAttacking()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("attack");
        }
    }
    */

    /* TODO - Now controlled by "PlayerController". Remove when satisfied.
    private void ControlDeath()
    {
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }
    */

    /* TODO - Now controlled by "PlayerController". Remove when satisfied.
    public IEnumerator Die()
    {
        GameManager.Instance.frozen = true;
        sfxAudioSource.PlayOneShot(deathSound);
        animator.SetBool("dead", true);
        animatorFunctions.EmitParticlesDeath();
        yield return new WaitForSeconds(2);
        LoadLevel("Level1");
    }
    */

    /* TODO - Now controlled by "PlayerController". Remove when satisfied.
    public IEnumerator FreezeEffect(float length, float timeScale)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSeconds(length);
        Time.timeScale = 1;
    }
    */

    /* TODO - Now controlled by "LevelManager". Remove when satisfied.
    public void LoadLevel(string loadSceneString)
    {
        animator.SetBool("dead", false);
        health = 100;
        coinsCollected = 0;
        RemoveInventoryItem("none", true);
        GameManager.Instance.frozen = false;
        SceneManager.LoadScene(loadSceneString);
        SetSpawnPosition();
        UpdateUI();
    }
    */

    /* TODO - Now controlled by "InventoryManager". Remove when satisfied.
    //Function triggered from Collectables
    public void AddInventoryItem(string inventoryName, Sprite image)
    {
        inventory.Add(inventoryName, image);
        //the blank sprite should now swap with the key sprite
        GameManager.Instance.inventoryItemImage.sprite = inventory[inventoryName];
    }
    */

    /* TODO - Now controlled by "InventoryManager". Remove when satisfied.
    //Function triggered from Collectables
    public void RemoveInventoryItem(string inventoryName, bool removeAll = false)
    {
        if (!removeAll)
        {
            inventory.Remove(inventoryName);
        }
        else
        {
            inventory.Clear();
        }
        GameManager.Instance.inventoryItemImage.sprite = inventoryItemBlank;
    }
    */

    /* TODO - Now controlled by "PlayerController". Remove when satisfied.
    public void HurtPlayer(int attackPower, int targetSide)
    {
        StartCoroutine(FreezeEffect(0.1f, 0.5f));
        animator.SetTrigger("hurt");
        launch = -targetSide * launchPower.x;
        velocity.y = launchPower.y;
        cameraEffects.Shake(5, 0.5f); // Will need fixing in NEW BUILD
        health -= attackPower;
        UpdateUI();
    }
    */

    /* TODO - Now controlled by "PlayerController". Remove when satisfied.
    public void IncreaseHealth(int healthIncrease)
    {
        health += healthIncrease;
        if (health > maxHealth)
        { health = maxHealth; }
        UpdateUI();
    }
    */

    /* TODO - Now controlled by "PlayerController". Remove when satisfied.
    public void IncreaseEnergy(int energyIncrease)
    {
        energy += energyIncrease;
        if (energy > maxEnergy)
        { energy = maxEnergy; }
        UpdateUI();
    }
    */
}
