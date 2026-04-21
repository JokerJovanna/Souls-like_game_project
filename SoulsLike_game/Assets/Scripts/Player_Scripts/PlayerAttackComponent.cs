using UnityEngine;

public class PlayerAttackComponent : MonoBehaviour
{
    [Header("Настройки атаки")]
    [SerializeField] private KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] private Attack[] attacks;          // массив доступных атак (можно один)
    [SerializeField] private float cooldown = 0.5f;     // кулдаун между атаками
    [SerializeField] private float staminaCost = 20f;   // стоимость в выносливости

    private StaminaComponent stamina;
    private float cooldownTimer;
    private int nextAttack;

    private void Start()
    {
        stamina = GetComponent<StaminaComponent>();
        if (stamina == null)
            Debug.LogError("PlayerAttackComponent требует компонент StaminaComponent!");

        if (attacks == null || attacks.Length == 0)
            Debug.LogError("Не назначены атаки в PlayerAttackComponent!");

        nextAttack = Random.Range(0, attacks.Length);
    }

    private void Update()
    {
        // Обновляем кулдаун
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        // Ввод атаки
        if (Input.GetKeyDown(attackKey))
        {
            // Проверка выносливости
            if (!stamina.TrySpendStamina(staminaCost))
            {
                Debug.Log("Недостаточно выносливости для атаки");
                return;
            }

            // Выбираем текущую атаку
            var attack = attacks[nextAttack];

            // Ищем цель перед игроком (можно улучшить – например, лучом или круговым сканированием)
            GameObject target = FindTarget(attack.AttackDistance);
            if (target != null)
            {
                attack.Perform(gameObject, target);
                cooldownTimer = cooldown;

                // Переключаемся на следующую атаку (если их несколько)
                nextAttack = (nextAttack + 1) % attacks.Length;
            }
            else
            {
                Debug.Log("Нет цели в радиусе атаки");
            }
        }
    }

    /// <summary>
    /// Поиск ближайшего врага в радиусе атаки.
    /// Можно заменить на Physics2D.OverlapCircle, как в прошлых версиях.
    /// </summary>
    private GameObject FindTarget(float attackDistance)
    {
        // Ищем всех врагов с компонентом HealthComponent в радиусе
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackDistance);
        float closestDistance = attackDistance + 1f;
        GameObject closestTarget = null;

        foreach (var hit in hitColliders)
        {
            // Проверяем, есть ли у объекта компонент HealthComponent (враг)
            var health = hit.GetComponent<EnemyHealthComponent>();
            if (health != null)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = hit.gameObject;
                }
            }
        }
        return closestTarget;
    }

    // Визуализация радиуса атаки в редакторе (для отладки)
    private void OnDrawGizmosSelected()
    {
        if (attacks != null && attacks.Length > 0 && attacks[0] != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attacks[0].AttackDistance);
        }
    }
}