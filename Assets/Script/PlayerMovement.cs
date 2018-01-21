using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    bool isInCollision = false;


    


    [SerializeField] float walkMoveStopRadius = 0.20f;
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {

        ProcessMouseMovement();

    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {

            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;
                    isInCollision = false;
                    break;
                case Layer.Enemy:
                    //currentClickTarget = cameraRaycaster.hit.point;
                    break;
                default:
                    print("should not be here");
                    return;

            }
        }

        var playerToClickPoint = currentClickTarget - transform.position;

        if (playerToClickPoint.magnitude >= walkMoveStopRadius && !isInCollision)
        {
            if (Input.GetKey(KeyCode.LeftShift)) playerToClickPoint *= 0.1f;
            m_Character.Move(playerToClickPoint, false, false);
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isInCollision = true;        
    }

    private void OnCollisionExit(Collision collision)
    {
        isInCollision = false;
    }

}

