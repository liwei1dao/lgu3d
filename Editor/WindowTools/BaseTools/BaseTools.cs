using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class BaseTools
{
    protected EditorWindow MyWindow;

    public BaseTools(EditorWindow _MyWindow)
    {
        MyWindow = _MyWindow;
    }

    public virtual void OnGUI()
    {

    }


    public virtual void OnClose()
    {

    }

    public virtual void OnDestroy()
    {

    }
}

public abstract class BaseTools<T> : BaseTools
{
    protected T Parent;
    public BaseTools(EditorWindow _MyWindow,T _Parent)
        :base(_MyWindow)
    {
        Parent = _Parent;
    }

}
