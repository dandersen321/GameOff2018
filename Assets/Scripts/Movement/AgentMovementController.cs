using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovementController : MonoBehaviour
{

    public BodyController bodyController;
    public NavMeshAgent agent;

    public GameObject rotateOverride = null;

    public bool stopMoving = false;

    // Use this for initialization
    void Start()
    {
        bodyController = GetComponent<BodyController>();
        initAgent();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stopMoving)
            return;
        moveTowardsTarget();
    }

    private void initAgent()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = bodyController.speed;
        // setting a higher acceleration helps the agent rotate faster
        agent.acceleration = bodyController.speed *= 2;
        agent.autoBraking = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    Vector3 getNextPosition()
    {
        Vector3 targetDestination = getPlayerPosition();
        agent.SetDestination(targetDestination);
        return agent.nextPosition;
    }

    void moveTowardsTarget()
    {
        //Debug.Log("Moving!");
        Vector3 directionToTarget = bodyController.getDirectionToTarget(getNextPosition());
        bodyController.moveInDirection(directionToTarget);
        if (rotateOverride != null)
        {
            bodyController.lookInDirection(bodyController.getDirectionToTarget(rotateOverride.transform.position));
        }
        else
        {
            bodyController.lookInDirection(directionToTarget);
        }
        // since the agent is not actively moving, we have to keep it updated to where it is
        agent.nextPosition = this.transform.position;
    }

    Vector3 getPlayerPosition()
    {
        return References.GetPlayer().transform.position;
    }

    public void ragDoll()
    {
        bodyController.Ragdoll();
        this.enabled = false;
    }

}
