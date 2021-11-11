using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private SpringJoint springJoint;
    private Camera cam;
    [SerializeField]
    private LayerMask movementMask;
    private NavMeshAgent agent;

    [SerializeField]
    private Text velText;

    public static bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;
    private Vector3 destination;

    private bool inAir;
    private bool gotHit;

    [SerializeField]
    private Rigidbody ballRB;
    private float ballYPos;

    private Animator animator;
    private int rightDriftStack, leftDriftStack;
    void Start()
    {
        springJoint = GetComponentInChildren<SpringJoint>();
        cam = Camera.main;
        agent = GetComponentInChildren<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        ballYPos = ballRB.transform.position.y;
    }
    void Update()
    {
        velText.text = GetComponentInChildren<Rigidbody>().velocity.magnitude.ToString(); ;
        tap = swipeDown = swipeUp = swipeLeft = swipeRight = false;
        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;

            if (agent.enabled && rightDriftStack == 0 && leftDriftStack == 0)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, movementMask))
                {
                    destination = hit.point;
                    MoveToPoint(destination);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {

            isDraging = false;
            Reset();
        }
        #endregion

        #region Mobile Input
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                Reset();
            }
        }
        #endregion

        //Calculate the distance
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length < 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        //Did we cross the distance?
        if (swipeDelta.magnitude > 100)
        {
            //Which direction?
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or Right
                if (x < 0)
                {
                    swipeLeft = true;
                }
                else
                {
                    swipeRight = true;
                }
            }
            else
            {
                //Up or Down
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }

            Reset();
        }
        Drift();

        if (agent.transform.position.y < -10)
        {
            Debug.LogError("You are dead");
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

    private void Drift()
    {
        if (swipeLeft && leftDriftStack < 3)
        {
            StopCoroutine(RotateAgent(false));
            StartCoroutine(RotateAgent(true));
            Debug.Log("Swiped Left");
        }

        if (swipeRight && rightDriftStack < 3)
        {
            StopCoroutine(RotateAgent(true));
            StartCoroutine(RotateAgent(false));
            Debug.Log("Swiped Right");
        }
    }

    private IEnumerator RotateAgent(bool Left)
    {
        if (Left)
        {
            leftDriftStack++;
            for (int i = 0; i < 50; i++)
            {
                agent.transform.position += agent.transform.forward / 8;
                agent.transform.Rotate(Vector3.up, 3f, Space.Self);
                yield return new WaitForSeconds(0.01f);
            }
            leftDriftStack--;
        }
        else
        {
            rightDriftStack++;
            for (int i = 0; i < 50; i++)
            {
                agent.transform.position += agent.transform.forward / 8;
                agent.transform.Rotate(Vector3.up, -3f, Space.Self);
                yield return new WaitForSeconds(0.01f);
            }
            rightDriftStack--;
        }
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
    private void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
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
