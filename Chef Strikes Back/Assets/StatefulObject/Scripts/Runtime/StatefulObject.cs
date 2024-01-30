using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Component to manage different states of a GameObejct
/// </summary>
[ExecuteInEditMode]
public class StatefulObject : MonoBehaviour
{
    [System.Serializable]
    public class StateEntry
    {
        public string StateName;
        public GameObject StateObject;
        public bool IsValid => !string.IsNullOrEmpty(StateName) && StateObject != null;
    };

    [SerializeField] private string _defaultStateName = null;
    [SerializeField] protected StateEntry _currentState = null;
    [SerializeField] protected List<StateEntry> _stateEntries = new List<StateEntry>(8);
    [SerializeField] protected bool _setDefaultStateOnBadStateRequest = false;

    /// <summary>
    /// The current state
    /// </summary>
    public StateEntry CurrentState => _currentState;

    /// <summary>
    /// Collection of all the available states
    /// </summary>
    public List<StateEntry> StateEntries => _stateEntries;

    /// <summary>
    /// Check for whether or not the current state is the default state
    /// </summary>
    public bool IsDefaultState => _currentState.StateName.Equals(_defaultStateName, System.StringComparison.Ordinal);

    protected virtual void OnEnable()
    {
#if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaved += HandleSceneSaving;
            return;
        }
#endif
        // Check to see if we're not currently in a state,
        // If we're not then we should set ourselves to the default state
        if (_currentState == null || _currentState.IsValid == false)
        {
            SetToDefaultState();
        }
    }

#if UNITY_EDITOR
    private void OnDisable()
    {
        if (Application.isPlaying) return;

        UnityEditor.SceneManagement.EditorSceneManager.sceneSaved -= HandleSceneSaving;
        if (_currentState != null && _currentState.StateName != _defaultStateName)
        {
            TrySetState(_defaultStateName);
        }
    }
#endif

    /// <summary>
    /// Sets the state to what the indicated default state is
    /// </summary>
    public void SetToDefaultState()
    {
        if (string.IsNullOrEmpty(_defaultStateName))
        {
            if (StateEntries.Count <= 0) return;
            Debug.LogWarning($"No default state given to [{gameObject.name}] - setting one now test");
            _defaultStateName = StateEntries[0].StateName;
            SetState(_defaultStateName, true);
        }
        else
        {
            SetState(_defaultStateName, true);
        }
    }

    /// <summary>
    /// Setup and create states based off of the children of the target
    /// </summary>
    /// <param name="target"></param>
    public void SetupValuesFromTarget(Transform target)
    {
        target.gameObject.SetActive(true);
        _stateEntries.Clear();
        _defaultStateName = null;
        // Go through the children of the target and register them as a state
        foreach (Transform t in target)
        {
            var targetObj = t.gameObject;
            var stateName = targetObj.name;
            _stateEntries.Add(new StateEntry() { StateName = stateName, StateObject = targetObj });
        }

        if (_stateEntries.Count > 0)
        {
            _defaultStateName = _stateEntries[0].StateName;
        }
    }

    /// <summary>
    /// Sets the state by name
    /// </summary>
    /// <param name="newState"></param>
    /// <returns></returns>
    public bool TrySetState(string newState)
    {
        return SetState(newState, false);
    }


    public void SetState(string newState)
    {
        SetState(newState, false);
    }

    /// <summary>
    /// Sets the state by name
    /// </summary>
    /// <param name="newState"></param>
    /// <param name="force"></param>
    /// <returns></returns>
    public virtual bool SetState(string newState, bool force)
    {
        _currentState = null;
        for (int i = _stateEntries.Count - 1; i >= 0; --i)
        {
            bool isTarget = false;
            if (_stateEntries[i] != null && _stateEntries[i].StateName != null)
            {
                isTarget = _stateEntries[i].StateName.Equals(newState, System.StringComparison.Ordinal);
            }

            if (isTarget)
            {
                _currentState = _stateEntries[i];
            }

            if (_stateEntries[i].StateObject != null)
            {
                _stateEntries[i].StateObject.SetActive(isTarget);
            }
        }

        // if we have successfully set the state we can early out.
        if (_currentState != null) return true;

        // otherwise try and force the default state if we can
        Debug.LogWarning($"<b>[UI]</b> Couldn't find state named [{newState}] on [{gameObject.name}]. Setting to default state");
        if (!_setDefaultStateOnBadStateRequest)
        {
            return false;
        }
        else
        {
            if (string.Equals(newState, _defaultStateName, System.StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else
            {
                return SetState(_defaultStateName, force);
            }
        }
    }

    /// <summary>
    /// Check to see if the given state is present
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool HasState(string state)
    {
        for (int i = _stateEntries.Count - 1; i >= 0; --i)
        {
            if (_stateEntries[i].StateName.Equals(state, System.StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check to see if this object is a target of a state
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsEntry(GameObject target)
    {
        for (int i = _stateEntries.Count - 1; i >= 0; --i)
        {
            if (_stateEntries[i].StateObject.GetInstanceID() == target.GetInstanceID())
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns a state based off of index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public StateEntry GetStateFromIndex(int index)
    {
        if (index < 0 || index >= _stateEntries.Count)
        {
            return null;
        }

        return _stateEntries[index];
    }

    /// <summary>
    /// Returns a state given a name
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    public StateEntry GetStateFromName(string stateName)
    {
        for (int i = _stateEntries.Count - 1; i >= 0; --i)
        {
            if (_stateEntries[i].StateName.Equals(stateName, System.StringComparison.Ordinal))
            {
                return _stateEntries[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Return the index of the current state
    /// </summary>
    /// <returns></returns>
    public int GetCurrentStateIndex()
    {
        if (_currentState == null) return -1;

        for (int i = _stateEntries.Count - 1; i >= 0; --i)
        {
            if (string.IsNullOrEmpty(_stateEntries[i].StateName)) continue;

            if (_stateEntries[i].StateName.Equals(_currentState.StateName, System.StringComparison.Ordinal))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Returns the current state
    /// </summary>
    /// <returns></returns>
    public StateEntry GetCurrentState()
    {
        if (_currentState == null) return null;

        for (int i = _stateEntries.Count - 1; i >= 0; --i)
        {
            if (string.IsNullOrEmpty(_stateEntries[i].StateName)) continue;

            if (_stateEntries[i].StateName.Equals(_currentState.StateName, System.StringComparison.Ordinal))
            {
                return _stateEntries[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Set the current state to the next state in the state list
    /// If the index has reached the end it will loop back from the beginning
    /// </summary>
    public void SetToNextState()
    {
        SetState(GetStateFromIndex((GetCurrentStateIndex() + 1) % _stateEntries.Count).StateName, true);
    }

    /// <summary>
    /// Sets the state to a random state
    /// </summary>
    public void SetToRandomState()
    {
        SetState(GetStateFromIndex(Random.Range(0, _stateEntries.Count)).StateName, true);
    }

#if UNITY_EDITOR

    #region Editor Stuff

    /// <summary>
    /// We use this function to reset back to the defaultState when we save to avoid accidentally saving in the wrong unexpected state
    /// </summary>
    /// <param name="scene">The scene being saved, we only care about the scene we are in</param>
    private void HandleSceneSaving(UnityEngine.SceneManagement.Scene scene)
    {
        if (gameObject.scene != scene) return;

        if (_currentState.StateName != _defaultStateName)
        {
            TrySetState(_defaultStateName);
        }
    }

    #endregion

#endif
}
