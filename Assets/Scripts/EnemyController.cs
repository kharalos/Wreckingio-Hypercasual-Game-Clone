using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject car;
    private Rigidbody carRB;

    private bool inAir;
    private bool landing;
    // Start is called before the first frame update
    void Start()
    {
        carRB = car.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
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
        if (landing) AirToGround();
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
