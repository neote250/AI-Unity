using UnityEngine;

public class AIIdleState : AIState
{
    float timer;

    public AIIdleState(StateAgent agent) : base(agent)
    {
        //
    }

    public override void OnEnter()
    {
        timer = Random.Range(1,3);
        agent.movement.Stop();
    }

    public override void OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer<=0)
        {
            agent.stateMachine.SetState(nameof(AIPatrolState));
        }
    }

    public override void OnExit()
    {
        //
    }
}
