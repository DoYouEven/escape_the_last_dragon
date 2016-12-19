using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singelton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>() as T;
            }
            if (instance == null)
            {
                GameObject instanceGameObject = new GameObject();
                instanceGameObject.name = "Instance";
                instance = instanceGameObject.AddComponent<T>();   
            }

            return instance;
        }
    }
   

}
