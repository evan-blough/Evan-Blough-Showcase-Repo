using UnityEngine;

public class BasicMovement : NPCMovement
{
    Vector2 currentMovement;

    private void Start()
    {
        mainCamera = Camera.main;
        originalPoint = transform.position;
    }

    public override Vector2 GetMovementVector(Vector3 currentPos)
    {
        if (!isMoving)
        {
            if (Random.Range(0, 10) <= movementFrequency && canMove)
            {
                if (Vector3.Distance(originalPoint, currentPos) > movementRadius)
                {
                    var moveBackToOrigin = CameraHandler.AutoMoveRelativeToCamera(originalPoint, currentPos);
                    StartCoroutine(MovementTimer());
                    currentMovement = new Vector2(moveBackToOrigin.x + Random.Range(-.15f, .15f), moveBackToOrigin.y + Random.Range(-.15f, .15f));
                }
                else
                {
                    StartCoroutine(MovementTimer());
                    currentMovement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                }
            }
            else
            {
                isMoving = false;
                currentMovement = Vector2.zero;
            }
        }

        return currentMovement;
    }
}
