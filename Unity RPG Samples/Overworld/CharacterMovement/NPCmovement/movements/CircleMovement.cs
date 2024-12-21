using UnityEngine;

public class CircleMovement : NPCMovement
{
    float angle = 0f;
    float speed { get { return movementRadius / 2f; } }
    public Transform whatever;

    private void Start()
    {
        originalPoint = new Vector3(transform.position.x - movementRadius, transform.position.y, transform.position.z);
    }

    public override Vector2 GetMovementVector(Vector3 currentPos)
    {
        var xVal = originalPoint.x + Mathf.Cos(angle) * movementRadius;
        var zVal = originalPoint.z + Mathf.Sin(angle) * movementRadius;

        // never manually code a circular path again... hacky solution to maintain a distance equal to the 
        // radius from the fixed point

        angle = angle + Time.deltaTime * ((speed * (speed / ((speed + movementRadius) / 2f))) / movementRadius);

        Vector3 target = new Vector3(xVal, 0f, zVal);
        Vector3 toMove = (target - currentPos).normalized;
        if (angle >= 360f)
            angle = 0f;

        return new Vector2(toMove.x, toMove.z);
    }
}
