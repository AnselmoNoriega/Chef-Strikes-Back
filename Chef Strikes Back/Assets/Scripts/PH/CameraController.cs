using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 24.0f;
    public float zoomSpeed = 1500.0f;
    private InputAction zoom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //dx, dy should use player's position to set the camera up
        float dx = panSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float dy = panSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        Camera.main.transform.Translate(dx, dy, 0);

        float dz = zoomSpeed * Time.deltaTime * Input.GetAxis("Zoom");
        
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + dz, 1.0f, 1.9f);

    }
}
