using UnityEngine;
using UnityEngine.AI;

public class AIPatrolState : AIState
{
    public AIPatrolState(StateAgent agent) : base(agent)
    {
        CreateTransition(nameof(AIIdleState))
            .AddCondition(agent.destinationDistance, Condition.Predicate.Less, 0.5f);

        CreateTransition(nameof(AIChaseState))
            .AddCondition(agent.health, Condition.Predicate.Greater, 2)
            .AddCondition(agent.enemySeen, true);
    }

    public override void OnEnter()
    {
        agent.movement.Destination = NavNode.GetRandomNavNode().transform.position;
        agent.movement.Resume();
    }

    public override void OnUpdate()
    {
        //
    }

    public override void OnExit()
    {
        //
    }

}
