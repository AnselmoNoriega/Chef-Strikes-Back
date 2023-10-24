using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class AIData : MonoBehaviour
{
    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;

    public Transform currentTarget;

    [SerializeField]
    public LayerMask TargetLayerMask;

    public int GetTargetsCount() => targets == null ? 0 : targets.Count;
}
