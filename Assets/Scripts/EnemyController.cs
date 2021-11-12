using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject driftPivotPoint;

    private bool inAir;
    private bool landing;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject.transform.parent.gameObject);
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
        }
    }
    private void FixedUpdate()
    {
        if (landing) AirToGround();
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
