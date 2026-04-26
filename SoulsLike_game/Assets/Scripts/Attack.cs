using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public abstract float AttackDistance { get; }
    public abstract bool IsPerforming { get; }
    public abstract void Perform(GameObject attacker, GameObject target);

}
