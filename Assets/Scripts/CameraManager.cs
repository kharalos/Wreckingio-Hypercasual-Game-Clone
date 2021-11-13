using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerTransform)
        {
            transform.LookAt(playerTransform);
            transform.position = Vector3.Lerp(transform.position, new Vector3(playerTransform.position.x, playerTransform.position.y + 15f, playerTransform.position.z + 15f), 0.5f);
        }
    }
}
