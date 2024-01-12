using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] Transform lightPos;

    [SerializeField]private float followSpeed = 0.05f;
    [SerializeField]private float zoomSpeed = 1500.0f;

    private InputControls inputManager;
    private InputAction zoom;
    public PolygonCollider2D boundaryPolygon; 

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
        var camOffset = ServiceLocator.Get<Player>().ThrowLookingDir;
        Vector2 dz = zoomSpeed * Time.deltaTime * -zoom.ReadValue<Vector2>();
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + dz.y, 2.3f, 3.5f);

        Vector3 targetPosition = new Vector3(playerPos.position.x + camOffset.x, playerPos.position.y + camOffset.y, lightPos.position.z - 1.0f);

        
        float halfCamHeight = Camera.main.orthographicSize;
        float halfCamWidth = Camera.main.aspect * halfCamHeight;

        targetPosition.x = Mathf.Clamp(targetPosition.x, boundaryPolygon.bounds.min.x + halfCamWidth, boundaryPolygon.bounds.max.x - halfCamWidth);
        targetPosition.y = Mathf.Clamp(targetPosition.y, boundaryPolygon.bounds.min.y + halfCamHeight, boundaryPolygon.bounds.max.y - halfCamHeight);

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
    }
}
