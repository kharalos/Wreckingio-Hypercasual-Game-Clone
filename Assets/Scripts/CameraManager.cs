using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerTransform);
    }
}
