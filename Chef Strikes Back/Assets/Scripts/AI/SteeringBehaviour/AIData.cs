using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    public List<Transform> targets = null;
    public List<Transform> closeList = null;
    public Collider2D[] obstacles = null;

    public AIState state;
    public bool isStand;

    public Transform Target;
    public Transform currentTarget;

    [SerializeField]
    public LayerMask TargetLayerMask;

    public int GetTargetsCount() => targets == null ? 0 : targets.Count;
}
