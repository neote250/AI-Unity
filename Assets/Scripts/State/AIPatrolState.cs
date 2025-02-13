using UnityEngine;
using UnityEngine.AI;

public class AIPatrolState : AIState
{
    Vector3 destination = Vector3.zero;

    public AIPatrolState(StateAgent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        destination = NavNode.GetRandomNavNode().transform.position;
        agent.movement.Destination = destination;
        agent.movement.Resume();
    }

    public override void OnUpdate()
    {
        Vector3 direction = (agent.transform.position - destination);
        direction.y = 0;
        float distance = direction.magnitude;
        if (distance<=0.5f)
        {
            agent.stateMachine.SetState(nameof(AIIdleState));
        }
    }

    public override void OnExit()
    {

    }

}
