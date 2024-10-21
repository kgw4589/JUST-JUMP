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

    public void GetDamage(float damage)
    {
        playerHp -= damage;
    }

    public void IsJumpChange(bool jumpAble)
    {
        isJump = jumpAble;
    }
}
