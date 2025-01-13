using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { INITALIZING, MOVING, CHASING, RETURNING }
public class EnemyHandler : NonPlayerCharacterHandler
{
    bool isInBattle;
    public EnemyState state;
    public List<GameObject> encounterFormation;
    Vector3 preChasePoint;
    public PlayerCharacterHandler chasingCharacter;
    private void Start()
    {
        cam = SceneManager.instance.cam.transform;
        mainCamera = Camera.main;
        state = EnemyState.INITALIZING;
        StartCoroutine(WaitToStart(1.5f));
    }

    private void Update()
    {
        SetMovement();
        VerticalMovement();
        Move();
        AnimationStateCheck();
    }
    public override void SetMovement()
    {
        if (isActive)
        {
            if (Vector3.Distance(transform.position, preChasePoint) < .5f && state == EnemyState.RETURNING)
            {
                //Debug.Log(Vector3.Distance(transform.position, preChasePoint));
                state = EnemyState.MOVING;
            }

            switch (state)
            {
                case EnemyState.MOVING:
                    move = movement.GetMovementVector(transform.position);
                    break;
                case EnemyState.RETURNING:
                    move = CameraHandler.AutoMoveRelativeToCamera(preChasePoint, transform.position);
                    break;
                case EnemyState.CHASING:
                    move = CameraHandler.AutoMoveRelativeToCamera(chasingCharacter.transform.position, transform.position);
                    break;
                default:
                    move = Vector2.zero;
                    break;
            }
        }
        else move = new Vector2(0f, 0f);

        ApplyMoveVector();
    }

    public override void Move()
    {
        Vector3 moveDir = movement.ApplyMovementVector(direction);
        animator.SetFloat("Speed", currentSpeed / 11);

        float speed = state == EnemyState.CHASING ? currentSpeed * 1.5f : currentSpeed;

        controller.Move(moveDir.normalized * speed * Time.deltaTime);
    }

    protected override IEnumerator WaitToStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        isActive = true;
        movement.canMove = true;
        state = EnemyState.MOVING;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        var whatever = other.gameObject.GetComponent<PlayerCharacterHandler>();
        if (whatever != null && isActive)
        {
            if (chasingCharacter == null)
                chasingCharacter = whatever;

            if (state == EnemyState.MOVING)
                preChasePoint = transform.position;

            state = EnemyState.CHASING;
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        var whatever = other.gameObject.GetComponent<PlayerCharacterHandler>();
        if (chasingCharacter == null && whatever != null && isActive)
            chasingCharacter = whatever;

        if (whatever != null && isActive)
        {
            if (Vector3.Distance(whatever.transform.position, transform.position) >= 1.5f)
            {
                state = EnemyState.CHASING;
            }
            else
            {
                isActive = false;
                SceneManager.instance.StartCoroutine(SceneManager.instance.StartBattle(this));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (chasingCharacter == other.gameObject.GetComponent<PlayerCharacterHandler>() && chasingCharacter != null)
        {
            chasingCharacter = null;
            state = EnemyState.RETURNING;
        }
    }
}
