using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBallControll : MonoBehaviour
{
    [SerializeField]
    private Transform targetPos, carPos;
    private Vector3 targetOriginalLocalPos;
    private Rigidbody m_Rigidbody;
    private bool isSpinning;

    [SerializeField]
    private float movePower = 1000f;

    [SerializeField]
    LineRenderer robe;

    private TrailRenderer trail;

    private bool isPlayer;
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
        if (transform.parent.gameObject.tag == "Player") isPlayer = true;
        else isPlayer = false;
        m_Rigidbody = GetComponent<Rigidbody>();
        targetOriginalLocalPos = targetPos.localPosition;
        isSpinning = false;
    }

    void FixedUpdate()
    {
        Vector3 targetDir = targetPos.position - transform.position;
        m_Rigidbody.velocity = targetDir * movePower * Time.deltaTime;
        if (isSpinning)
        {
            targetPos.RotateAround(carPos.position, Vector3.up, 10f);
            robe.enabled = false;
            trail.emitting = true;
        }
        else
        {
            robe.enabled = true;
            trail.emitting = false;
            targetPos.localPosition = targetOriginalLocalPos;
        }
    }
    public void Spin()
    {
        StartCoroutine(SpinAround());
    }
    private IEnumerator SpinAround()
    {
        isSpinning = true;
        yield return new WaitForSeconds(10f);
        isSpinning = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Car" && collision.gameObject != carPos.gameObject)
        {
            float vel = m_Rigidbody.velocity.magnitude;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * vel * 10000);

            gameObject.GetComponentInParent<AnimatorController>().Hit();

            Debug.Log($"Car launched with velocity of {vel}.");

            if (isPlayer) transform.parent.GetComponent<Customizer>().Hit(vel);
        }
    }
}
