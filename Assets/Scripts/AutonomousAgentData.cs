using UnityEngine;

[CreateAssetMenu(fileName = "AutonomousAgentData", menuName = "Data/AutonomousAgentData")]
public class AutonomousAgentData : ScriptableObject
{
    [Header("Wander")]
    [Range(0,180)] public float displacement;
    [Range(0,10)] public float distance;
    [Range(0,10)] public float radius;

    [Header("Flock")]
    [Range(0,5)] public float CohesionWeight;
    [Range(0,5)] public float SeparationRadius;
    [Range(0,5)] public float SeparationWeight;
    [Range(0,5)] public float AlignmentWeight;
}
 