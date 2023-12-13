using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    public GameObject debugObject;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void print()
    {
        Instantiate(debugObject, this.transform.position, this.transform.rotation);
    }
}
