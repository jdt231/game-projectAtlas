using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Singleton instantiation
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<UIManager>();
            return instance;
        }
    }

    [Header("UI References")]
    [SerializeField] private Text coinsText;
    [SerializeField] private Text batteriesText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image energyBar;
    [SerializeField] public Image inventoryItemImage;
    [SerializeField] public Animator uiAnimator;

    [Header("Other References")]
    [SerializeField] public PlayerController player;
    [SerializeField] public WalkerController walker;

    [Header("Attributes")]
    private Vector2 healthBarOrigSize;
    private Vector2 energyBarOrigSize;

    private void Awake()
    {
        if (GameObject.Find("Primary UI Manager"))
        { Destroy(gameObject); }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.name = "Primary UI Manager";

        healthBarOrigSize = healthBar.rectTransform.sizeDelta;
        energyBarOrigSize = energyBar.rectTransform.sizeDelta;

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        //if the healthbar's original size has not been set yet, match it to the healthbar rectTransform size.
        if (healthBarOrigSize == Vector2.zero) healthBarOrigSize = healthBar.rectTransform.sizeDelta;
        if (energyBarOrigSize == Vector2.zero) energyBarOrigSize = energyBar.rectTransform.sizeDelta;
        coinsText.text = ("COINS: " + InventoryManager.Instance.coinsCollected.ToString());
        batteriesText.text = ("BATTS: " + InventoryManager.Instance.batteriesCollected.ToString());

        //Set the health bar width to a percentage of its original value
        healthBar.rectTransform.sizeDelta = new Vector2(healthBarOrigSize.x * ((float)player.health / (float)GameManager.Instance.maxHealth), healthBar.rectTransform.sizeDelta.y);
        energyBar.rectTransform.sizeDelta = new Vector2(energyBarOrigSize.x * ((float)player.energy / (float)GameManager.Instance.maxEnergy), energyBar.rectTransform.sizeDelta.y);
    }
}
