using UnityEngine;

public abstract class CharacterHandler : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    protected Camera mainCamera;
    public SpriteRenderer sprite;
    public Animator animator;
    protected float speed = 10f;
    protected float currentSpeed = 3.5f;
    public float animDirection;
    protected float gravity = -9.81f;
    public Vector3 direction;
    public Vector2 move;
    protected int directionMultiplier = 1;
    protected Vector3 velocity;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask groundMask;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    public virtual void Move()
    {
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            animator.SetFloat("Speed", currentSpeed / 11);

            var futurePosition = transform.position + (moveDir.normalized * currentSpeed * Time.deltaTime);

            if (!Physics.Raycast(futurePosition, Vector3.down, groundDistance, groundMask) && velocity.y < .1f && isGrounded)
            {
                moveDir = moveDir + Vector3.down;
            }

            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
    }

    public virtual void AnimationStateCheck()
    {
        var angleDif = CameraHandler.AngleNormalization(sprite.transform.eulerAngles.y);

        animDirection = GetDirection(transform.eulerAngles.y - angleDif);
        animator.SetFloat("Direction", animDirection);

        if (animDirection <= 7 && animDirection >= 5)
            sprite.flipX = true;
        else
            sprite.flipX = false;
    }

    public virtual void VerticalMovement()
    {
        //jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void ApplyMoveVector()
    {
        sprite.gameObject.transform.rotation = mainCamera.transform.rotation;

        // make movement relative to the camera
        var forward = cam.transform.forward;
        forward.y = 0f;
        forward.Normalize();
        var right = cam.transform.right;
        right.y = 0f;
        right.Normalize();

        direction = forward * move.y + right * move.x;
        directionMultiplier = move.x < 0 ? -1 : move.x == 0 ? directionMultiplier : 1;
    }

    public float GetDirection(float angle)
    {
        // keep angle between the bounds
        angle = CameraHandler.AngleNormalization(angle);

        // north
        if ((angle >= -22.5f && angle <= 0f) || (angle >= 0f && angle < 22.5f)) return 0f;
        //northeast
        else if (angle >= 22.5f && angle < 67.5f) return 1f;
        //east
        else if (angle >= 67.5f && angle < 112.5f) return 2f;
        //southeast
        else if (angle >= 112.5f && angle < 157.5f) return 3f;
        //southwest
        else if (angle >= -157.5f && angle < -112.5f) return 5f;
        //west
        else if (angle >= -112.5f && angle < -67.5f) return 6f;
        //northwest
        else if (angle >= -67.5f && angle < -22.5f) return 7f;
        //south
        else return 4f;
    }

}
