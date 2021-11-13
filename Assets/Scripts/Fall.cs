using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y < -80)
        {
            Destroy(gameObject);
        }
    }
}
