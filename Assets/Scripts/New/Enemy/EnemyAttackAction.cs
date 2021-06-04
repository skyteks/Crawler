using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AI/Enemy Actions/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    public int attackScore = 3;
    public float recoveryTime = 2f;

    public float attackAngle = 70f;

    public Range distanceNeededToAttack = new Range(0f, 3f);
}
