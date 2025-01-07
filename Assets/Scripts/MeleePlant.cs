using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePlant : Plant
{
    [SerializeField]private float lastAttackTime;
    [SerializeField]private float attackCooldown;
    [SerializeField]private float attackDamage;

    public override void Attack()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            List<GameObject> zombies = GetZombiesInArea(1);
            foreach (var zombie in zombies)
            {
                if (IsTargetInCellRange(zombie.transform.position))
                {
                    zombie.GetComponent<Zombie>().TakeDamage(attackDamage);
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}