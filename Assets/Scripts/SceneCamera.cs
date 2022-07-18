using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SceneCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        //cinemachineVirtualCamera.Follow = FindObjectOfType<CharacterController2D>().transform; *** IMPORTANT ***
        //Originally how this was done. Revert if new method causes issues. Changed due to camera lag when entering scene as walker

        if (GameManager.Instance.isPlayerInWalker == false)
        { FollowPlayer(); }
        else
        { FollowWalker(); }
    }

    public void FollowPlayer() // Also called from GameManager when exiting the Walker
    { cinemachineVirtualCamera.Follow = GameObject.FindGameObjectWithTag("Player").transform; }

    public void FollowWalker() // Also called from GameManager when entering the Walker
    { cinemachineVirtualCamera.Follow = GameObject.FindGameObjectWithTag("Walker").transform; }

}
