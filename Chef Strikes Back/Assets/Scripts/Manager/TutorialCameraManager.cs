using System.Collections;
using System.Collections.Generic;
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
    private PlayerVariables _playerVariables;

    [SerializeField] private List<GameObject> _narrativePos;
    [SerializeField] private bool _followPlayer = true;

    public void MoveThePos(Vector3 position)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _targetPosition = position;
            _followPlayer = false;

            Vector2 dz = _zoomSpeed * Time.deltaTime * (-new Vector2(5.0f, 5.0f));
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + dz.y, 2.3f, 3.5f);

            _camHeight = Camera.main.orthographicSize;
            _camWidth = Camera.main.aspect * _camHeight;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _followPlayer = true;
        }
    }
    void Update()
    {
        if (_narrativePos.Count > 0)
        {
            foreach (var pos in _narrativePos)
            {
                Vector3 newpos = pos.transform.position;
                MoveThePos(newpos);
            }
        }
        if (_followPlayer)
        {
            _targetPosition = (Vector2)_playerTransform.position;
        }
    }
}
