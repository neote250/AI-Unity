using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AIAttackState : AIState
{
    float attackTimer;
    bool hasAttacked;
    public AIAttackState(StateAgent agent) : base(agent)
    {
        CreateTransition(nameof(AIIdleState))
            .AddCondition(agent.enemySeen, false);

        CreateTransition(nameof(AIChaseState))
            .AddCondition(agent.timer, Condition.Predicate.LessOrEqual, 0);
    }

    public override void OnEnter()
    {
        attackTimer = 2;
        hasAttacked = false;

        agent.timer.value = 2;
        agent.movement.Stop();
        //agent.animator.SetTrigger("Attack");
    }

    public override void OnUpdate()
    {
        Vector3 direction = agent.enemy.transform.position-agent.transform.position;
        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * 5);

        attackTimer -= Time.deltaTime;
        if(!hasAttacked && attackTimer<=0) //easiest first, bool compare is cheaper
        {
            hasAttacked = true;
            agent.Attack();
        }
    }

    public override void OnExit()
    {
    }

    // IEnumerator Attack()
    // {
    //     yield return new WaitForSeconds(1);
    //     agent.Attack();
    // }
}
