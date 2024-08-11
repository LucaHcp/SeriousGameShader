using UnityEngine;

public class FishAIInput : MonoBehaviour
{
    [SerializeField]
    Vector2 targetLocation;
    [SerializeField]
    Vector2 targetDirection;
    IMover mover;

    private void Awake()
    {
        mover = GetComponent<IMover>();
    }

    void Start()
    {
        GetTargetLocation();
        InvokeRepeating("CheckIfTargetReached", 2f, 1f);
        InvokeRepeating("GetTargetDirection", 0f, 1f);
    }

    void Update()
    {
        mover.MoveToPosition(targetDirection);
    }

    void CheckIfTargetReached()
    {
        Vector2 dir = new( targetLocation.x - transform.position.x, targetLocation.y - transform.position.y);
        if (dir.magnitude <= 1f)
        {
            GetTargetLocation();
        }
    }

    void GetTargetLocation()
    {
        targetLocation = new Vector2(Random.Range(-33f, 33f), Random.Range(-32f, 14f));
    }

    void GetTargetDirection()
    {
        targetDirection = (targetLocation - new Vector2(transform.position.x, transform.position.y)).normalized;
    }
}
