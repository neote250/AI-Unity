using Unity.VisualScripting;
using UnityEngine;

public class AutonomousAgent : AIAgent
{
    [SerializeField] AutonomousAgentData data;

    [Header("Perception")]
    public Perception seekPerception;
    public Perception fleePerception;    
    public Perception flockPerception;    

    float angle;

    private void Update()
    {
        //movement.ApplyForce(Vector3.forward * 10);
        
        //Debug.DrawRay(transform.position, transform.forward * seekPerception.maxDistance, Color.yellow);

        //SEEK
        if(seekPerception!=null)
        {
            var gameObjects = seekPerception.GetGameObjects();
            if (gameObjects.Length>0)
            {
                Vector3 force = Seek(gameObjects[0]);
                movement.ApplyForce(force);
            }
            foreach(var go in gameObjects)
            {
                Debug.DrawLine(transform.position, go.transform.position, Color.blue);
            }
        }

        //FLEE
        if(fleePerception!=null)
        {
            var gameObjects = fleePerception.GetGameObjects();
            if (gameObjects.Length>0)
            {
                Vector3 force = Flee(gameObjects[0]);
                movement.ApplyForce(force);
            }
            foreach(var go in gameObjects)
            {
                Debug.DrawLine(transform.position, go.transform.position, Color.red);
            }
        }

        //Flock
        if(flockPerception!=null)
        {
            var gameObjects = flockPerception.GetGameObjects();
            if (gameObjects.Length>0)
            {
                movement.ApplyForce(Cohesion(gameObjects) * data.CohesionWeight);
                movement.ApplyForce(Separation(gameObjects, data.SeparationRadius) * data.SeparationWeight);
                movement.ApplyForce(Alignment(gameObjects) * data.AlignmentWeight);
            }
        }

        //WANDER
        if(movement.Acceleration.sqrMagnitude == 0)
        {
            Vector3 force = Wander();
            movement.ApplyForce(force);
        }

        Vector3 acceleration = movement.Acceleration;
        acceleration.y = 0;
        movement.Acceleration = acceleration;

        if (movement.Direction.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement.Direction);
        }


        float mapSize = 10;
        transform.position = Utilities.Wrap(transform.position, new Vector3(-mapSize,-mapSize,-mapSize), new Vector3(mapSize,mapSize,mapSize));


    }
    private Vector3 Seek(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }
    private Vector3 Flee(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(-direction);
        return force;
    }
    private Vector3 Cohesion(GameObject[] neighbors)
    {
        Vector3 positions = Vector3.zero;
        foreach(var neighbor in neighbors)
        {
            positions += neighbor.transform.position;
        }
        Vector3 center = positions/neighbors.Length;
        Vector3 direction = center - transform.position;

        Vector3 force = GetSteeringForce(direction);

        return force;
    }
    private Vector3 Separation(GameObject[] neighbors, float radius)
    {
        
        return Vector3.zero;
    }
    private Vector3 Alignment(GameObject[] neighbors)
    {
        
        return Vector3.zero;
    }

    private Vector3 Wander()
    {
            //randomly adjust angle +/- displacement
            angle += Random.Range(-data.displacement, data.displacement);
            //create rotation quaternion around y axis (up)
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 point = rotation * (Vector3.forward * data.radius);
            // set point in front of agent
            Vector3 forward = movement.Direction * data.distance;
            //apply force towards point in front
            Vector3 force = GetSteeringForce(forward + point);
            return force;
    }



    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.data.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.data.maxForce);
        return force;
    }
}
