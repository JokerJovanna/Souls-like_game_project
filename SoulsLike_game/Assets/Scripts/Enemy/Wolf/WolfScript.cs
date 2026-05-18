using UnityEngine;

public class WolfScript : MonoBehaviour
{
    private AttackComponent attack;
    private PatrolComponent patrol;
    private PlayerChaserComponent chaser;
    private PlayerDetectorComponent detector;

    void Awake()
    { 
        attack = GetComponent<AttackComponent>();
        patrol = GetComponent<PatrolComponent>();
        chaser = GetComponent<PlayerChaserComponent>();
        detector = GetComponent<PlayerDetectorComponent>();
        if (attack == null) Debug.LogError("AttackComponent missing");
        if (patrol == null) Debug.LogError("PatrolComponent missing");
        if (chaser == null) Debug.LogError("PlayerChaserComponent missing");
        if (detector == null) Debug.LogError("PlayerDetectorComponent missing");
    }

    void Start()
    {
        InitializeComponents();
        DisableCollisionWithPlayer();
    }

    public void OnPlayerDetected(GameObject player)
    {
        patrol.enabled = false;
        chaser.SetTarget(player);
        attack.SetTarget(player);
    }

    public void OnPlayerLost()
    {
        patrol.enabled = true;
        chaser.ClearTarget();
        attack.ClearTarget();
    }

    private void OnDestroy()
    {
        if (detector != null)
        {
            detector.OnPlayerDetected -= OnPlayerDetected;
            detector.OnPlayerLost -= OnPlayerLost;
        }
    }

    private void InitializeComponents()
    {
        attack.enabled = true;
        patrol.enabled = true;
        chaser.enabled = true;
        detector.enabled = true;

        detector.OnPlayerDetected += OnPlayerDetected;
        detector.OnPlayerLost += OnPlayerLost;
    }

    private void DisableCollisionWithPlayer()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;
        var player_col = player.GetComponent<Collider2D>();
        var myCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(player_col, myCollider);
    }

    public void ResizeTriggerCollider(float coef)
    {
        BoxCollider2D triggerCollider = null;
        foreach (var col in GetComponents<BoxCollider2D>())
            if (col.isTrigger) triggerCollider = col;
        var size = triggerCollider.size;
        var newSize = new Vector2(size.x * coef, size.y * coef);
        triggerCollider.size = newSize;
    }
}
