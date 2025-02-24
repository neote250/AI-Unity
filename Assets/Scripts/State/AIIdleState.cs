using UnityEngine;

public class AIIdleState : AIState
{
    // ValueCondition<float> distanceCheck;
    ValueCondition<float> timerCheck;
    public AIIdleState(StateAgent agent) : base(agent)
    {
        CreateTransition(nameof(AIPatrolState))
            .AddCondition(agent.timer, Condition.Predicate.LessOrEqual, 0);

        CreateTransition(nameof(AIChaseState))
            .AddCondition(agent.health, Condition.Predicate.Greater, 2)
            .AddCondition(agent.enemySeen, true);

        // var transition = new StateTransition(nameof(AIPatrolState));
        // transition.AddCondition(new ValueCondition<float>
        // (
        //     agent.timer, Condition.Predicate.Less, 5
        // ));
        // timerCheck = new ValueCondition<float>
        // (
        //     agent.timer, Condition.Predicate.Less, 5
        // );
        // timerCheck.IsTrue();
    }

    public override void OnEnter()
    {
        agent.timer.value = Random.Range(1,3);
        agent.movement.Stop();
    }

    public override void OnUpdate()
    {
        // if (agent.timer<=0)
        // {
        //     agent.stateMachine.SetState(nameof(AIPatrolState));
        // }
    }

    public override void OnExit()
    {
        //
    }
}
