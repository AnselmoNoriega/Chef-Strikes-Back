using System;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public GameObject target;
    public float offScreenBound = 10f;
    public Camera camera;
    private bool isIndicatorActive = true;

    private void Update()
    {
        if (isIndicatorActive)
        {
            Vector3 targetDirectin = target.transform.position - transform.position;
            float distanceToTarget = targetDirectin.magnitude;

            if (distanceToTarget > offScreenBound)
            {
                gameObject.SetActive(false);
                isIndicatorActive = false;
            }
            else
            {
                Vector3 targetViewPoint = camera.WorldToViewportPoint(target.transform.position);

                if (targetViewPoint.z > 0 && targetViewPoint.x > 0 && targetViewPoint.x < 1 && targetViewPoint.y > 0 && targetViewPoint.y < 1)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(true);
                    Vector3 screenEdge = camera.ViewportToWorldPoint(new Vector3(Mathf.Clamp(targetViewPoint.x, 0.1f, 0.9f)
                                                                    , Mathf.Clamp(targetViewPoint.y, 0.1f, 0.9f)
                                                                    , camera.nearClipPlane));
                    transform.position = new Vector3(screenEdge.x, screenEdge.y, 0);
                    Vector3 direction = target.transform.position - transform.position;
                    float angle = MathF.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
    }
}
