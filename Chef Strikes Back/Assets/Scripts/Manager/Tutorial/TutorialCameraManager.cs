using UnityEngine;

public class TutorialCameraManager : MonoBehaviour
{
    [Header("Camera Variables")]
    private float _followSpeed = 1.0f;
    [SerializeField] private float _zoomSpeed;
    private Vector3 _targetPosition;
    private float _camHeight;
    private float _camWidth;
    [SerializeField] private float _followTargetSpeed;
    [SerializeField] private float _followThrowSpeed;

    [Space]
    [SerializeField] private PolygonCollider2D _boundaryPolygon;
    private Transform _playerTransform;
    private Transform _targetTransform;
    private PlayerVariables _playerVariables;

    public void Initialize()
    {
        _camHeight = Camera.main.orthographicSize;
        _camWidth = Camera.main.aspect * _camHeight;
        var player = ServiceLocator.Get<Player>();
        _playerTransform = player.transform;
        _playerVariables = player.GetComponent<PlayerVariables>();
        _targetTransform = _playerTransform;
        ZoomIn();
    }
    void Update()
    {
        MoveToThrowDir();
        StayInsideBound();
        MoveCameraToPosition();
    }

    public void ChangeTarget(Transform target)
    {
        _targetTransform = target;
        if (_targetTransform == _playerTransform)
        {
            _followSpeed = _followThrowSpeed;
        }
        else
        {
            _followSpeed = _followTargetSpeed;
        }
    }

    private void MoveCameraToPosition()
    {
        Camera.main.transform.position = Vector3.Lerp
            (Camera.main.transform.position,
            new Vector3(_targetPosition.x,
            _targetPosition.y, Camera.main.transform.position.z), _followSpeed * Time.deltaTime);
    }

    private void MoveToThrowDir()
    {
        if (_targetTransform != _playerTransform)
        {
            _targetPosition = _targetTransform.position;
            return;
        }
        _targetPosition = (Vector2)_playerTransform.position + _playerVariables.ThrowDirection;
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, _boundaryPolygon.bounds.min.x + _camWidth, _boundaryPolygon.bounds.max.x - _camWidth);
        _targetPosition.y = Mathf.Clamp(_targetPosition.y, _boundaryPolygon.bounds.min.y + _camHeight, _boundaryPolygon.bounds.max.y - _camHeight);
        _targetPosition.z = -1.0f;
        transform.position = Vector3.Lerp(transform.position, _targetPosition, 0.0f);
    }

    private void StayInsideBound()
    {
        _targetPosition.x = Mathf.Clamp
            (_targetPosition.x, _boundaryPolygon.bounds.min.x + (_camWidth / 2), _boundaryPolygon.bounds.max.x - (_camWidth / 2));
        _targetPosition.y = Mathf.Clamp
            (_targetPosition.y, _boundaryPolygon.bounds.min.y + (_camHeight / 2), _boundaryPolygon.bounds.max.y - (_camHeight / 2));
        _targetPosition.z = -1.0f;
    }

    public void ZoomIn(float x = -5.0f, float y = -5.0f)
    {
        Vector2 dz = _zoomSpeed * Time.deltaTime * new Vector2(x, y);
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + dz.y, 2.3f, 3.5f);

        _camHeight = Camera.main.orthographicSize;
        _camWidth = Camera.main.aspect * _camHeight;
    }
}
