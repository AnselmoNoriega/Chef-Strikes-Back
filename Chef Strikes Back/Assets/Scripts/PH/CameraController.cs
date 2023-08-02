using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] Transform lightPos;
    public float zoomSpeed = 1500.0f;
    private InputControls inputManager;
    private InputAction zoom;

    private void Awake()
    {
        inputManager = new InputControls();
    }

    private void OnEnable()
    {
        zoom = inputManager.Player.Zoom;
        zoom.Enable();
        zoom.performed += zoomIn;
    }

    private void OnDisable()
    {
        zoom.performed -= zoomIn;
        zoom.Disable();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 dz = zoomSpeed * Time.deltaTime * zoom.ReadValue<Vector2>();
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + dz.y, 1.0f, 1.9f);
        Camera.main.transform.position = new Vector3(playerPos.position.x, playerPos.position.y, lightPos.position.z - 1.0f);
    }

    private void zoomIn(InputAction.CallbackContext input)
    {
    }
}
