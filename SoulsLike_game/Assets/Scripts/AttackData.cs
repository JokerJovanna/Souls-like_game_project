using UnityEngine;

public struct AttackData
{
    public float Damage;
    public GameObject Attacker;
    public bool CanBeBlocked;

    public AttackData(float damage, GameObject attacker, bool canBeBlocked)
    {
        Damage = damage; 
        Attacker = attacker; 
        CanBeBlocked = canBeBlocked;
    }
}
