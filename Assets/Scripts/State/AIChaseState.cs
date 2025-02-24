using UnityEngine;

public class AIChaseState : AIState
{
    [SerializeField] Perception perception;

    public AIChaseState(StateAgent agent) : base(agent)
    {
        CreateTransition(nameof(AIIdleState))
            .AddCondition(agent.enemySeen, false);

        CreateTransition(nameof(AIAttackState))
            .AddCondition(agent.enemyDistance, Condition.Predicate.LessOrEqual, 1.5f);

    // Chase State:
    //     If an enemy is detected using their Perception, transition to Chase.
    //     Move towards the enemy at an increased speed.
    //     If within attack range, transition to Attack.
    //     If the enemy escapes and not seen, transition to Idle.

    // Attack State:
    //     Detect enemies using their Perception.
    //     If an enemy is within range, transition to Attack state.
    //     Play an attack animation and apply damage using the OnDamage() function.
    //     Transition back to Idle after attacking.

    // Hit State:
    //     Triggered when an agent is hit by an enemy.
    //     Play a hit animation and reduce health.
    //     Return to Patrol or Attack if still alive.

    // Death State:
    //     If health reaches zero, transition to Death state.
    //     Play a death animation.
    //     Destroy the agent GameObject after a short delay (Destroy(gameObject, 3f);).

    }

    public override void OnEnter()
    {
        agent.movement.MaxSpeed *= 2;

        agent.movement.Resume();
        //if(perception!=null)
        // {
        //     var gameObjects = perception.GetGameObjects();
        //     if (gameObjects.Length>0)
        //     {
        //         agent.movement.Destination = gameObjects[0].transform.position;
        //     }
        // }
    }

    public override void OnUpdate()
    {
        agent.movement.Destination = agent.enemy.transform.position;

        // Vector3 direction = agent.transform.position - agent.movement.Destination;
        // direction.y = 0;
        // float distance = direction.magnitude;
        // if (agent.transform.position.DistanceXZ(agent.movement.Destination)<=0.5f)
        // {
        //     agent.stateMachine.SetState(nameof(AIIdleState));
        // }
    }

    public override void OnExit()
    {
        agent.movement.Stop();
        agent.movement.MaxSpeed /= 2;
    }
}
