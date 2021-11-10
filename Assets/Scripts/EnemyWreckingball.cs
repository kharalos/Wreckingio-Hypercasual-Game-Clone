using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWreckingball : MonoBehaviour
{
    private Rigidbody rigidBody;
    [SerializeField]
    private float forcePower = 25000;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            player.AgentGotHit(rigidBody.velocity.magnitude);

            Vector3 dir = (collision.gameObject.transform.position - gameObject.transform.position).normalized;
            dir.y = 0;
            Vector3 launchDirection = (dir + Vector3.up) * (rigidBody.velocity.magnitude) * forcePower;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(launchDirection);
            Debug.Log("Enemy car launched.");
        }
    }
}
