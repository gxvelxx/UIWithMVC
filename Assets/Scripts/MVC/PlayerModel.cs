using System;
using UnityEngine;

//체력 변화가 있을 수 있는 플레이어
public class PlayerModel
{
    private int health = 100;
    public int Health => health; // getter { get {return health;} }

    public event Action<int> OnHealthChange; // 체력 변했을 때 발동시킬 메서드 등록하는 곳

    public void TakeDamage(int amount)
    {
        health = health - amount;
        OnHealthChange?.Invoke(health);
    }
}
