using UnityEngine;

public class FishMovement : MonoBehaviour, IMover
{
    static float seaLevel;

    [Header("Movement")]
    public float initialSwimspeed;
    public float growSwimspeedFactor;
    public float currentSwimspeed;

    public float initialRotationSpeed;
    public float growRotationSpeedFactor;
    public float currentRotationSpeed;

    [SerializeField]
    private float endurance;
    [SerializeField]
    private float maxEndurance;
    float sprintTimer;
    [SerializeField]
    float sprintCooldown;

    bool dashed;

    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody2D;

    private void Awake()
    {
        seaLevel = 15f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        Invoke("CheckHightForEmote", 3f);
    }

    void Update()
    {
        RegenerateEndurance();
        TryGetComponent(out PlayerInput playerInput);
        if (playerInput != null)
        {
            AvatarFrameManager.instance.UpdateEnduranceBar(endurance / maxEndurance);
        }
    }

    void CheckHightForEmote()
    {
        if (TryGetComponent(out PlayerInput playerInput))
        {
            if (transform.position.y >= 8f)
            {
                EmoteManager.instance.SetTempratureStatus(2);
            }
            else if (transform.position.y <= -23f)
            {
                EmoteManager.instance.SetTempratureStatus(1);
            }
            else
            {
                EmoteManager.instance.SetTempratureStatus(0);
            }
        }
        Invoke("CheckHightForEmote", 3f);
    }

    public void MoveToPosition(Vector2 inputDirection)
    {
        if (transform.position.y < seaLevel)
        {
            Swim(inputDirection);
        }
        else
        {
            endurance = 0;
            FlipSprite();
            Rotate(inputDirection);
        }
    }

    void Swim(Vector2 inputDirection)
    {
        FlipSprite();
        Rotate(inputDirection);
        rigidbody2D.AddForce(new Vector2(transform.right.x * Time.deltaTime * currentSwimspeed, transform.right.y * Time.deltaTime * currentSwimspeed));
    }

    void FlipSprite()
    {
        if (transform.right.x > 0)
        {
            spriteRenderer.flipY = false;
        }
        else
        {
            spriteRenderer.flipY = true;
        }
    }

    void Rotate(Vector2 inputDirection)
    {
        if (inputDirection.x - 0.01 > transform.right.x || inputDirection.x + 0.01 < transform.right.x || inputDirection.y - 0.01 > transform.right.y || transform.right.y > inputDirection.y + 0.01)
        {
            float direction = 1;
            float angle = Vector2.SignedAngle(inputDirection, transform.right);

            if (angle > 0)
            {
                direction = -1;
            }
            transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime * direction);
        }
    }

    public void Sprint()
    {
        if (transform.position.y < seaLevel)
        {
            sprintTimer = 0;
            if (endurance > 0)
            {
                rigidbody2D.AddForce(new Vector2(transform.right.x * Time.deltaTime * currentSwimspeed, transform.right.y * Time.deltaTime * currentSwimspeed));
                endurance -= Time.deltaTime;
                if (TryGetComponent(out FishAudio fishAudio))
                {
                    fishAudio.PlaySprintAudio();
                }
            }
        }
    }

    void RegenerateEndurance()
    {
        if (sprintTimer < sprintCooldown)
        {
            sprintTimer += Time.deltaTime;
        }
        else if (endurance <= maxEndurance)
        {
            endurance += Time.deltaTime;
        }
    }

    public void Dash()
    {
        if (!dashed)
        {
            Debug.Log("Dash");
            dashed = true;
            rigidbody2D.AddForce(new Vector2(transform.right.x * Time.deltaTime * currentSwimspeed * 100, transform.right.y * Time.deltaTime * currentSwimspeed * 100));
            Invoke("ResetDashed", 3f);
        }
    }

    void ResetDashed()
    {
        dashed = false;
    }
}
