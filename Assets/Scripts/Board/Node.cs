using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{

    public bool IsUsable {  get; private set; }
    public GameObject Object;

    public Node(bool isUsable, GameObject obj)
    {
        IsUsable = isUsable;
        Object = obj;
    }
}
