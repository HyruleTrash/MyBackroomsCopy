using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    public float walkingSpeedNormal = 0.15f;
    public float MaxwalkingSpeedNormal = 5f;
    public float SlowDownHNormal = 0.1f;
    public float SlowDownVNormal = 0.05f;

    public float walkingSpeedAirBorn = 0.2f;
    public float MaxwalkingSpeedAirBorn = 10f;
    public float SlowDownHAirBorn = 0.12f;
    public float SlowDownVAirBorn = 0.04f;

    public float jumpSpeed = 8.0f;
    bool falling = false;
    bool jumped = false;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    float curSpeedX = 0;
    float curSpeedY = 0;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    public GameObject StatTracker;



    void Start()
    {
        StatTracker = GameObject.Find("StatTracker");
        StatTracker.GetComponent<StatTracker>().CollectedAlmondWater = StatTracker.GetComponent<StatTracker>().ToBeCollectedAlmondWater;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float walkingSpeed = (characterController.isGrounded ? walkingSpeedNormal : walkingSpeedAirBorn);
        float MaxwalkingSpeed = (characterController.isGrounded ? MaxwalkingSpeedNormal : MaxwalkingSpeedAirBorn);
        float SlowDownH = (characterController.isGrounded ? SlowDownHNormal : SlowDownHAirBorn);
        float SlowDownV = (characterController.isGrounded ? SlowDownVNormal : SlowDownVAirBorn);
        //WASD & arrow movement
        curSpeedX += canMove ? walkingSpeed * Input.GetAxis("Vertical") : 0;
        curSpeedY += canMove ? walkingSpeed * Input.GetAxis("Horizontal") : 0;
        //speed cap
        curSpeedX = curSpeedX > MaxwalkingSpeed ? MaxwalkingSpeed : (curSpeedX < -MaxwalkingSpeed ? -MaxwalkingSpeed : curSpeedX);
        curSpeedY = curSpeedY > MaxwalkingSpeed ? MaxwalkingSpeed : (curSpeedY < -MaxwalkingSpeed ? -MaxwalkingSpeed : curSpeedY);
        //when not moving slide
        curSpeedX -= Input.GetAxis("Vertical") == 0 ? curSpeedX * SlowDownV : 0;
        curSpeedY -= Input.GetAxis("Horizontal") == 0 ? curSpeedY * SlowDownH : 0;


        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
            // jump sound
            jumped = true;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (characterController.velocity.y < -5)
        {
            falling = true;
        }
        if (characterController.isGrounded == true)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                // walk sound
            }
            else
            {
                // walk sound stop
            }
        }
        else
        {
            // walk sound stop
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "AlmondWaterCollectable":
                StatTracker.GetComponent<StatTracker>().AddAlmondWaterCollect(2);
                GameObject.Destroy(other.gameObject);
                break;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<SceneTransitionThingy>() && hit.gameObject.GetComponent<SceneTransitionThingy>().isTransitionOrNot)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Use()
    {
        
    }
    public void Drop(float force, Vector3 startPositionOverride, float range)
    {
        
    }
}
