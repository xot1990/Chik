using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planlhouse : Plant
{
    protected override void Die()
    {
        EventBus.RaiseOnGameOver();
    }

    public override void Attack()
    {
        
    }
}
