using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject drop;
    [SerializeField]
    private Text debugText;
    [SerializeField]
    private Rigidbody carRB;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnDrop", 0.0f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = "Velocity: " + carRB.velocity.magnitude;
    }

    private void SpawnDrop()
    {
        Instantiate(drop, new Vector3(Random.Range(-30,30),30, Random.Range(-30, 30)), drop.transform.rotation);
    }

}
