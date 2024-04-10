using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTTest : TempMapTrigger
{
    [SerializeField] private float temp;
    
    public override void EnterEvent()
    {
        Debug.Log(temp);
    }

    public override void ExitEvent()
    {
        
    }
}
