using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private SpringJoint springJoint;
    private NavMeshAgent agent; 
    void Start()
    {
        springJoint = GetComponentInChildren<SpringJoint>();
        agent = GetComponentInChildren<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.enabled)
        {
            agent.SetDestination(GameObject.FindGameObjectWithTag("SwirlingTarget").transform.position);
        }
        if(agent.transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    public void AgentGotHit(float impactScore)
    {
        agent.enabled = false;
        impactScore /= 10;
        Debug.Log(impactScore);
        StartCoroutine(FloatingCorouting(impactScore));
    }

    private IEnumerator FloatingCorouting(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Vector3 agentPos = agent.transform.position;
        Vector3 targetPos = new Vector3(agentPos.x, 0.18816733360290528f, agentPos.z);
        Quaternion targetRot = new Quaternion(0, agent.transform.localRotation.y, 0, agent.transform.localRotation.w);
        Debug.DrawRay(agentPos, Vector3.down*10, Color.red);
        if (Physics.Raycast(agentPos, Vector3.down, out RaycastHit slopeHit))
        {
            if (waitTime > 1)
            {
                for (int i = 0; i < 100; i++)
                {
                    yield return new WaitForSeconds(waitTime/100);
                    agent.transform.position = Vector3.Lerp(agent.transform.position, targetPos, 0.1f);
                    agent.transform.localRotation = Quaternion.Lerp(agent.transform.localRotation, targetRot, 0.1f);
                }
            }
            agent.enabled = true;
        }
        else
            agent.enabled = false;
    }
}
