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

    public bool attackPlayer = false;

    public Vector3 ufoStartingPosition;
    public Vector3 artifactGuardOffset = Vector3.zero;

    private Timer nextArtifactCheck = new Timer();
    private Timer nextArtifactBodyCheck = new Timer();
    //private Timer nextArtifactFinishCheck = new Timer();

    // Use this for initialization
    void Start()
    {
        bodyController = GetComponent<BodyController>();
        initAgent();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!attackPlayer && nextArtifactCheck.Expired())
        {
            updateArtifactCheck();
        }

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
        Vector3 targetDestination = attackPlayer ? getPlayerPosition() : getArtifactPosition();
        agent.SetDestination(targetDestination);
        return agent.nextPosition;
    }

    Vector3 getArtifactPosition()
    {
        Artifact artifact = References.getArtifact();
        if(artifact.heldBy == this.gameObject.GetComponent<Enemy>())
        {
            // we are the runner
            return ufoStartingPosition;
        }
        else if(artifact.heldBy != null)
        {
            if(artifactGuardOffset == Vector3.zero && nextArtifactBodyCheck.Expired())
            {
                if(Vector3.Distance(this.transform.position, References.getArtifact().transform.position) < 3f)
                {
                    artifactGuardOffset = this.transform.position - References.getArtifact().transform.position;
                }
            }

            nextArtifactBodyCheck.Start(1f);

            return References.getArtifact().transform.position + artifactGuardOffset;
        }
        else
        {
            // no one holds the artifiact
            return References.getArtifact().transform.position;
        }

        
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

    private void updateArtifactCheck()
    {
        Artifact artifact = References.getArtifact();
        if (artifact.heldBy == null && Vector3.Distance(this.transform.position, References.getArtifact().transform.position) < 3f)
        {
            // grab artifact
            grabArtifact();
        }

        if(artifact.heldBy == this.gameObject.GetComponent<Enemy>())
        {
            if (Vector3.Distance(this.transform.position, ufoStartingPosition) < 2f)
            {
                Debug.Log("You Lose!");
            }
            else
            {
                Debug.Log("Distance to lose: " + Vector3.Distance(this.transform.position, ufoStartingPosition));
            }
        }

        nextArtifactCheck.Start(1f);
    }

    private void grabArtifact()
    {
        References.getArtifact().heldBy = this.gameObject.GetComponent<Enemy>();
        References.getArtifact().transform.position = this.gameObject.transform.position + new Vector3(0, 1, 1);
        References.getArtifact().transform.parent = this.gameObject.transform;


    }

    public void ragDoll()
    {
        bodyController.Ragdoll();
        this.enabled = false;
    }



}
