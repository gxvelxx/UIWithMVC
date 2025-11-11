using System;
using UnityEngine;

//체력 변화가 있을 수 있는 플레이어
public class PlayerModel
{
    private int health = 100;
    public int Health => health; // getter { get {return health;} }

    private int jumpCount = 0;
    public int JumpCount => jumpCount;

    public event Action<int> OnHealthChange; // 체력 변했을 때 발동시킬 메서드 등록하는 곳
    public event Action<int> OnjumpCountChange; // 점프횟수 알림

    public void TakeHeal(int amount)
    {
        health = health + amount;
        OnHealthChange?.Invoke(health);
    }

    public void TakeHit(int amount)
    {
        health = health - amount;
        OnHealthChange?.Invoke(health);
    }

    public void AddJumpCount()
    {
        jumpCount++;
        OnjumpCountChange?.Invoke(jumpCount);
    }
}
