using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject driftPivotPoint;

    [SerializeField]
    private float driftSpeed = 5, carSpeed = 10;

    private bool inAir;
    private bool landing;
    private bool isDrifting;

    [SerializeField]
    private TrailRenderer[] tireTracks;
    [SerializeField]
    private ParticleSystem driftEffect;

    private Vector3 centrePos, playerPos, lookPos, destination;

    private Rigidbody m_Rigidbody;
    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        FindObjectOfType<GameManager>().EnemyNumberChange(true);
    }
    private void FixedUpdate()
    {
        if (landing) AirToGround();
        if (isDrifting) transform.RotateAround(driftPivotPoint.transform.position, Vector3.up, driftSpeed);
        if (!inAir)
        {
            if (!isDrifting)
                transform.rotation = Quaternion.Slerp(transform.rotation
                                      , Quaternion.LookRotation((playerPos + destination) - transform.position)
                                      , 3f * Time.deltaTime);

            /* Move at Player*/
            transform.position += transform.forward * carSpeed * Time.deltaTime;
        }
    }
    void Update()
    {

        centrePos = GameObject.FindGameObjectWithTag("Centre").transform.position;
        if(GameObject.FindGameObjectWithTag("Player"))
            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().transform.position;

        destination = (centrePos - playerPos).normalized * 7;

        if ((playerPos - transform.position).magnitude < 15)
        {
            isDrifting = true;
        }
        else isDrifting = false;

        if (transform.position.y < -10)
        {
            FindObjectOfType<GameManager>().EnemyNumberChange(false);
            Destroy(gameObject.transform.parent.gameObject);
        }
        var emission = driftEffect.emission;
        if (isDrifting)
        {
            emission.rateOverTime = 20;
        }
        else
        {
            emission.rateOverTime = 0;
        }
        if (transform.position.y > 0.5f && !landing)
        {
            inAir = true;
        }

        if (inAir)
        {
            var size = GetComponent<BoxCollider>().size;
            Debug.DrawRay(transform.position, -transform.up * 0.1f, Color.red); Debug.DrawRay(transform.position, transform.up * (size.y + 0.1f), Color.red);
            if (Physics.Raycast(transform.position, -transform.up, 0.1f, 64) || Physics.Raycast(transform.position, transform.up, size.y + 0.1f, 64))
            {
                StartCoroutine(AirToGroundCounter());
            }
            if (GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
            {
                StartCoroutine(AirToGroundCounter());
            }
            for (int i = 0; i < tireTracks.Length; i++)
            {
                tireTracks[i].emitting = false;
            }
        }
        else
        {
            for (int i = 0; i < tireTracks.Length; i++)
            {
                tireTracks[i].emitting = true;
            }
        }

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
