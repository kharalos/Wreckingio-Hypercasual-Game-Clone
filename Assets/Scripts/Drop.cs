using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField]
    private GameObject parachute;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //Powerup
            collision.gameObject.transform.parent.GetComponentInChildren<EnemyWreckingball>().Spin();

            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Player")
        {
            //Powerup
            collision.gameObject.transform.parent.GetComponentInChildren<PlayerWreckingball>().Spin();

            Destroy(gameObject);
        }
        else
        {
            parachute.SetActive(false);
        }
    }
}
