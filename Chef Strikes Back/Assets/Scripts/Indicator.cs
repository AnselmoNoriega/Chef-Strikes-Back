/*using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float offScreenBound = 10f;
    [SerializeField] private Camera camera;
    [SerializeField] private float viewportMarginMin = 0.1f;
    [SerializeField] private float viewportMarginMax = 0.9f;
    [SerializeField] private SpriteRenderer indicator;

    void Update()
    {
        Debug.LogWarning("Debuging");
        if (target == null || camera == null)
        {
            Debug.LogWarning("Indicato missing references.");
            return;
        }

        if (!ShouldDisplayIndicator())
        {
            //gameObject.SetActive(false);
            indicator.enabled = false;
            return;
        }

        indicator.enabled = false;
        //gameObject.SetActive(true);
        PositionAndRotateIndicator();
    }

    private bool ShouldDisplayIndicator()
    {
        Vector3 targetDirection = target.transform.position - transform.position;
        if (targetDirection.magnitude > offScreenBound) return false;

        Vector3 targetViewPoint = camera.WorldToViewportPoint(target.transform.position);
        return targetViewPoint.z <= 0 || targetViewPoint.x <= viewportMarginMin || targetViewPoint.x >= viewportMarginMax
            || targetViewPoint.y <= viewportMarginMin || targetViewPoint.y >= viewportMarginMax;
    }

    private void PositionAndRotateIndicator()
    {
        Vector3 targetViewPoint = camera.WorldToViewportPoint(target.transform.position);
        Vector3 screenEdge = camera.ViewportToWorldPoint(new Vector3(Mathf.Clamp(targetViewPoint.x, viewportMarginMin, viewportMarginMax),
                                                                    Mathf.Clamp(targetViewPoint.y, viewportMarginMin, viewportMarginMax),
                                                                    camera.nearClipPlane));
        transform.position = new Vector3(screenEdge.x, screenEdge.y, 0);

        Vector3 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
*/

using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public GameObject indicator;
    public GameObject Target;
    Renderer rd;

    private void Start()
    {
        rd = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (!rd.isVisible)
        {
            if (!indicator.activeSelf)
            {
                indicator.SetActive(true);
            }
            Vector2 Direction = Target.transform.position - transform.position;
            RaycastHit2D ray = Physics2D.Raycast(transform.position,Direction);

            if (ray.collider != null)
            {
                indicator.transform.position = ray.point;
            }
        }
        else
        {
            if (indicator.activeSelf)
            {
                indicator.SetActive(false);
            }
        }
    }

}
