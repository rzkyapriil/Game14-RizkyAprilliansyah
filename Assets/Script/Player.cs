using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField, Range(0.01f, 1f)] float moveDuration = 0.2f;
    [SerializeField, Range(0.01f, 1f)] float jumpHeight = 0.8f;

    private void Update()
    {
        var moveDir = Vector3.zero;
        var rotateDir = Vector3.zero;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            moveDir += new Vector3(0, 0, 1);
            rotateDir = new Vector3(0f, 0f, 0f);
        }
        
        if(Input.GetKey(KeyCode.DownArrow))
        {
            moveDir += new Vector3(0, 0, -1);
            rotateDir = new Vector3(0f, 180f, 0f);
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            moveDir += new Vector3(1, 0, 0);
            rotateDir = new Vector3(0f, 90f, 0f);
        }
        
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir += new Vector3(-1, 0, 0);
            rotateDir = new Vector3(0f, -90f, 0f);
        }

        if(moveDir != Vector3.zero && isJumping() == false)
            Jump(moveDir, rotateDir);
    }

    private void Jump(Vector3 targetMoveDirection, Vector3 targetRotateDirection)
    {
        var TargetPosition = transform.position + targetMoveDirection;

        //transform.LookAt(targetDirection);

        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0.4f, moveDuration/2));

        transform.DOMoveX(TargetPosition.x, moveDuration);
        transform.DOMoveZ(TargetPosition.z, moveDuration);
        transform.DORotate(targetRotateDirection, moveDuration/2);
    }

    private bool isJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
        {
            AnimateDie();
        }
        
    }

    private void AnimateDie()
    {
        transform.DOScaleY(0.01f, 1);
        transform.DOScaleX(0.25f, 1);
        transform.DOScaleZ(0.2f, 1);
        this.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        // Debug.Log("Work");
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
