using UnityEngine;

public class TutorialCameraManager : MonoBehaviour
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
    private Transform _targetTransform;
    private Transform _movingTargetTransform;
    private PlayerVariables _playerVariables;

    public void Initialize()
    {
        _camHeight = Camera.main.orthographicSize;
        _camWidth = Camera.main.aspect * _camHeight;

        var player = ServiceLocator.Get<Player>();
        _playerTransform = player.transform;
        _playerVariables = player.GetComponent<PlayerVariables>();
    }
    void Update()
    {
        if (_followPlayer)
        {
            GoFollowPlayer();
            MoveToThrowDir();
            StayInsideBound();
        }
    }

    public void FollowTarget(Transform target)
    {
        _targetTransform = target;
        _movingTargetTransform = null;
        MoveCameraToPosition(_targetTransform.position);
    }

    public void Go2Target(Transform target)
    {
        _targetTransform = null;
        _movingTargetTransform = target;
        MoveCameraToPosition(_movingTargetTransform.position);
    }

    private void MoveCameraToPosition(Vector3 position)
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(position.x, position.y, Camera.main.transform.position.z), _followSpeed * Time.deltaTime);
        ZoomIn();
    }

    private void MoveToThrowDir()
    {
        if (_targetPosition == transform.position)
        {
            return;
        }
        _targetPosition = (Vector2)_playerTransform.position + _playerVariables.ThrowDirection;
    }

    private void StayInsideBound()
    {
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, _boundaryPolygon.bounds.min.x + _camWidth, _boundaryPolygon.bounds.max.x - _camWidth);
        _targetPosition.y = Mathf.Clamp(_targetPosition.y, _boundaryPolygon.bounds.min.y + _camHeight, _boundaryPolygon.bounds.max.y - _camHeight);
        _targetPosition.z = -1.0f;
    }
    public void ZoomIn()
    {
        Vector2 dz = _zoomSpeed * Time.deltaTime * new Vector2(-5.0f, -5.0f);
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + dz.y, 2.3f, 3.5f);

        _camHeight = Camera.main.orthographicSize;
        _camWidth = Camera.main.aspect * _camHeight;
    }
}
