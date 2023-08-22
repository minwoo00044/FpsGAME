using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    public EnemyFSM fsm;

    public void HitPlayer()
    {
        fsm.AttackAction();
    }
}
