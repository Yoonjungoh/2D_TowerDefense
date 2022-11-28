using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum MultiplySpeedType
    {
        X1,
        X2,
        X3,
    }
    public enum SystemMSGType
    {
        Money,
        Build
    }
    public enum EnemyDestroyType
    {
        Kill,
        Arrive,
    }
    public enum WeaponType
    {
        Canon,
        Laser,
        Slow,
        Buff,
    }
    public enum WeaponState
    {
        SearchTarget,
        TryAttackCanon,
        TryAttackLaser,
    }
}
