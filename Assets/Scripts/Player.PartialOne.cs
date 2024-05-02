using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    public void ChangeJumpPower(float speed)
    {
        maxPower = speed;
    }

    public void Bounce(Vector2 dir, float power)
    {
        _rd.AddForce(dir * power , ForceMode2D.Impulse);
    }
    public void StopMove(Vector2 dir, float power)
    {
        _rd.AddForce(dir * power , ForceMode2D.Impulse);
    }
}
