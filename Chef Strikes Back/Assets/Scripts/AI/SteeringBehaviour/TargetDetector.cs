using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField]
    private float targetDetectionRange = 5;

    [SerializeField]
    private LayerMask obstactesLayerMask;
    [SerializeField]
    private LayerMask playerLayerMask;
    [SerializeField]
    private bool showGizmos = false;

    private List<Transform> colliders;
    Collider2D targetCollider = null;

    public override void Detect(AIData aiData)
    {
        
        if (!ServiceLocator.Get<GameManager>().rageMode)
        {
             targetCollider = getClosestObject(Physics2D.OverlapCircleAll(transform.position, targetDetectionRange, aiData.TargetLayerMask));
        }
        else
        {
            targetCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRange,playerLayerMask);
        }

        if (targetCollider != null) 
        {
            Vector2 direction = (targetCollider.transform.position - transform.position).normalized;

            for(int i = 0; i < targetDetectionRange;++i)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, i, obstactesLayerMask);
                
                if (hit.collider != null )
                {
                    colliders = new List<Transform>() { targetCollider.transform };
                }
                else
                {
                    colliders = null;
                }

                if(colliders != null)
                {
                    aiData.targets = colliders;
                    break;
                }
                
            }
           
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
                    distance = Vector2.Distance(target.transform.position, transform.position);
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
