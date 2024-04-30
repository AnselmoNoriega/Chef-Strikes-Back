using UnityEngine;

public class TutorialCameraManager : MonoBehaviour
{
    [Header("Camera Variables")]
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _zoomSpeed;
    private Vector3 _targetPosition;
    private float _camHeight;
    private float _camWidth;

    private Transform _playerTransform;

    [SerializeField] private bool _followPlayer = true;

    public void Initialize()
    {
        _camHeight = Camera.main.orthographicSize;
        _camWidth = Camera.main.aspect * _camHeight;

        var player = ServiceLocator.Get<Player>();
        _playerTransform = player.transform;
    }
    void Update()
    {
        if (_followPlayer)
        {
            GoFollowPlayer();
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
