using System.Linq;
using UnityEngine;

public class StateAgent : AIAgent
{
    [SerializeField] Perception perception;
    public StateMachine stateMachine = new StateMachine();

    public ValueRef<float> timer = new ValueRef<float>();
    public ValueRef<float> health = new ValueRef<float>();
    public ValueRef<float> destinationDistance = new ValueRef<float>();
    public ValueRef<float> enemyDistance = new ValueRef<float>();
    public ValueRef<bool> enemySeen = new ValueRef<bool>();

    public AIAgent enemy;

    private void Start()
    {
        //health.value = 100;

        stateMachine.AddState(nameof(AIIdleState), new AIIdleState(this));
        stateMachine.AddState(nameof(AIPatrolState), new AIPatrolState(this));
        stateMachine.AddState(nameof(AIChaseState), new AIChaseState(this));
        //stateMachine.AddState(nameof(AIHitState), new AIHitState(this));
        stateMachine.AddState(nameof(AIAttackState), new AIAttackState(this));


        stateMachine.SetState(nameof(AIIdleState));
    }

    private void Update()
    {
        //update parameters
        timer.value -= Time.deltaTime;

        if(perception!=null)
        {
            var gameObjects = perception.GetGameObjects();
            enemySeen.value = gameObjects.Length > 0;
            if (gameObjects.Length>0)
            {
                gameObjects[0].TryGetComponent<AIAgent>(out enemy);
                enemy = gameObjects[0].GetComponent<AIAgent>();
                enemyDistance.value = transform.position.DistanceXZ(enemy.transform.position);
                //movement.Destination = gameObjects[0].transform.position;
            }
        }
        destinationDistance.value = transform.position.DistanceXZ(movement.Destination);

        // Vector3 direction = (agent.transform.position - destination);
        // direction.y = 0;
        // float distance = direction.magnitude;

        stateMachine.CurrentState?.CheckTransitions();
        stateMachine.Update();

        // //SEEK
    }

    private void OnGUI()
	{
		// draw label of current state above agent
		GUI.backgroundColor = Color.black;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		Rect rect = new Rect(0, 0, 100, 20);
		// get point above agent
		Vector3 point = Camera.main.WorldToScreenPoint(transform.position);
		rect.x = point.x - (rect.width / 2);
		rect.y = Screen.height - point.y - rect.height - 20;
		// draw label with current state name
		GUI.Label(rect, stateMachine.CurrentState.Name);
	}

    public void OnDamage(float damage)
    {
        health.value -= damage;

        //if (health > 0) stateMachine.SetState(nameof(AIHitState));
        //else stateMachine.SetState(nameof(AIDeathState));
    }

    public void Attack()
    {
        // check for collision with surroundings
        var colliders = Physics.OverlapSphere(transform.position, 3);
        foreach (var collider in colliders)
        {
            // enable collision only with enemy
            if (collider.gameObject != enemy) continue;

            // check if collider object is a state agent, damage agent
            if (collider.gameObject.TryGetComponent<StateAgent>(out var agent))
            {
                agent.OnDamage(Random.Range(20, 50));
            }
        }
    }
}
