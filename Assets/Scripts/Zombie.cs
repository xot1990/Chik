using UnityEngine;
using System.Collections.Generic;

public class Zombie : MonoBehaviour
{
    public float speed = 1.0f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.0f;
    public float health = 100f;
    public float finishLine = 0f; // Позиция по X, где находится финиш
    private Transform farmer;
    private float lastAttackTime;
    private bool reachedFinish = false; // Флаг для отслеживания достижения финиша

    private Dictionary<(int, int), GameObject> placedPlants;
    private GridManager gridManager;
    private ObjectPlacer objectPlacer;


    void Start()
    {
       // Находим фермера по тегу
       farmer = GameObject.FindGameObjectWithTag("Farmer").transform;
         if (farmer == null)
         {
            Debug.LogError("Не найден фермер с тегом 'Farmer' в сцене!");
            enabled = false;
         }
        // Находим GridManager и ObjectPlacer
        gridManager = FindObjectOfType<GridManager>();
        if(gridManager == null)
        {
            Debug.LogError("Не найден GridManager в сцене!");
            enabled = false;
        }
         objectPlacer = FindObjectOfType<ObjectPlacer>();
         if (objectPlacer == null)
         {
            Debug.LogError("Не найден ObjectPlacer в сценеqq!");
             enabled = false;
         }
        placedPlants = objectPlacer.GetComponent<ObjectPlacer>().placedPlants;
        finishLine = farmer.transform.position.x; // Получаем координаты финиша от фермера

    }

    void Update()
    {
        if (farmer == null) return;

        // Направление к фермеру по оси X
       Vector3 moveDirection = new Vector3(finishLine - transform.position.x, 0, 0);
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime);

        if (transform.position.x <= finishLine)
        {
            reachedFinish = true;
        }


    }

   void OnTriggerEnter(Collider other)
   {
         if (other.CompareTag("Plant") )
        {
          // Получаем координаты ячейки
            (int x, int y) gridPosition = gridManager.GetGridPosition(transform.position);

            if (placedPlants.ContainsKey(gridPosition))
                {
                    // Атакуем растение если оно есть в словаре
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

          // Атака фермера
          Debug.Log("Атака фермера");
          // Здесь добавьте логику атаки фермера
          // Например: Destroy(target);
           Die();
     }
    void AttackPlant(GameObject target)
     {
          if (Time.time - lastAttackTime > attackCooldown)
          {
             // Атака растения
              Plant plant = target.GetComponent<Plant>();

             if (plant != null)
                {
                   plant.TakeDamage(attackDamage);
                    lastAttackTime = Time.time;
                }
                // Если у растения 0 здоровья
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
         if (health <= 0)
         {
            Die();
         }
    }

    void Die()
     {
        Destroy(gameObject);
        // Завершаем уровень если зомби дошел до фермера
          if (reachedFinish)
          {
            EndLevel(false);
          }
     }

  public void EndLevel(bool win)
    {
        // Действия при завершении уровня (можно расширить)
         if (win)
         {
              Debug.Log("Уровень пройден!");
         } else
         {
             Debug.Log("Уровень не пройден!");
         }

        ObjectPlacer objectPlacer = FindObjectOfType<ObjectPlacer>();
        objectPlacer.EndLevel(); // Очищаем поле
    }
}