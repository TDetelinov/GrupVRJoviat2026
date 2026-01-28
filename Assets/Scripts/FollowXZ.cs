using UnityEngine;

public class FollowXZ : MonoBehaviour
{
    public Transform target;
    private float fixedY;

    void Start()
    {
        fixedY = transform.position.y;
    }

    void Update()
    {
        if (target == null) return;

        transform.position = new Vector3(
            target.position.x,
            fixedY,
            target.position.z
        );
    }
}