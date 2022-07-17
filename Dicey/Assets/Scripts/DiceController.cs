using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
public class DiceController : MonoBehaviour
{
    private Vector2 _movement;
    [HideInInspector]
    public Vector2 CurrentInput;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        _movement = new Vector2(0, 0);
        rb = GetComponent<Rigidbody>();
        CurrentInput = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (IsMoving())
        {
            rb.AddForce(GetMovementVector());
        }
    }

    private bool IsMoving()
    {
        return _movement.x != 0 || _movement.y != 0;
    }
    private Vector3 GetMovementVector()
    {
        return new Vector3(_movement.x, 0, _movement.y) * 10;
    }
    public void MoveDice(InputAction.CallbackContext context)
    {
        if (context.action.name == "MoveDice")
        {
            if (context.action.phase == InputActionPhase.Started)
            {
                // Debug.Log("Started");
            }
            else if (context.action.phase == InputActionPhase.Performed)
            {
                updateMovement(context.action.ReadValue<Vector2>());
                CurrentInput = context.action.ReadValue<Vector2>();
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                _movement = new Vector2(0, 0);
                CurrentInput = new Vector2(0, 0);
            }
        }
    }

    public void updateMovement(Vector2 currentMovement)
    {
        // adjust Vector2 to match the camera angle
        Vector2 adjustedMovement = currentMovement;
        Vector2 viewDir = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
        // -1.5708f is the constant angle of the calculation
        float angle = Mathf.Atan2(viewDir.y, viewDir.x) - 1.5708f;
        adjustedMovement = new Vector2(
            adjustedMovement.x * Mathf.Cos(angle) - adjustedMovement.y * Mathf.Sin(angle),
            adjustedMovement.x * Mathf.Sin(angle) + adjustedMovement.y * Mathf.Cos(angle));
        _movement = adjustedMovement;
        Debug.Log("Movement: " + _movement + " currentMovement: " + currentMovement);
    }
    public float jumpStrength = 800;
    private bool _isJumping = true;
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.action.name == "Jump")
        {
            if (_isJumping)
            {

                if (context.action.phase == InputActionPhase.Performed)
                {
                    rb.AddForce(new Vector3(0, jumpStrength, 0));
                    rb.AddTorque(new Vector3(Random.Range(1f, 2f) * jumpStrength, Random.Range(1f, 2f) * jumpStrength, Random.Range(1f, 2f) * jumpStrength));

                }
                else if (context.action.phase == InputActionPhase.Canceled)
                {
                    // Debug.Log("Canceled");
                }
            }
        }
    }

    // gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // draw force arrow
        // Vector2 viewDir = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z).normalized;
        // float angle = Mathf.Atan2(viewDir.y, viewDir.x) *Mathf.Rad2Deg;//Vector2.Angle(new Vector2(0, 1), viewDir);
        // if(angle < 0){
        //     GUI.color = Color.red;
        //     angle = 360 + angle;
        // }
        // else{
        //     GUI.color = Color.green;
        // }
        // Handles.Label(transform.position, "Angle: " + angle);

        if (IsMoving())
        {
            Gizmos.DrawRay(transform.position, GetMovementVector());
        }
    }
}
