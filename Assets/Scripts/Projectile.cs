using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    public float speed = 20f;
    private float damage;
    public bool rotate = true;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            if (rotate) {
                RotateTowardsTarget();
            }
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                HitTarget();
                Destroy(gameObject);
            }
        }
    }

    void HitTarget()
    {
        if (target == null) return;
        Zombie zombie = target.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombie.TakeDamage(damage);
            Debug.Log("Hit!");
        }
    }
    private void RotateTowardsTarget() {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}