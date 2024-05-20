using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator anim;
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerInfo playerInfo;

    public bool isInteracting;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        inputManager =GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerInfo = GetComponent<PlayerInfo>();
    }
    private void Update()
    {
        inputManager.HandleAllInput();
    }
    private void FixedUpdate()
    {
        playerInfo.AllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.AllCameraMovement();

        isInteracting = anim.GetBool("isInteracting");
        playerInfo.isJumping = anim.GetBool("isJumping");
        anim.SetBool("isGrounded",playerInfo.isGrounded);
    }
}
