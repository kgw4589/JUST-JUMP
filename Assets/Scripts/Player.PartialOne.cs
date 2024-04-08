using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    public void SpringBoard(float power)
    {
        _rd.AddForce(transform.up * power , ForceMode2D.Impulse);
    }
    public void ChangeJumpPower(float speed)
    {
        maxPower = speed;
    }

    public void Bounce(Vector2 dir, float power)
    {
        
    }
}
