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

    private List<Transform> colliders;
    Collider2D targetCollider = null;

    public override void Detect(AIData aiData)
    {

        if(aiData.state == AIState.Good)
        {
            if (aiData.Target == null || !aiData.Target.GetComponent<Chair>().seatAvaliable)
            {
                targetCollider = getRandomChair(Physics2D.OverlapCircleAll(transform.position, targetDetectionRange, aiData.TargetLayerMask));
                aiData.Target = targetCollider.transform;
                if (targetCollider != null)
                {
                    aiData.targets = ServiceLocator.Get<ChairFinder>().CheckNextMove(this.transform, aiData);
                }
            }
        }
        else if(aiData.state == AIState.Bad)
        {
            if(aiData.Target == null && !aiData.isStand)
            {
                aiData.targets = ServiceLocator.Get<ChairFinder>().CheckNextLocate(this.transform, aiData);
            }
        }
        else if(aiData.state == AIState.Leaving)
        {
            if (aiData.Target == null)
            {
                aiData.currentTarget = null;
                GameObject ExitPoint = new GameObject("NullTarget");
                ExitPoint.transform.position = ServiceLocator.Get<TileManager>().requestEntrancePos();
                aiData.Target = ExitPoint.transform;
                aiData.targets = ServiceLocator.Get<ChairFinder>().CheckNextMove(this.transform, aiData);
            }
        }
        else if(aiData.state == AIState.Rage)
        {
            targetCollider = Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask);
            if(targetCollider != null) 
            {
                colliders = new List<Transform> { targetCollider.transform };
            }
            aiData.targets = colliders;
        }

    }

    public Collider2D getRandomChair(Collider2D[] targetCollider)
    {
        //float distance = 1000;
        var random = Random.Range(0, targetCollider.Length);
        Collider2D targetChair = null;
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
