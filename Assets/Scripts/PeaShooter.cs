using UnityEngine;

public class PeaShooter : Plant
{
    public float attackDamage = 20f;
    public float attackRange = 1.0f;
    public float attackCooldown = 1.5f;
    public Transform firePoint;
    public GameObject projectilePrefab;
    private float lastAttackTime;
    public LayerMask enemyLayers;

    void Update()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }
    void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayers);

        foreach (var hitCollider in hitColliders)
        {
            if (hitColliders.Length > 0) {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

                Projectile projectileComponent = projectile.GetComponent<Projectile>();
                if (projectileComponent != null)
                {
                    projectileComponent.SetTarget(hitCollider.transform);
                }

            }

        }
    }
    public override void OnLevelEnd()
    {
        Destroy(gameObject);
    }

}