using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class CameraControl : MonoBehaviour
{
    private float CameraDistance;
    private float _cameraZoom = 0;
    private Vector2 _directionChange;
    public CinemachineVirtualCamera VirtualCamera;
    private Cinemachine3rdPersonFollow _followCam;
    void Start()
    {
        _followCam = VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        CameraDistance = _followCam.CameraDistance;
    }
    void Update()
    {
        // rotate camera based on _directionChange
        if (_directionChange.x != 0 || _directionChange.y != 0)
        {
            transform.RotateAround(transform.position, Vector3.up, -_directionChange.x * Time.deltaTime * 100f);
            transform.RotateAround(transform.position, transform.right, _directionChange.y * Time.deltaTime * 100f);
        }

        if(_cameraZoom != 0)
        {
            CameraDistance = Mathf.Clamp(CameraDistance + _cameraZoom, 2f, 20f);
            _followCam.CameraDistance = CameraDistance;
        }
    }

    public void RotateCamera(InputAction.CallbackContext context)
    {
        if (context.action.name == "RotateCamera")
        {
            if (context.action.phase == InputActionPhase.Started)
            {
                // Debug.Log("Started");
            }
            else if (context.action.phase == InputActionPhase.Performed)
            {
                if (context.action.activeControl.device.name == "Mouse")
                {
                    _directionChange = -context.action.ReadValue<Vector2>() * 0.2f;
                }
                else
                {
                    _directionChange = -context.action.ReadValue<Vector2>();
                }
                // Debug.Log("Direction change: " + directionChange);
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                _directionChange = new Vector2(0, 0);
            }
        }
    }

    public void ZoomCamera(InputAction.CallbackContext context)
    {
        if (context.action.name == "ZoomCamera")
        {
            if (context.action.phase == InputActionPhase.Performed)
            {
                // Debug.Log("Zoom: " + context.action.ReadValue<float>());
                if (context.action.activeControl.device.name == "Mouse")
                {
                    CameraDistance += context.action.ReadValue<float>() < 0 ? 3 : -3;
                }
                else
                {
                    _cameraZoom = context.action.ReadValue<float>();
                }
                // Debug.Log("Zoom: " + cameraZoom);
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                _cameraZoom = 0;
            }
        }
    }
}
