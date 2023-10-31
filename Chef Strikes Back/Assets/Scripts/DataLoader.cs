using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour, IGameModule
{
    [SerializeField] private List<ScriptableObject> _dataObjects;

    public ScriptableObject GetData(int index)
    {
        return _dataObjects[index];
    }

    public IEnumerator LoadModule()
    {
        // Load data... 
        ServiceLocator.Register<DataLoader>(this);
        yield break;
    }
}
