using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    PlayerControls controls;
    public CinemachineFreeLook cam;
    public float targetRotation;
    public float currentRotation;
    public float rotationSpeed = 5f;
    public Vector2 stickRotation;
    public bool isKeyboard = false;
    void Awake()
    {
        SceneManager.instance.cam = this;
        controls = ControlsHandler.instance.playerControls;
        cam.m_XAxis.Value = 0f;
        cam.m_YAxis.Value = .5f;

        controls.overworld.CameraRotation.performed += ctx => RotateCamera(ctx.ReadValue<float>());

        controls.overworld.ManualCamera.performed += ctx => stickRotation = ctx.ReadValue<Vector2>();
        controls.overworld.ManualCamera.canceled += ctx => stickRotation = Vector2.zero;
    }

    public void RotateCamera(float rotateDirection)
    {
        isKeyboard = true;
        targetRotation += 45 * (rotateDirection == 0 ? 0 : (rotateDirection < 0 ? -1 : 1));
        if (targetRotation > 180)
        {
            targetRotation = -135;
        }
        else if (targetRotation < -180)
        {
            targetRotation = 135;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stickRotation.magnitude >= .1f)
        {
            isKeyboard = false;
            cam.m_XAxis.Value += stickRotation.x * Time.deltaTime * 90;
            cam.m_YAxis.Value += stickRotation.y * Time.deltaTime;
        }

        if (Mathf.Abs(currentRotation - targetRotation) > 0.1f && isKeyboard)
        {
            currentRotation = Mathf.LerpAngle(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

            currentRotation = AngleNormalization(currentRotation);

            cam.m_XAxis.Value = currentRotation;
        }

    }

    // want to make PlayerCharacterHandler also use this... logic in that gets a little funky, though. Might be a refactor day for that.
    public static Vector2 AutoMoveRelativeToCamera(Vector3 positionToMoveTowards, Vector3 currentPosition)
    {
        positionToMoveTowards = Camera.main.transform.InverseTransformPoint(positionToMoveTowards);
        currentPosition = Camera.main.transform.InverseTransformPoint(currentPosition);

        Vector3 moveTowards = (positionToMoveTowards - currentPosition).normalized;

        return new Vector2(moveTowards.x, moveTowards.z);
    }

    // normalizes the angle to a scale of -180 => 180 degrees due to that being the range of transform.eulerAngles
    public static float AngleNormalization(float angle)
    {
        float newAngle = angle;

        // fixes some angle normalization issues where values were @ or slightly below 360 degrees... keeps it in range of the 45 degrees of the animation direction function
        if (newAngle > 350) newAngle -= 360;

        if (newAngle > 180) newAngle = (newAngle % 180) + (-180);
        else if (newAngle < -180) newAngle = 180 - (newAngle % 180);

        return newAngle;
    }
}
