using UnityEngine;

public abstract class View : MonoBehaviour
{
    public bool initialized { get; private set; }

    public virtual void init()
    {
        initialized = true;
    }

}
