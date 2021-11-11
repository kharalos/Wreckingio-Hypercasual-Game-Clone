using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private SpringJoint springJoint;
    private NavMeshAgent agent;
    private bool inAir;
    private bool gotHit;

    [SerializeField]
    private Rigidbody ballRB;
    private float ballYPos;

    private Animator animator;
    private bool isDrifting;

    void Start()
    {
        springJoint = GetComponentInChildren<SpringJoint>();
        agent = GetComponentInChildren<NavMeshAgent>();
        ballYPos = ballRB.transform.position.y;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (agent.enabled)
        {
            Vector3 centrePos = GameObject.FindGameObjectWithTag("Centre").transform.position;
            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<NavMeshAgent>().transform.position;
            Vector3 destination = (centrePos - playerPos).normalized * 7;
            if ((playerPos - agent.transform.position).magnitude < 15 && !isDrifting)
            {
                StartCoroutine(Drift());
            }
            agent.SetDestination(playerPos + destination);
        }
        if (agent.transform.position.y < -10)
        {
            Destroy(gameObject);
        }
        if (gotHit)
        {
            if (GetComponentInChildren<Rigidbody>().velocity.magnitude < 1)
            {
                gotHit = false;
                inAir = true;
            }
        }
        if (inAir)
        {
            Debug.DrawRay(agent.transform.position, Vector3.down * 2, Color.red);
            if (Physics.Raycast(agent.transform.position, Vector3.down, 2, 64))
            {
                StartCoroutine(AirToGround());
            }
        }
    }
    private IEnumerator Drift()
    {
        isDrifting = true;
        for (int i = 0; i < 10; i++)
        {
            agent.transform.position += agent.transform.forward / 3;
            agent.transform.Rotate(Vector3.up, 1f, Space.Self);
            yield return new WaitForSeconds(0.01f);
        }
        isDrifting = false;
    }
    public void AgentGotHit(float impactScore)
    {
        agent.enabled = false;
        gotHit = true;
        ballRB.constraints = RigidbodyConstraints.None;
        Debug.Log("Player ball hit enemy with velocity of " + impactScore);
    }
    private IEnumerator AirToGround()
    {
        inAir = false;
        Vector3 agentPos = agent.transform.position;
        Vector3 targetPos = new Vector3(agentPos.x, -0.04999947547912598f, agentPos.z);
        Quaternion targetRot = new Quaternion(0, agent.transform.localRotation.y, 0, agent.transform.localRotation.w);


        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(0.01f);
            agent.transform.position = Vector3.Lerp(agent.transform.position, targetPos, 0.5f);
            agent.transform.localRotation = Quaternion.Lerp(agent.transform.localRotation, targetRot, 0.5f);
            ballRB.transform.position = Vector3.Lerp(ballRB.transform.position, new Vector3(ballRB.transform.position.x, ballYPos, ballRB.transform.position.z), 0.5f);
        }
        ballRB.constraints = RigidbodyConstraints.FreezePositionY;
        agent.enabled = true;
    }
}

