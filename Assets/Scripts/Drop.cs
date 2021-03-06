using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField]
    private GameObject parachute;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Car")
        {
            //Powerup
            collision.gameObject.transform.parent.GetComponentInChildren<WreckingBallControll>().Spin();

            Destroy(gameObject);
        }
        else
        {
            parachute.SetActive(false);
        }
    }
    private void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
