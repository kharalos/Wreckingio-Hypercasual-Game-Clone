using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject driftPivotPoint;

    private Vector2 startTouch, currentMousePos, swipeDirection;

    private bool inAir;
    private bool landing;
    private bool carActive;
    private bool drifting;

    [SerializeField]
    private float maxClampX = 50, maxClampY = 100f, carSpeed = 6f, driftSpeed = 6f;

    private float horizontal, vertical;

    private bool holdingTouch;

    [SerializeField]
    private TrailRenderer[] tireTracks;
    [SerializeField]
    private ParticleSystem driftEffect;
    void Start()
    {
        inAir = false;
        landing = false;
        carActive = true;
        holdingTouch = false;
        horizontal = vertical = 0;
        swipeDirection = Vector2.zero;
        currentMousePos = Vector2.zero;
        startTouch = Vector2.zero;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            holdingTouch = true;
        }

        if (Input.GetMouseButton(0) && holdingTouch)
        {
            currentMousePos = Input.mousePosition;
            swipeDirection = currentMousePos - startTouch;
            horizontal = swipeDirection.x;
            vertical = swipeDirection.y;
        }
        else
        {
            horizontal = vertical = 0;
        }
        ClampSwipe();
        var emission = driftEffect.emission;
        if (horizontal != 0)
        {
            emission.rateOverTime = horizontal;
        }
        else
        {
            emission.rateOverTime = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            holdingTouch = false;
            swipeDirection = Vector2.zero;
        }

        if (transform.position.y < -10)
        {
            Debug.LogError("You died.");
        }
        if (transform.position.y > 0.5f && !landing)
        {
            inAir = true;
        }

        if (inAir)
        {
            var size = GetComponent<BoxCollider>().size;
            Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red); Debug.DrawRay(transform.position, Vector3.up * (size.y + 0.1f), Color.red);
            Debug.DrawRay(transform.position, Vector3.left * (size.z / 2 + 0.1f), Color.red); Debug.DrawRay(transform.position, Vector3.right * (size.z / 2 + 0.1f), Color.red);
            if (Physics.Raycast(transform.position, Vector3.down, 0.1f, 64) || Physics.Raycast(transform.position, Vector3.up, size.y + 0.1f, 64)
                || Physics.Raycast(transform.position, Vector3.left, size.z / 2 + 0.1f, 64) || Physics.Raycast(transform.position, Vector3.right, size.z / 2 + 0.1f, 64))
            {
                StartCoroutine(AirToGroundCounter());
            }
            tireTracks[0].emitting = false;
            tireTracks[1].emitting = false;
            tireTracks[2].emitting = false;
            tireTracks[3].emitting = false;
        }
        else
        {
            tireTracks[0].emitting = true;
            tireTracks[1].emitting = true;
            tireTracks[2].emitting = true;
            tireTracks[3].emitting = true;
        }
    }
    private void FixedUpdate()
    {
        if (!carActive) return;
        if (!inAir)
        {
            MoveCar();
        }
        if (landing) AirToGround();
    }
    private void ClampSwipe()
    {
        Mathf.Clamp(horizontal, -maxClampX, maxClampX);
        horizontal /= maxClampX;
        Mathf.Clamp(vertical, -maxClampY, maxClampY);
        vertical /= maxClampY;
        Debug.Log($"Horizontal: {horizontal}, Vertical: {vertical}");
    }

    private void MoveCar()
    {
        //transform.Rotate(Vector3.up, (horizontal * driftSpeed) / 10, Space.Self);
        transform.RotateAround(driftPivotPoint.transform.position, Vector3.up, (horizontal * driftSpeed) / 10);
        transform.position += (transform.right * vertical * carSpeed) / 100;
    }

    private void AirToGround()
    {
        Vector3 carPos = transform.position;
        Vector3 targetPos = new Vector3(carPos.x, -0.05f, carPos.z);
        Quaternion targetRot = new Quaternion(0, transform.localRotation.y, 0, transform.localRotation.w);
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, 0.5f);
    }

    private IEnumerator AirToGroundCounter()
    {
        inAir = false;
        landing = true;
        yield return new WaitForSeconds(0.3f);
        landing = false;
    }
}


