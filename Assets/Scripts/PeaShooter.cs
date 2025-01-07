using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeaShooter : Plant
{
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float attackRange;
    public float projectileSpeed = 20f;
    public float attackCooldown = 1f;
    public float attackDamage = 10f;
    private float lastAttackTime;
    private LineRenderer attackRangeLineRenderer;
    private GameObject closestZombie;
    protected override void OnAwake()
    {
         base.OnAwake();
        attackRangeLineRenderer = gameObject.AddComponent<LineRenderer>();
        attackRangeLineRenderer.startWidth = 0.1f;
        attackRangeLineRenderer.endWidth = 0.1f;
         attackRangeLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        attackRangeLineRenderer.startColor = Color.yellow;
         attackRangeLineRenderer.endColor = Color.yellow;
       attackRangeLineRenderer.positionCount = 0;
     }
    public override void Attack()
    {
          if (Time.time - lastAttackTime > attackCooldown)
        {
           DrawAttackArea();
           List<GameObject> zombiesInRange = GetZombiesInArea(attackRange);
           closestZombie = GetClosestZombie(zombiesInRange);
            ClearAttackArea();
           if (closestZombie != null)
            {
                 anima.Play("Attack");
                lastAttackTime = Time.time;
            }
         }
     }
     private GameObject GetClosestZombie(List<GameObject> zombies)
    {
        if (zombies == null || zombies.Count == 0) return null;
          return zombies[0];
     }
    private void ShootProjectile()
     {
        if (projectilePrefab == null || projectileSpawnPoint == null) return;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
         if (projectileComponent != null)
         {
            projectileComponent.SetTarget(closestZombie.transform);
            projectileComponent.SetDamage(attackDamage);
           projectileComponent.SetSpeed(projectileSpeed);
         }
    }
    private void DrawAttackArea()
    {
        int segments = 360;
        attackRangeLineRenderer.positionCount = segments + 1;
       float radius = attackRange * gridManager.cellSize; // Radius in world units

       for (int i = 0; i <= segments; i++)
        {
           float angle = i * Mathf.Deg2Rad * (360f / segments);
            Vector3 position = transform.position + new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);
           attackRangeLineRenderer.SetPosition(i, position);
        }
     }
    private void ClearAttackArea()
    {
         attackRangeLineRenderer.positionCount = 0;
     }
   protected override void Die()
    {
        base.Die();
         (int x, int y) gridPosition = gridManager.GetGridPosition(transform.position);
         if (objectPlacer.placedPlants.ContainsKey(gridPosition))
         {
             objectPlacer.placedPlants.Remove(gridPosition);
        }
    }
}