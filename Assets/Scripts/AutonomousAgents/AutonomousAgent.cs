using Unity.VisualScripting;
using UnityEngine;

public class AutonomousAgent : AIAgent
{
    [SerializeField] AutonomousAgentData data;

    [Header("Perception")]
    public Perception seekPerception;
    public Perception fleePerception;    
    public Perception flockPerception;
    public Perception obstaclePerception;

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

        //FLOCK
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

        //OBSTACLE
        
        // if(obstaclePerception!=null)
        // {
        //     if(obstaclePerception.CheckDirection(Vector3.forward))
        //     {
        //         Debug.DrawRay(transform.position, transform.rotation * Vector3.forward * 3, Color.red, 0.5f);
        //     }
        //     else
        //     {
        //         Debug.DrawRay(transform.position, transform.rotation * Vector3.forward * 3, Color.green, 0.5f);
        //     }
        // }
        if (obstaclePerception != null && obstaclePerception.CheckDirection(Vector3.forward))
        {
            Vector3 direction = Vector3.zero;
            if (obstaclePerception.GetOpenDirection(ref direction))
            {
                Debug.DrawRay(transform.position, direction * 5, Color.red, 0.2f);
                movement.ApplyForce(GetSteeringForce(direction) * data.obstacleWeight);
            }
        }

        //WANDER
        if(movement.Acceleration.sqrMagnitude == 0)
        {
            Vector3 force = Wander();
            movement.ApplyForce(force);
        }

        Vector3 acceleration = movement.Acceleration;
        //acceleration.y = 0;
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
        Vector3 separation = Vector3.zero;
        //accumulate the separation vectors of the neighbors
        foreach(var neighbor in neighbors)
        {
            Vector3 direction = transform.position - neighbor.transform.position;
            float distance = Vector3.Magnitude(direction);
            if(distance<radius)
            {
                separation += direction / (distance * distance);
            }
        }
        return GetSteeringForce(separation);
    }
    private Vector3 Alignment(GameObject[] neighbors)
    {
        Vector3 velocities = Vector3.zero;
        foreach(var neighbor in neighbors)
        {
            velocities += neighbor.GetComponent<AIAgent>().movement.Velocity;
        }
        Vector3 averageVelocity = velocities / neighbors.Length;

        return GetSteeringForce(averageVelocity);
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
