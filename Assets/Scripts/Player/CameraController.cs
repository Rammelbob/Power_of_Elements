using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    PlayerInput playerInput;
    public Transform direction;
    Vector2 lookSpeed = new Vector2(200, 170f);
    Vector2 lookMovment;
    float mouseX, mouseY;

    private void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.CameraControls.MoveCamera.started += OnInput;
        playerInput.CameraControls.MoveCamera.canceled += OnInput;
        //playerInput.CameraControls.MoveCamera.performed += OnInput;
    }
    void OnInput(InputAction.CallbackContext context)
    {
        lookMovment = context.ReadValue<Vector2>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        CamControl();
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        //}
    }

    void CamControl()
    {
        mouseX += lookMovment.x * Time.deltaTime * lookSpeed.x;
        mouseY -= lookMovment.y * Time.deltaTime * lookSpeed.y;

        mouseY = Mathf.Clamp(mouseY, -55, 75);

        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        direction.rotation = Quaternion.Euler(0, mouseX, 0);
    }

    private void OnEnable()
    {
        playerInput.CameraControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CameraControls.Disable();
    }
}
