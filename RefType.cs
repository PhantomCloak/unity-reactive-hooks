using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RefType<T>
{
    private T _value;
    private Dictionary<WeakReference<object>, Action<T>> _onValueChangedList = new Dictionary<WeakReference<object>, Action<T>>();
    private static bool isInit = false;

    public RefType(T value = default)
    {
        if (!isInit)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            isInit = true;
        }

        _value = value;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _onValueChangedList.Clear();
    }

    public Action<T> this[object instance]
    {
        set
        {
            _onValueChangedList.Add(new WeakReference<object>(instance), value);
            value.Invoke(Value);
        }
    }

    public T Value
    {
        get => _value;
        set
        {
            _value = value;

            foreach (var callback in _onValueChangedList)
            {
                if (callback.Key != null && !callback.Key.TryGetTarget(out var target) && target != null)
                {
                    continue;
                }

                try
                {
                    callback.Value.Invoke(value);
                }
                catch (Exception e)
                {
#if UNITY_ENGINE
                    if(e is UnityEngine.MissingReferenceException)
                    {
                        Logger.LogError("There might be some resouce management issue since reference couldn't find for the object: " + e);
                        continue;
                    }
#endif

                    Logger.LogError("An error occured inside callback: " + e);
                }
            }
        }
    }
}
