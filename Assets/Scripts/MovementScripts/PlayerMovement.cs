using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 mousePosition;
    public float speed = 3.0f;
    public float additionalSpeed = 0.0f;
    float speedPenalty = 0f;
    public GameObject knockbackObject;

    public Vector3 lastMoveDir;

    public void SetSpeed(float movementSpeed) {
        speed = movementSpeed;
    }

    public void SetSpeedPenalty(float newPenalty){
        speedPenalty = newPenalty;
    }

    public float GetSpeed(){
        return speed + additionalSpeed - speedPenalty;
    }

    void Update(){
        if(PauseManager.Instance.CurrPauseState == PauseManager.PauseState.NONE) {
            float step = GetSpeed() * Time.deltaTime;
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 movementVector = new Vector3(horizontal * step, vertical * step, 0);
            if(movementVector.magnitude > 0){
                lastMoveDir = movementVector.normalized;
                transform.position += movementVector;
            }
        }
    }
}
