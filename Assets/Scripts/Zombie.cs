using UnityEngine;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
    public float speed = 1.0f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.0f;
    public float health = 100f;
    public float finishLine = 0f;
    private Transform farmer;
    private float lastAttackTime;
    private bool reachedFinish = false;

    private Dictionary<(int, int), GameObject> placedPlants;
    private GridManager gridManager;
    private ObjectPlacer objectPlacer;

      void OnEnable()
    {
        EventBus.OnZombieDamaged += TakeDamageEvent;
          EventBus.OnZombieDied += HandleZombieDied;
     }
    void OnDisable()
    {
        EventBus.OnZombieDamaged -= TakeDamageEvent;
         EventBus.OnZombieDied -= HandleZombieDied;
    }
    void Start()
    {
        farmer = GameObject.FindGameObjectWithTag("Farmer").transform;
        if (farmer == null)
        {
            Debug.LogError("Не найден фермер с тегом 'Farmer' в сцене!");
            enabled = false;
        }
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("Не найден GridManager в сцене!");
            enabled = false;
        }
        objectPlacer = FindObjectOfType<ObjectPlacer>();
        if (objectPlacer == null)
        {
            Debug.LogError("Не найден ObjectPlacer в сцене!");
            enabled = false;
        }
        placedPlants = objectPlacer.GetComponent<ObjectPlacer>().placedPlants;
        finishLine = farmer.transform.position.x;
    }

    void Update()
    {
        if (farmer == null) return;
        Vector3 moveDirection = new Vector3(finishLine - transform.position.x, 0, 0);
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
        if (transform.position.x <= finishLine)
        {
            reachedFinish = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plant"))
        {
            (int x, int y) gridPosition = gridManager.GetGridPosition(transform.position);
            if (placedPlants.ContainsKey(gridPosition))
            {
                AttackPlant(placedPlants[gridPosition]);
                return;
            }
        }
        if (other.CompareTag("Farmer") && reachedFinish)
        {
            AttackFarmer(other.gameObject);
            return;
        }
    }

    void AttackFarmer(GameObject target)
    {
        Debug.Log("Атака фермера");
        EventBus.RaiseOnGameOver();
        Die();
    }

    void AttackPlant(GameObject target)
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            Plant plant = target.GetComponent<Plant>();
            if (plant != null)
            {
                plant.TakeDamage(attackDamage);
                lastAttackTime = Time.time;
            }
            if (target == null)
            {
                (int x, int y) gridPosition = gridManager.GetGridPosition(transform.position);
                placedPlants.Remove(gridPosition);
            }
        }
    }
      public void TakeDamage(float damage)
    {
        health -= damage;
        EventBus.RaiseOnZombieDamaged(gameObject, damage);
          if (health <= 0)
            {
                Die();
            }
    }
     public virtual void TakeDamageEvent(GameObject zombie, float damage)
    {
         if(zombie == gameObject)
            {
             health -= damage;
               if (health <= 0)
                 {
                     Die();
                  }
            }
    }
      void HandleZombieDied(GameObject zombie)
    {
          if(zombie == gameObject)
          {
                Debug.Log("Зомби погиб");
          }
    }
    void Die()
    {
        EventBus.RaiseOnZombieDied(gameObject);
        Destroy(gameObject);
        if (reachedFinish)
        {
            EndLevel(false);
        }
    }

    public void EndLevel(bool win)
    {
          EventBus.RaiseOnLevelEnd(win);
          if (win)
         {
            Debug.Log("Уровень пройден!");
        }
        else
         {
            Debug.Log("Уровень не пройден!");
        }
        ObjectPlacer objectPlacer = FindObjectOfType<ObjectPlacer>();
        objectPlacer.EndLevel();
    }
}