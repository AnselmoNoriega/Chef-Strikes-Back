using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    [SerializeField]
    Collider2D ExitPoint;

    private List<Transform> colliders;
    Collider2D targetCollider = null;

    public override void Detect(AIData aiData)
    {
        
        if (aiData.TargetChair == null || !ServiceLocator.Get<GameLoopManager>().rageMode && aiData.targets.Count <= 0 || !aiData.TargetChair.GetComponent<Chair>().seatAvaliable)
        {
            aiData.targets = null;
            targetCollider = getRandomChair(Physics2D.OverlapCircleAll(transform.position, targetDetectionRange, aiData.TargetLayerMask));
            aiData.TargetChair = targetCollider.transform;
            if (targetCollider != null)
            {
                aiData.targets = ServiceLocator.Get<ChairFinder>().CheckNextMove(this.transform, aiData);
            }
        }
        else if(ServiceLocator.Get<GameLoopManager>().rageMode)
        {

        }

    }

    public Collider2D getRandomChair(Collider2D[] targetCollider)
    {
        //float distance = 1000;
        var random = Random.Range(0, targetCollider.Length);
        Collider2D targetChair = null;
        //foreach(var target in targetCollider)
        //{
        //    if(target.gameObject.GetComponent<Chair>().seatAvaliable)
        //    {
        //        if (Vector2.Distance(target.transform.position, transform.position) < distance)
        //        {
        //            targeChair = target;
        //            distance = Vector2.Distance(target.transform.position, transform.position);
        //        }
        //    }
        //}

        targetChair = targetCollider[random];
        if(targetChair.gameObject.GetComponent<Chair>().seatAvaliable)
        {
            return targetChair;
        }
        else
        {
            return getRandomChair(targetCollider);
        }
        

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
