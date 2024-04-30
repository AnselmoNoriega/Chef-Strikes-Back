using UnityEngine;

public class TutorialCameraManager : MonoBehaviour
{
    [Header("Camera Variables")]
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _zoomSpeed;
    private Vector3 _targetPosition;
    private float _camHeight;
    private float _camWidth;
    [SerializeField] private bool _followPlayer = true;

    [Space]
    [SerializeField] private PolygonCollider2D _boundaryPolygon;
    private Transform _playerTransform;
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
            StayInsideBound();
            MoveToThrowDir();
        }
        else
        {
            StartNarrativeMovement();
        }
    }

    private void GoFollowPlayer()
    {
        if (_playerTransform != null)
        {
            _targetPosition = _playerTransform.position;
            MoveCameraToPosition(_targetPosition);
        }
    }

    private void MoveCameraToPosition(Vector3 position)
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(position.x, position.y, Camera.main.transform.position.z), _followSpeed * Time.deltaTime);
        ZoomIn();
    }

    private void StartNarrativeMovement(int index = 0)
    {
        _followPlayer = false;
        var tlm = ServiceLocator.Get<TutorialLoopManager>();
        if (index < tlm.FocusPositions.Count)
        {
            _targetPosition = tlm.FocusPositions[index].transform.position;
            MoveCameraToPosition(_targetPosition);
            if (Vector3.Distance(Camera.main.transform.position, _targetPosition) < 0.1f)
            {
                if (index + 1 < tlm.FocusPositions.Count)
                {
                    StartNarrativeMovement(index + 1);
                }
                else
                {
                    _followPlayer = true;
                }
            }
        }
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
    public void SetFollowPlayer(bool shouldfollow)
    {
        _followPlayer = shouldfollow;
    }
}
