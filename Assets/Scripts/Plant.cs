using UnityEngine;

public abstract class Plant : MonoBehaviour
{
    public float health = 100f;
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public abstract void OnLevelEnd(); // Абстрактный метод для очистки при окончании уровня
}