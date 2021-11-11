using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWreckingball : MonoBehaviour
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
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
            float vel = rigidBody.velocity.magnitude;
            if (vel > 0.5f)
            {
                enemy.AgentGotHit(vel);

                Vector3 dir = (collision.gameObject.transform.position - gameObject.transform.position).normalized;
                dir.y = 0;
                Vector3 launchDirection;

                if (vel < 1) launchDirection = (dir + Vector3.up) * forcePower;
                else launchDirection = (dir + Vector3.up) * (vel) * forcePower;

                collision.gameObject.GetComponent<Rigidbody>().AddForce(launchDirection);
                Debug.Log("Enemy car launched.");
            }
            else Debug.Log("Wrecking ball touched the other car.");
        }
    }
}
