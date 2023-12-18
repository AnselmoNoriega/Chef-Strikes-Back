using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairFinder : MonoBehaviour
{
    [SerializeField] private List<Transform> checkPointPos;
    [SerializeField] private List<Transform> enemyCheckPoint;
    [SerializeField] private List<Transform> enemyStandPoint;

    public List<Transform> CheckNextMove(Transform aiPos, AIData aiData)
    {
        List<Transform> openList = new List<Transform>();
        List<Transform> closeList = new List<Transform>();
        Transform currentPos = aiPos;
        Transform checkpoint = currentPos;
        aiData.closeList = closeList;
        foreach(var checkpt in checkPointPos)
        {
            openList.Add(checkpt);
        }
        while (openList.Count > 0) 
        {
            
                float DistanceCheck = Vector2.Distance(currentPos.position, aiData.Target.position);
                List<Transform> neighbour = GetNeighbour(currentPos, openList);
                foreach (var checkpt in neighbour)
                {
                    if(closeList.Contains(checkpt) || aiData.closeList.Contains(checkpt))
                    {
                        continue;
                    }
                    float checkptToChair = Vector2.Distance(checkpt.position, aiData.Target.position);
                    if(checkptToChair < DistanceCheck) 
                    {
                        checkpoint = checkpt;
                        DistanceCheck = checkptToChair;
                    }
                    else
                    {
                        openList.Remove(checkpt);
                    }
                }
                openList.Remove(checkpoint);
                if (currentPos == checkpoint) break;
                if(!closeList.Contains(checkpoint))
                {
                    closeList.Add(checkpoint);
                    currentPos = checkpoint;
                }
            
        }
        closeList.Add(aiData.Target);

        return closeList;
    }

    public List<Transform> CheckNextLocate(Transform aiPos, AIData aiData)
    {
        List<Transform> openList = new List<Transform>();
        List<Transform> closeList = new List<Transform>();
        Transform currentPos = aiPos;
        Transform checkpoint = currentPos;
        aiData.currentTarget = null;
        foreach (var checkpt in enemyCheckPoint)
        {
            openList.Add(checkpt);
        }

        //check if all position is token
        foreach (var enemypt in enemyStandPoint)
        {
            Collider2D hit = Physics2D.OverlapCircle(enemypt.position, 0.1f);
            if (!hit.CompareTag("Enemy")) 
            {
                aiData.Target = enemypt;
                break;
            }
            continue;
        }
        
        if(aiData.Target == null) 
        {
            GameObject nullTarget = new GameObject("NullTarget");
            nullTarget.transform.position = ServiceLocator.Get<TileManager>().requestEmptyPos();
            aiData.Target = nullTarget.transform;
        }


        while (openList.Count > 0)
        {

            float DistanceCheck = Vector2.Distance(currentPos.position, aiData.Target.position);
            List<Transform> neighbour = new List<Transform>();
            neighbour =  GetNeighbour(currentPos, openList);
            foreach (var checkpt in neighbour)
            {
                if (closeList.Contains(checkpt))
                {
                    continue;
                }
                float checkptToChair = Vector2.Distance(checkpt.position, aiData.Target.position);
                if (checkptToChair < DistanceCheck)
                {
                    checkpoint = checkpt;
                    DistanceCheck = checkptToChair;
                }
                else
                {
                    openList.Remove(checkpt);
                }
            }
            openList.Remove(checkpoint);
            if (currentPos == checkpoint) break;
            if (!closeList.Contains(checkpoint))
            {
                closeList.Add(checkpoint);
                currentPos = checkpoint;
            }

        }
        closeList.Add(aiData.Target);
        return closeList;
    }
    List<Transform> GetNeighbour(Transform currentPos, List<Transform> list)
    {
        //get two point
        List<Transform> neighbour = new List<Transform>();
        foreach(var checkPt in list) 
        {
            if(neighbour.Count < 2)
            {
                neighbour.Add(checkPt);
            }
            else
            {
                for (int i = 0; i < neighbour.Count; i++)
                {
                    float distanceToCheck = Vector2.Distance(currentPos.position, checkPt.position);
                    float distanceToNeighbour = Vector2.Distance(currentPos.position, neighbour[i].position);
                    if (distanceToCheck < distanceToNeighbour)
                    {
                        neighbour[i] = checkPt;
                        break;
                    }
                }
            }
        }
        return neighbour;
    }
}
