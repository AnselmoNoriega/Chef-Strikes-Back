using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Variables")]
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _zoomSpeed;
    private Vector3 _targetPosition;
    private float _camHeight;
    private float _camWidth;

    [Space]
    [SerializeField] private PolygonCollider2D _boundaryPolygon;

    private Transform _playerTransform;
    private PlayerVariables _playerVariables;

    private InputControls _inputManager;
    private InputAction _zoom;
    [SerializeField] private List<GameObject> _narrativePos;
    [SerializeField] private bool _followPlayer = true;

    public void Initialize()
    {
        _inputManager = new InputControls();
        _zoom = _inputManager.Player.Zoom;
        _zoom.performed += Zoom;

        _camHeight = Camera.main.orthographicSize;
        _camWidth = Camera.main.aspect * _camHeight;

        var player = ServiceLocator.Get<Player>();
        _playerTransform = player.transform;
        _playerVariables = player.GetComponent<PlayerVariables>();
    }

    private void OnEnable()
    {
        _zoom.Enable();
    }

    private void OnDisable()
    {
        _zoom.Disable();
    }

    private void OnDestroy()
    {
        _zoom.performed -= Zoom;
    }

    private void Zoom(InputAction.CallbackContext input)
    {
        Vector2 dz = _zoomSpeed * Time.deltaTime * (-_zoom.ReadValue<Vector2>());
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + dz.y, 2.3f, 3.5f);

        _camHeight = Camera.main.orthographicSize;
        _camWidth = Camera.main.aspect * _camHeight;
    }

    void Update()
    {
        _targetPosition = (Vector2)_playerTransform.position + _playerVariables.ThrowDirection;
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, _boundaryPolygon.bounds.min.x + _camWidth, _boundaryPolygon.bounds.max.x - _camWidth);
        _targetPosition.y = Mathf.Clamp(_targetPosition.y, _boundaryPolygon.bounds.min.y + _camHeight, _boundaryPolygon.bounds.max.y - _camHeight);
        _targetPosition.z = -1.0f;
        transform.position = Vector3.Lerp(transform.position, _targetPosition, _followSpeed);
    }
}
