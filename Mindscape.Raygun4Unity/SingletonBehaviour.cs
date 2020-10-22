using UnityEngine;

namespace Mindscape.Raygun4Unity
{
  public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
  {
    private static T _instance;
    private static object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
      get
      {
        if (_applicationIsQuitting) 
        {
          return null;
        }
  
        lock(_lock)
        {
          if (_instance == null)
          {
            _instance = (T)FindObjectOfType(typeof(T));
          
            if (_instance == null)
            {
              GameObject singleton = new GameObject();
              _instance = singleton.AddComponent<T>();
              singleton.name = typeof(T).ToString();

              DontDestroyOnLoad(singleton);
            }
          }
          return _instance;
        }
      }
    }

    public virtual void Awake() 
    {
      DontDestroyOnLoad(this.gameObject);
    }

    public void OnDestroy()
    {
      _applicationIsQuitting = true;
    }
  }
}