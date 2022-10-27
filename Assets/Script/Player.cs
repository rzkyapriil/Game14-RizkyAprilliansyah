using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;
    [SerializeField] ParticleSystem dieParticles;
    [SerializeField, Range(0.01f, 1f)] float moveDuration = 0.2f;
    [SerializeField, Range(0.01f, 1f)] float jumpHeight = 0.8f;
    private int minZPos;
    private int extent;
    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;

    [SerializeField] private int maxTravel;
    [SerializeField] private int currentTravel;

    public int MaxTravel {get => maxTravel;}
    public int CurrentTravel {get => currentTravel;}
    public bool IsDie {get => this.enabled == false;}

    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos-1;
        leftBoundary = -(extent+1);
        rightBoundary = extent+1;
    }

    private void Update()
    {
        var moveDir = Vector3.zero;
        // var rotateDir = Vector3.zero;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            moveDir += new Vector3(0, 0, 1);
            // rotateDir = new Vector3(0f, 0f, 0f);
        }
        
        if(Input.GetKey(KeyCode.DownArrow))
        {
            moveDir += new Vector3(0, 0, -1);
            // rotateDir = new Vector3(0f, 180f, 0f);
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            moveDir += new Vector3(1, 0, 0);
            // rotateDir = new Vector3(0f, 90f, 0f);
        }
        
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir += new Vector3(-1, 0, 0);
            // rotateDir = new Vector3(0f, -90f, 0f);
        }

        if(moveDir != Vector3.zero && isJumping() == false)
            Jump(moveDir);
    }

    private void Jump(Vector3 targetDirection)
    {
        // Atur Posisi
        var targetPosition = transform.position + targetDirection;
        transform.LookAt(targetPosition);

        // Loncat keatas
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0, moveDuration/2));

        if(targetPosition.z <= backBoundary ||
            targetPosition.x <= leftBoundary ||
            targetPosition.x >= rightBoundary)
            return;

        if(Tree.AllPositions.Contains(targetPosition))
            return;
        
        // gerak maju/mundur/samping
        transform.DOMoveX(targetPosition.x, moveDuration);
        transform.DOMoveZ(targetPosition.z, moveDuration).OnComplete(UpdateTravel);
    }

    private void UpdateTravel()
    {
        currentTravel = (int) this.transform.position.z;

        if(currentTravel > maxTravel)
            maxTravel = currentTravel;
        
        stepText.text = "STEP:" + maxTravel.ToString();
    }

    private bool isJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.enabled==false)
            return;

        var car = other.GetComponent<Car>();

        if(car != null)
            AnimateCrash(car);

        // if(other.tag == "Car") {}
            // AnimateDie(car);
        
    }

    private void AnimateCrash(Car car)
    {
        transform.DOScaleY(0.1f, 1);
        transform.DOScaleX(2, 1);
        transform.DOScaleZ(2, 1);

        this.enabled = false;
        dieParticles.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        // Debug.Log("Work");
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
