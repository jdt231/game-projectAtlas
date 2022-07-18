using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : MonoBehaviour
{
    public enum Task {Follow, Wait, Repair, CollectBattery, RefillEnergy};
    public Task currentTask;
    //[SerializeField] public GameObject[] edePositions; // TODO - obsolete due to powerloader reference. Remove when certain no longer needed.

    [Header("References")]
    [SerializeField] public PlayerController player;
    [SerializeField] public WalkerController walker;

    public bool isRepairing = false;
    public bool isRefillingEnergy = false;
    public bool isCollectingBattery = false;

    [Header("Attributes")]
    [SerializeField] float followSpeed = 10;
    [SerializeField] float taskSpeed = 10;
    [SerializeField] float repairTime = 3;
    [SerializeField] float energyRefillTime = 3;
    [SerializeField] float batteryCollectionTime = 3;
    [SerializeField] int repairValue = 20;
    [SerializeField] int energyRefillValue = 20;

    [Header("Audio SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip taskConfirmedSound;
    [SerializeField] float taskConfirmedSoundVolume = 10;
    [SerializeField] AudioClip taskErrorSound;
    [SerializeField] float taskErrorSoundVolume = 10;
    [SerializeField] AudioClip repairSound;
    [SerializeField] float repairSoundVolume = 10;
    [SerializeField] AudioClip harvestBatterySound;
    [SerializeField] float harvestBatterySoundVolume = 10;
    [SerializeField] AudioClip energyRefillSound;
    [SerializeField] float energyRefillSoundVolume = 10;

    [Header("Animations")]
    [SerializeField] Animator animator;

    [Header("Batteries")]
    //public int inventoryBatteries = 0; TODO - Now controlled by "PlayerController". Remove when satisfied.
    public List<GameObject> batteries = new List<GameObject>(); // TODO - change to private when no longer needing to test.
    public GameObject closestBattery = null; // TODO - change to private when no longer needing to test.
    public float batteryShortestDistance = 50; // TODO - change to private when no longer needing to test.
    private Transform collectionPoint;


    // Start is called before the first frame update
    void Start()
    {
        string newScene = LevelManager.Instance.GetCurrentScene();

        if (newScene == GameManager.Instance.walkerCurrentScene)
        {
            this.gameObject.SetActive(true);
            currentTask = Task.Follow;
            animator.SetBool("isIdle", true);
        }
        else { this.gameObject.SetActive(false); }
    }

    // Update is called once per frame
    void Update()
    {
        ControlLocalScale();
        ControlCurrentMovement();
        SelectCurrentTask();
    }

    private void ControlLocalScale()
    {
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    private void ControlCurrentMovement()
    {
        if (currentTask == Task.Follow)
        //{ transform.position = Vector2.MoveTowards(transform.position, edePositions[0].transform.position, followSpeed * Time.deltaTime); } TODO - obsolete due to powerloader reference. Remove when certain no longer needed.
        { transform.position = Vector2.MoveTowards(transform.position, walker.followPosition.transform.position, followSpeed * Time.deltaTime); }

        else if (currentTask == Task.Wait)
        { transform.position = transform.position; }

        else if (currentTask == Task.CollectBattery)
        {
            if (!isCollectingBattery)
            { transform.position = Vector2.MoveTowards(transform.position, collectionPoint.position, taskSpeed * Time.deltaTime); }
            else
            { transform.position = collectionPoint.position; }
        }
        else if (currentTask == Task.Repair)
        {
            if (!isRepairing)
            //{ transform.position = Vector2.MoveTowards(transform.position, edePositions[1].transform.position, taskSpeed * Time.deltaTime); } TODO - obsolete due to powerloader reference. Remove when certain no longer needed.
            { transform.position = Vector2.MoveTowards(transform.position, walker.repairPosition.transform.position, taskSpeed * Time.deltaTime); }
            else
            //{ transform.position = edePositions[1].transform.position; } TODO - obsolete due to powerloader reference. Remove when certain no longer needed.
            { transform.position = walker.repairPosition.transform.position; }
        }
        else if (currentTask == Task.RefillEnergy)
        {
            if (!isRefillingEnergy)
            //{ transform.position = Vector2.MoveTowards(transform.position, edePositions[1].transform.position, taskSpeed * Time.deltaTime); } TODO - obsolete due to powerloader reference. Remove when certain no longer needed.
            { transform.position = Vector2.MoveTowards(transform.position, walker.repairPosition.transform.position, taskSpeed * Time.deltaTime); }
            else
            //{ transform.position = edePositions[1].transform.position; } TODO - obsolete due to powerloader reference. Remove when certain no longer needed.
            { transform.position = walker.repairPosition.transform.position; }
        }
    }

    private void SelectCurrentTask()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            audioSource.PlayOneShot(taskConfirmedSound, taskConfirmedSoundVolume);
            animator.SetBool("isIdle", true);
            currentTask = Task.Follow;
            //Debug.Log("EDE is now following");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            audioSource.PlayOneShot(taskConfirmedSound, taskConfirmedSoundVolume);
            animator.SetBool("isIdle", true);
            currentTask = Task.Wait;
            //Debug.Log("EDE is now waiting");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            AttemptRepairs();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            AttemptBatteryCollection();
            //Debug.Log("EDE is now looking for battery");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            AttemptEnergyRefuel();
        }
    }

    private void AttemptEnergyRefuel()
    {
        if (GameManager.Instance.isPlayerInWalker == true && InventoryManager.Instance.batteriesCollected > 0)
        {
            animator.SetBool("isIdle", false);
            audioSource.PlayOneShot(taskConfirmedSound, taskConfirmedSoundVolume);
            currentTask = Task.RefillEnergy;
            Debug.Log("EDE is now moving to refill energy");
        }
        else if (GameManager.Instance.isPlayerInWalker == true && InventoryManager.Instance.batteriesCollected <= 0)
        {
            audioSource.PlayOneShot(taskErrorSound, taskErrorSoundVolume);
            Debug.Log("I cannot refill energy without a battery");
        }
        else
        {
            audioSource.PlayOneShot(taskErrorSound, taskErrorSoundVolume);
            Debug.Log("I cannot refill energy when you are not deployed");
        }
    }

    private void AttemptRepairs()
    {
        if (GameManager.Instance.isPlayerInWalker == true && InventoryManager.Instance.batteriesCollected > 0)
        {
            animator.SetBool("isIdle", false);
            audioSource.PlayOneShot(taskConfirmedSound, taskConfirmedSoundVolume);
            currentTask = Task.Repair;
            //Debug.Log("EDE is now moving to begin repairing");
        }
        else if (GameManager.Instance.isPlayerInWalker == true && InventoryManager.Instance.batteriesCollected <= 0)
        {
            audioSource.PlayOneShot(taskErrorSound, taskErrorSoundVolume);
            Debug.Log("I cannot make repairs without a battery");
        }

        else
        {
            audioSource.PlayOneShot(taskErrorSound, taskErrorSoundVolume);
            Debug.Log("I cannot make repairs when you are not deployed");
        }
    }

    private IEnumerator CommenceRepairs()
    {
        //Debug.Log("EDE is now commencing repairs");
        audioSource.PlayOneShot(repairSound, repairSoundVolume);
        yield return new WaitForSeconds(repairTime);
        InventoryManager.Instance.batteriesCollected--;
        player.IncreaseHealth(repairValue);
        audioSource.Stop();
        isRepairing = false;
        currentTask = Task.Follow;
        animator.SetBool("isIdle", true);
        walker.repairPosition.GetComponent<BoxCollider2D>().enabled = true;
        //Debug.Log("EDE has completed repairs and will return to following");
    }

    private void AttemptBatteryCollection()
    {
        closestBattery = null;
        batteryShortestDistance = 50;

        FindClosestBattery();

        if (closestBattery)
        {
            //Debug.Log("The closest battery is " + closestBattery.gameObject.name);
            animator.SetBool("isIdle", false);
            audioSource.PlayOneShot(taskConfirmedSound, taskConfirmedSoundVolume);
            collectionPoint = closestBattery.GetComponent<BatteryController>().collectionPoint;
            currentTask = Task.CollectBattery;
            //Debug.Log("EDE is now collecting a battery");
        }
        else
        {
            audioSource.PlayOneShot(taskErrorSound, taskErrorSoundVolume);
            Debug.Log("There are no batteries nearby");
        }
    }

    private void FindClosestBattery()
    {
        foreach (GameObject battery in GameObject.FindGameObjectsWithTag("Batteries"))
        { batteries.Add(battery); }

        foreach (GameObject battery in batteries)
        {
            float distance = Vector3.Distance(transform.position, battery.transform.position);

            if (distance < batteryShortestDistance)
            {
                closestBattery = battery;
                batteryShortestDistance = distance;
            }
        }
        batteries.Clear();
    }

    private IEnumerator HarvestBattery()
    {
        //Debug.Log("I will now harvest this battery");
        isCollectingBattery = true;
        audioSource.PlayOneShot(harvestBatterySound, harvestBatterySoundVolume);
        yield return new WaitForSeconds(batteryCollectionTime);
        InventoryManager.Instance.batteriesCollected++;
        UIManager.Instance.UpdateUI();
        //Debug.Log("I now have " + inventoryBatteries + " in my inventory");
        Destroy(closestBattery.gameObject);
        isCollectingBattery = false;
        currentTask = Task.Follow;
        animator.SetBool("isIdle", true);
        //Debug.Log("Battery harvested, returning to follow");
    }

    private IEnumerator CommenceRefilling()
    {
        //Debug.Log("EDE is now commencing refilling");
        audioSource.PlayOneShot(energyRefillSound, energyRefillSoundVolume);
        yield return new WaitForSeconds(energyRefillTime);
        InventoryManager.Instance.batteriesCollected--;
        player.IncreaseEnergy(energyRefillValue);
        audioSource.Stop();
        isRefillingEnergy = false;
        currentTask = Task.Follow;
        animator.SetBool("isIdle", true);
        walker.repairPosition.GetComponent<BoxCollider2D>().enabled = true;
        //Debug.Log("EDE has completed refilling and will return to following");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("I have collided with: " + collision.gameObject.name);
        if (collision.gameObject.name == "Repair Position" && currentTask == Task.Repair)
        {
            //Debug.Log("I have collided with: " + collision.gameObject.name + " and will now commence repairs");
            isRepairing = true;
            walker.repairPosition.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(CommenceRepairs());
        }

        else if (collision.gameObject.name == "Repair Position" && currentTask == Task.Follow)
        {
            //Debug.Log("I have collided with: " + collision.gameObject.name + " but I am not ready to repair");
        }

        else if (collision.gameObject.name == "Collection Point" && currentTask == Task.CollectBattery)
        {
            //Debug.Log("I have collided with: " + collision.gameObject.name + " and will now collect this battery");
            isCollectingBattery = true;
            StartCoroutine(HarvestBattery());
        }
        else if (collision.gameObject.name == "Repair Position" && currentTask == Task.RefillEnergy)
        {
            Debug.Log("I have collided with: " + collision.gameObject.name + " and will now refill energy");
            isRefillingEnergy = true;
            walker.repairPosition.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(CommenceRefilling());
        }
    }
}
