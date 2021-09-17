using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("General Setup Settings")]
    [Tooltip("Keyboard controls for ship movement")] [SerializeField] InputAction movement;
    [Tooltip("Keyboard controls for firing lasers")] [SerializeField] InputAction fire;
    [Tooltip("Ships lasers")] [SerializeField] GameObject[] lasers;
    [Header("Ship position")]
    [Tooltip("Left boundary for ship movement")] [SerializeField] float xMinimumPosition = -5;
    [Tooltip("Right boundary for ship movement")] [SerializeField] float xMaximumPosition = 5;
    [Tooltip("Top of screen boundary for ship movement")] [SerializeField] float yMinimumPosition = -5;
    [Tooltip("Bottom of screen boundary for ship movement")] [SerializeField] float yMaximumPosition = 5;
    [Header("Ship speed")]
    [Tooltip("How fast the ship moves left and right on the screen")] [SerializeField] float xSpeed = 10;
    [Tooltip("How fast the ship moves up and down on the screen")] [SerializeField] float ySpeed = 10;
    [Tooltip("How violently the ship pitches up and down")] [SerializeField] float throwPitchFactor = 10f;
    [Tooltip("How violently the ship rolls left and right")] [SerializeField] float throwRollFactor = 10f;

    void Start()
    {
        transform.localPosition = new Vector3(0, -7, 14);
    }

    private void OnEnable()
    {
        movement.Enable();
        fire.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        fire.Enable();
    }

    void Update()
    {
        float xThrow = movement.ReadValue<Vector2>().x;
        float yThrow = movement.ReadValue<Vector2>().y;
        ProcessMovement(xThrow, yThrow);
        ProcessShipOrientation(xThrow, yThrow);
        ProcessFiring();
    }

    private void ProcessFiring()
    {
        var fireValue = fire.ReadValue<float>();
        if (fireValue > .5f)
        {
            ToggleLasers(true);
        }
        else
        {
            ToggleLasers(false);
        }
    }

    private void ToggleLasers(bool enabled)
    {
        foreach(GameObject laser in lasers)
        {
            var emission = laser.GetComponent<ParticleSystem>().emission;
            emission.enabled = enabled;
        }
    }

    private void ProcessShipOrientation(float xThrow, float yThrow)
    {
        var rollRight = new Quaternion(-0.619994879f, 0.340009391f, -0.340009391f, 0.619994879f);
        var rollLeft = new Quaternion(-0.650105536f, -0.278141707f, 0.278141707f, 0.650105536f);
        var rollZero = new Quaternion(-0.641359985f, 0.00192682236f, -0.00109485397f, 0.767236948f);
        Quaternion roll;

        if (xThrow > 0)
        {
            roll = rollRight;
        }
        else if (xThrow < 0)
        {
            roll = rollLeft;
        }
        else
        {
            roll = rollZero;
        }

        var pitchUp = new Quaternion(-0.830135643f, 0, 0, 0.557561517f);
        var pitchDown = new Quaternion(-0.464707583f, 0, 0, 0.885464191f);
        var pitchZero = rollZero;
        Quaternion pitch;

        if (yThrow > Mathf.Epsilon)
        {
            pitch = pitchUp;
        }
        else if (yThrow < -Mathf.Epsilon)
        {
            pitch = pitchDown;
        }
        else
        {
            pitch = pitchZero;
        }

        //transform.localRotation = Quaternion.Slerp(transform.localRotation, roll, throwRollFactor * Time.deltaTime);
        //transform.localRotation = Quaternion.Slerp(transform.localRotation, pitch, throwPitchFactor * Time.deltaTime);
    }

    private void ProcessMovement(float xThrow, float yThrow)
    {
        Debug.Log($"MOVE: xThrow: {xThrow} | yThrow: {yThrow}");
        float yOffset = yThrow * ySpeed * Time.deltaTime;
        float yTarget = transform.localPosition.y + yOffset;

        float xOffset = xThrow * xSpeed * Time.deltaTime;
        float xTarget = transform.localPosition.x + xOffset;

        float xClamped = Mathf.Clamp(xTarget, xMinimumPosition, xMaximumPosition);
        float yClamped = Mathf.Clamp(yTarget, yMinimumPosition, yMaximumPosition);

        transform.localPosition = new Vector3(
            xClamped,
            yClamped,
            transform.localPosition.z
        );
    }
}
