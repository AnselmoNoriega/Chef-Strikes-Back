using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    public static int FaceMovementDirection(Animator animator, Vector2 lookDirection, string[] directionNames)
    {
        int directionIndex = Mathf.FloorToInt((Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 360 + 22.5f) / 45f) % 8;
        animator.Play(directionNames[directionIndex]);
        Debug.Log(directionIndex);
        return directionIndex;
    }

    public static string GetDirection(Vector2 lookDirection, string[] animDirection)
    {
        int directionIndex = Mathf.FloorToInt((Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 360 + 22.5f) / 45f) % 8;
        return animDirection[directionIndex];
    }
}
