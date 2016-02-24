using UnityEngine;
using System.Collections;


public interface IDamageable
{
    void TakeDamage(float damage);
}

public interface IKillable
{
    void Killed();
}

public interface ILootable
{
    void DropLoot();
}

