using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] private List<Chair> _chairs = new();
    [SerializeField] private List<CreationTable> _combiner = new();

    [SerializeField] private Transform[] _exitPoint;
    [SerializeField] private Transform[] _copStartPoint;
    [SerializeField] private Transform[] _badAiPoint;

    private List<AI> _goodCustomers;

    public Chair GiveMeChair()
    {
        var chair = _chairs[Random.Range(0, _chairs.Count)];
        _chairs.Remove(chair);
        return chair;
    }

    public int GetAvailableChairsCount()
    {
        return _chairs.Count;
    }

    public void AddAvailableChair(Chair chair)
    {
        _chairs.Add(chair);
    }

    public Vector2 ExitPosition()
    {
        return _exitPoint[Random.Range(0, _exitPoint.Length)].position;
    }

    public Vector2 CopEnterPosition()
    {
        return _copStartPoint[Random.Range(0, _copStartPoint.Length)].position;
    }

    public Vector2 BadAiEnterPosition()
    {
        return _badAiPoint[Random.Range(0, _badAiPoint.Length)].position;
    }

    public void AddHungryCustomer(AI customerPos)
    {
        _goodCustomers.Add(customerPos);
    }

    public void RemoveCustomer(AI customerPos)
    {
        _goodCustomers.Remove(customerPos);
    }

    public AI GetRandomCustomer()
    {
        if (_goodCustomers.Count > 0)
        {
            var customer = _goodCustomers[Random.Range(0, _goodCustomers.Count)];
            RemoveCustomer(customer);
            return customer;
        }
        return null;
    }

    public CreationTable GiveMeCreationTable()
    {
        var combiner = _combiner[Random.Range(0, _combiner.Count)];
        _combiner.Remove(combiner);
        return combiner;
    }
    public void Lock(CreationTable combiner)
    {
        combiner.isLocked = true;
    }

    public int GetUnLockedTable()
    {
        return _combiner.Count;
    }

    public void UnLockTable(CreationTable combiner)
    {
        _combiner.Add(combiner);
    }
}
