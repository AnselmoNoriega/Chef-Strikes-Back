using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SystemManager : MonoBehaviour
{
    static SystemManager instance;
    List<Manager> managers = new List<Manager>();

    public event Action OnApplicationEnd;

    public static SystemManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SystemManager>();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                UnityEditor.SceneVisibilityManager.instance.Show(gameObject, true);
#endif
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        GetComponentsInChildren(managers);

        foreach (var manager in managers)
            manager.OnStart();
    }

    public T Get<T>() where T : Manager
    {
        Type type = typeof(T);

        foreach (Manager manager in managers)
            if (manager.GetType() == type)
                return manager as T;

        T item = FindObjectOfType<T>();
        if (item != null) return item;

        Debug.LogWarning("Fallback - Instatiated a new manager of type: " + type.Name);

        item = new GameObject(type.Name).AddComponent<T>();
        item.transform.SetParent(transform);
        return item;
    }

    private void OnDestroy()
    {
        if (!instance || instance != this) return;

        foreach (var manager in managers)
            manager.OnEnd();

        OnApplicationEnd?.Invoke();
        instance = null;
        
    }
}
