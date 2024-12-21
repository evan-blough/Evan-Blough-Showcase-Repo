using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StraightPathMovement : NPCMovement
{
    public List<Vector3> points = new List<Vector3>();
    public Vector3 targetPoint;
    public bool hasDelay = false;
    private void Start()
    {
        originalPoint = transform.position;
        targetPoint = points.First();
    }

    public override Vector2 GetMovementVector(Vector3 currentPos)
    {
        if (Vector3.Distance(currentPos, targetPoint) < .5f)
        {
            targetPoint = points[(points.IndexOf(targetPoint) + 1) % points.Count];
            if (hasDelay)
            {
                StartCoroutine(MovementTimer());
            }
        }
        if (canMove)
        {
            Vector3 target = Camera.main.transform.InverseTransformPoint(targetPoint);
            currentPos = Camera.main.transform.InverseTransformPoint(currentPos);
            Vector3 newMove = (target - currentPos).normalized;

            return new Vector2(newMove.x, newMove.z);
        }

        return Vector2.zero;
    }

    public override IEnumerator MovementTimer()
    {
        canMove = false;
        yield return new WaitForSeconds(movementPause);
        canMove = true;
    }
}
