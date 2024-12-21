using System.Collections;
using UnityEngine;

public class NonPlayerCharacterHandler : CharacterHandler
{
    protected bool isActive = false;
    public NPCMovement movement;

    private void Start()
    {
        cam = SceneManager.instance.cam.transform;
    }


    // Update is called once per frame
    void Update()
    {
        SetMovement();
        VerticalMovement();
        Move();
        AnimationStateCheck();
    }

    protected virtual IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(2f);
        isActive = true;
        movement.canMove = true;
    }

    public virtual void SetMovement()
    {
        if (isActive)
        {
            move = movement.GetMovementVector(transform.position);
        }
        else move = new Vector2(0f, 0f);

        ApplyMoveVector();
    }

    public override void Move()
    {
        Vector3 moveDir = movement.ApplyMovementVector(direction);
        animator.SetFloat("Speed", currentSpeed / 11);

        controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        runDirection = directionMultiplier * Vector3.Angle(moveDir, Camera.main.transform.forward);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>())
        {

        }
    }

    protected void OnEnable()
    {
        StartCoroutine(WaitToStart());
    }

    protected void OnDisable()
    {
        isActive = false;
        move = Vector2.zero;
    }
}
