using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private SpringJoint springJoint;
    private Camera cam;
    [SerializeField]
    private LayerMask movementMask;
    private NavMeshAgent agent;

    void Start()
    {
        springJoint = GetComponentInChildren<SpringJoint>();
        cam = Camera.main;
        agent = GetComponentInChildren<NavMeshAgent>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0) && agent.enabled)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                MoveToPoint(hit.point, true);
            }
        }
        if (Input.GetMouseButtonUp(0) && agent.enabled)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                MoveToPoint(hit.point, false);
            }
        }
    }
    private void MoveToPoint(Vector3 point, bool holdingTouch)
    {
        if (holdingTouch)
        {
            agent.SetDestination(point);
        }
        else
            agent.SetDestination(point);
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
        Debug.DrawRay(agentPos, Vector3.down * 10, Color.red);
        if (Physics.Raycast(agentPos, Vector3.down, out RaycastHit slopeHit))
        {
            if (waitTime > 1)
            {
                for (int i = 0; i < 100; i++)
                {
                    yield return new WaitForSeconds(waitTime / 200);
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
