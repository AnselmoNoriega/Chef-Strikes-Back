using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestingCam : MonoBehaviour
{
    [SerializeField] Transform playerPos; 
    [SerializeField] Transform lightPos; 
    public float followSpeed = 0.05f; 
    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(playerPos.position.x, playerPos.position.y, lightPos.position.z - 1.0f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
    }
}

