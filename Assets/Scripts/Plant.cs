using UnityEngine;

public abstract class Plant : MonoBehaviour
{
    public float health = 100f;
    void OnEnable()
    {
        EventBus.OnPlantDamaged += TakeDamageEvent;
    }
    void OnDisable()
    {
        EventBus.OnPlantDamaged -= TakeDamageEvent;
    }
    public virtual void TakeDamage(float damage)
    {
        EventBus.RaiseOnPlantDamaged(gameObject, damage);
        if (health <= 0)
        {
            Die();
        }
    }
    public virtual void TakeDamageEvent(GameObject plant, float damage)
    {
        if(plant == gameObject)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }
    protected virtual void Die()
    {
        EventBus.RaiseOnPlantSold(gameObject);
        Destroy(gameObject);
    }

    public abstract void OnLevelEnd(); // Абстрактный метод для очистки при окончании уровня
}