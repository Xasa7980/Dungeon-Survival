using UnityEngine;

public class SingletonPersistent<T> : MonoBehaviour where T : Component
{
    public static T instance { get; private set; }
    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }
}
