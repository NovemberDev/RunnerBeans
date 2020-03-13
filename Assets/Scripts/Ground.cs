using UnityEngine;

public enum GroundType
{
    Forward,
    Left,
    Right
}

public class Ground : MonoBehaviour
{
    public Transform NextGround;

    public GroundType GroundType;

    public GameObject obstacleLeft, obstacleRight, obstacleMiddle, pickupSlot, bean;

    public GameObject[] LeftObstacles, RightObstacles, MiddleObstacles;

    public GameObject[] Static;

    public bool HasHole = false;

    void Start()
    {
        if(GroundType == GroundType.Left)
        {
            Instantiate(Static[Random.Range(0, Static.Length)], transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -90, 0)), transform);
            Instantiate(Static[Random.Range(0, Static.Length)], transform);
        }

        if (GroundType == GroundType.Right)
        {
            Instantiate(Static[Random.Range(0, Static.Length)], transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 180, 0)), transform);
            Instantiate(Static[Random.Range(0, Static.Length)], transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -90, 0)), transform);
        }

        if (GroundType != GroundType.Forward)
        {
            return;
        }

        foreach(Transform t in pickupSlot.transform)
        {
            if(Random.Range(0, 100) <= 15.0f)
            {
                Instantiate(bean, t.position + new Vector3(0, 0.75f, 0), t.rotation, transform);
            }
        }

        Instantiate(Static[Random.Range(0, Static.Length)], transform);
        Instantiate(Static[Random.Range(0, Static.Length)], transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 180, 0)), transform);

        if(HasHole)
        {
            return;
        }

        if (Random.Range(0, 100) >= 20)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    SpawnObstacle(LeftObstacles, obstacleLeft);
                    break;
                case 1:
                    SpawnObstacle(MiddleObstacles, obstacleMiddle);
                    break;
                case 2:
                    SpawnObstacle(RightObstacles, obstacleRight);
                    break;
            }
        }
    }

    void SpawnObstacle(GameObject[] prefabs, GameObject target)
    {
        Instantiate(prefabs[Random.Range(0, prefabs.Length)], target.transform.GetChild(Random.Range(0, target.transform.childCount)).position, transform.rotation, transform);
    }

    public void DestroyGround()
    {
        Destroy(gameObject, 1.0f);
    }

    void OnDrawGizmos()
    {
        /*if(GroundType != GroundType.Forward)
        {
            return;
        }

        Gizmos.color = Color.yellow;

        foreach(Transform g in obstacleLeft.transform)
        {
            Gizmos.DrawSphere(g.transform.position, 0.5f);
        }

        foreach (Transform g in obstacleRight.transform)
        {
            Gizmos.DrawSphere(g.transform.position, 0.5f);
        }

        foreach (Transform g in obstacleMiddle.transform)
        {
            Gizmos.DrawSphere(g.transform.position, 0.5f);
        }

        Gizmos.color = Color.cyan;

        foreach (Transform g in pickupSlot.transform)
        {
            Gizmos.DrawSphere(g.transform.position, 0.5f);
        }*/
    }
}