using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWayPoint;

    [Range(.5f, 10f)]
    public float width = 1f;
    [Tooltip("If true, will change Agent state to Idle")] public bool isIdleTrigger;

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2;
        Vector3 maxBound = transform.position - transform.right * width / 2;
        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
