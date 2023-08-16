using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] Transform playerPos;
    [SerializeField] Transform lightPos;
    public float followSpeed = 0.05f;

    [Header("Camera Zoom")]
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
    }

    private void OnDisable()
    {
        zoom.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // Zoom functionality
        Vector2 dz = zoomSpeed * Time.deltaTime * -zoom.ReadValue<Vector2>();
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + dz.y, 2.3f, 3.5f);

        // Camera movement functionality
        Vector3 targetPosition = new Vector3(playerPos.position.x, playerPos.position.y, lightPos.position.z - 1.0f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
    }
}
