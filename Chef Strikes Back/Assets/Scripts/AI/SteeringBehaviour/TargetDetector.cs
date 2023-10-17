using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField]
    private float targetDetectionRange = 5;

    [SerializeField]
    private LayerMask obstactesLayerMask, targetLayerMask;

    [SerializeField]
    private bool showGizmos = false;

    private List<Transform> colliders;

    public override void Detect(AIData aiData)
    {
        Collider2D targetCollider = getClosestObject(Physics2D.OverlapCircleAll(transform.position, targetDetectionRange, targetLayerMask));

        if (targetCollider != null) 
        {
            Vector2 direction = (targetCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, targetDetectionRange, obstactesLayerMask);
            
            if(hit.collider != null && (targetLayerMask & (1<<hit.collider.gameObject.layer)) != 0)
            {
                colliders = new List<Transform>() { targetCollider.transform };
            }
            else
            {
                colliders = null;
            }
            aiData.targets = colliders;
        }
    }

    public Collider2D getClosestObject(Collider2D[] targetCollider)
    {
        float distance = 1000;
        Collider2D targeChair = null;
        foreach(var target in targetCollider)
        {
            if(target.gameObject.GetComponent<Chair>().seatAvaliable)
            {
                if (Vector2.Distance(target.transform.position, transform.position) < distance)
                {
                    targeChair = target;
                    
                }
            }
        }
        return targeChair;

    }

    

    private void OnDrawGizmosSelected()
    {
        if(showGizmos == false) return;

        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        if (colliders == null)
            return;
        Gizmos.color = Color.magenta;
        foreach(var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}
