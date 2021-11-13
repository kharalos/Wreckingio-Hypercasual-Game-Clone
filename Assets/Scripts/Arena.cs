using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Arena : MonoBehaviour
{

    public Rigidbody[] rigidbodies;
    private void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }
    public void MakeItFall(int phase)
    {
        switch (phase)
        {
            case 2:
                for(int i = 0; i <30; i++)
                {
                    rigidbodies[i].isKinematic = false;
                    rigidbodies[i].useGravity = true;
                }
                break;
            case 3:
                for (int i = 30; i < 55; i++)
                {
                    rigidbodies[i].isKinematic = false;
                    rigidbodies[i].useGravity = true;
                }
                break;
            case 4:
                for (int i = 55; i < 75; i++)
                {
                    rigidbodies[i].isKinematic = false;
                    rigidbodies[i].useGravity = true;
                }
                break;
            case 5:
                for (int i = 75; i < 85; i++)
                {
                    rigidbodies[i].isKinematic = false;
                    rigidbodies[i].useGravity = true;
                }
                break;
            case 6:
                for (int i = 85; i < 100; i++)
                {
                    rigidbodies[i].isKinematic = false;
                    rigidbodies[i].useGravity = true;
                }
                break;
        }
    }

}
