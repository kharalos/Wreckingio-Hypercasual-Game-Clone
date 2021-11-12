using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject car;
    private Rigidbody carRB;

    private Vector2 startTouch, currentMousePos, swipeDirection;

    private bool inAir;
    private bool landing;

    [SerializeField]
    private float maxClampX = 90f, maxClampY = 200f, carSpeed = 10f, driftSpeed = 2f;

    private float horizontal, vertical;

    private bool holdingTouch;
    void Start()
    {
        carRB = car.GetComponent<Rigidbody>();
        holdingTouch = false;
        horizontal = vertical = 0;
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

        if (Input.GetMouseButtonUp(0))
        {
            holdingTouch = false;
            swipeDirection = Vector3.zero;
        }

        if (transform.position.y < -10)
        {
            Debug.LogError("You died.");
        }
        if (transform.position.y > 2 && !landing)
        {
            inAir = true;
        }
        if (inAir)
        {
            Debug.DrawRay(car.transform.position, Vector3.down * 2, Color.red);
            if (Physics.Raycast(car.transform.position, Vector3.down, 2, 64))
            {
                StartCoroutine(AirToGroundCounter());
            }
        }
    }
    private void FixedUpdate()
    {
        ClampSwipe();
        MoveCar();
        if (landing) AirToGround();
    }
    private void ClampSwipe()
    {
        Mathf.Clamp(horizontal, -maxClampX, maxClampX);
        horizontal /= maxClampX;
        Mathf.Clamp(vertical, -maxClampY, maxClampY);
        vertical /= maxClampY;
    }

    private void MoveCar()
    {
        transform.Rotate(Vector3.up, (horizontal * driftSpeed) / 10, Space.Self);
        transform.position += (car.transform.right * vertical * carSpeed) / 100;
    }

    private void AirToGround()
    {
        Vector3 carPos = car.transform.position;
        Vector3 targetPos = new Vector3(carPos.x, -0.05f, carPos.z);
        Quaternion targetRot = new Quaternion(0, car.transform.localRotation.y, 0, car.transform.localRotation.w);
        car.transform.position = Vector3.Lerp(car.transform.position, targetPos, 0.5f);
        car.transform.localRotation = Quaternion.Lerp(car.transform.localRotation, targetRot, 0.5f);
    }

    private IEnumerator AirToGroundCounter()
    {
        inAir = false;
        landing = true;
        yield return new WaitForSeconds(0.3f);
        landing = false;
    }
}


