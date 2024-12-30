using System.Collections;
using UnityEngine;

public abstract class NPCMovement : MonoBehaviour
{
    public Vector3 originalPoint;
    public float movementRadius;
    public int movementFrequency;
    public float movementPause = 1.5f;
    public float movementTime = .5f;
    public bool canMove;
    public bool isMoving;
    protected Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    public abstract Vector2 GetMovementVector(Vector3 currentPos);
    public virtual Vector3 ApplyMovementVector(Vector3 direction)
    {
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        return direction;
    }

    public virtual IEnumerator MovementTimer()
    {
        isMoving = true;
        yield return new WaitForSeconds(movementTime);

        canMove = false;
        isMoving = false;

        yield return new WaitForSeconds(movementPause);
        canMove = true;
    }
}

