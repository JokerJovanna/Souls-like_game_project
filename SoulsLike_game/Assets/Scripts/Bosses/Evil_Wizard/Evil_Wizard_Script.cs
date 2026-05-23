using System.Runtime.CompilerServices;
using UnityEngine;

public class Evil_Wizard_Script : MonoBehaviour
{
    [SerializeField] private PortalScript portal;
    private EnemyHealthComponent health;
    private PlayerChaserComponent chaser;
    private Evil_Wizard_AttackComponent attack;

    private void Awake()
    {
        health = GetComponent<EnemyHealthComponent>();
        chaser = GetComponent<PlayerChaserComponent>();
        attack = GetComponent<Evil_Wizard_AttackComponent>();
    }

    private void Start()
    {
        InitailizeComponents();
        DisableCollisionWithPlayer();
        var player = GameObject.FindWithTag("Player");
        if (player != null)
            chaser.SetTarget(player);
    }

    private void DisableCollisionWithPlayer()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;
        var player_col = player.GetComponent<Collider2D>();
        var myCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(player_col, myCollider);
    }
    
    private void InitailizeComponents()
    {
        health.enabled = true;
        attack.enabled = true;
        chaser.enabled = true;

        health.OnDied += DisableComponents;
        if (portal != null) health.OnDied += portal.Activate;
    }

    private void DisableComponents()
    {
        health.enabled = false;
        attack.enabled = false;
        chaser.enabled = false;
    }
}
