using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doom : StatusEffect
{
    public Doom(int turns, CombatUnit unit)
    {
        if (unit.GetType() == typeof(Enemy) && ((Enemy)unit).enemyType == Enemy.EnemyType.BOSS) return;
        this.remainingTurn = turns;
        this.unit = unit;
        this.statusPrefab = Resources.Load<GameObject>(path + "Doom");
        this.TakeEffect(unit);
    }


    public override void RemoveEffect()
    {
        unit.TakeDamage(null, unit.HP);
    }

    public override int DecreaseTurn()
    {
        this.remainingTurn--;
        if (this.remainingTurn == 0)
        {
            RemoveEffect();
            return -2;
        }
        return -1;
    }
}
