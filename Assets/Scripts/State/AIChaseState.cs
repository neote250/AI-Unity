using UnityEngine;

public class AIChaseState : AIState
{
    [SerializeField] Perception perception;

    public AIChaseState(StateAgent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        if(perception!=null)
        {
            var gameObjects = perception.GetGameObjects();
            if (gameObjects.Length>0)
            {
                agent.movement.Destination = gameObjects[0].transform.position;
            }
            agent.movement.Resume();
        }
    }

    public override void OnUpdate()
    {
        Vector3 direction = (agent.transform.position - agent.movement.Destination);
        direction.y = 0;
        float distance = direction.magnitude;
        if (distance<=0.5f)
        {
            agent.stateMachine.SetState(nameof(AIIdleState));
        }
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
