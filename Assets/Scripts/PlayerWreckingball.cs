using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWreckingball : MonoBehaviour
{
    private Rigidbody rigidBody;
    [SerializeField]
    private float forcePower = 25000;

    [SerializeField]
    private Animator animator;
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
            if (vel > 0.3f)
            {
                enemy.AgentGotHit(vel);

                Vector3 dir = (collision.gameObject.transform.position - gameObject.transform.position).normalized;
                dir.y = 0;
                dir.Normalize();
                Vector3 launchDirection;

                if (vel < 1)
                {
                    launchDirection = dir * forcePower;
                }
                else
                {
                    dir = ((dir) + Vector3.up).normalized;

                    launchDirection = dir * (vel) * forcePower;
                }

                collision.gameObject.GetComponent<Rigidbody>().AddForce(launchDirection);
                animator.SetTrigger("Hit");
                Debug.Log("Enemy car launched.");
            }
            else Debug.Log("Wrecking ball touched the other car.");
        }
    }
}
