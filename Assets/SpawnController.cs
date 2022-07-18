using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private PlayerController player;
    private WalkerController walker;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        //walker = GameObject.FindObjectOfType<WalkerController>(true);
        OrganisePlayer();
        //OrganiseWalker();
    }

    private void OrganisePlayer()
    {
        Debug.Log("Attempting to organise player");
        player.SetSpawnPosition();
        player.EstablishReferences();
        Debug.Log("I have finished attempting to organise the player");
    }

    private void OrganiseWalker()
    {
        string newScene = LevelManager.Instance.GetCurrentScene();
        if (newScene == GameManager.Instance.walkerCurrentScene)
        {
            walker.gameObject.SetActive(true);
            walker.SetSpawnPosition();
            walker.EstablishReferences();
        }
        else
        {
            walker.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
