using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Zombie : MonoBehaviour
{
     public float speed = 1.0f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.0f;
     public float health = 100f;
    public float finishLine = 0f;
     public float attackRange = 1.0f; // Радиус атаки зомби (в клетках)
    private Transform farmer;
    private float lastAttackTime;
    private bool reachedFinish = false;
   private bool isAttacking = false;
    private GameObject currentPlantTarget;
    private Dictionary<(int, int), GameObject> placedPlants;
    private GridManager gridManager;
    private ObjectPlacer objectPlacer;
    private Animator anima;
    public TMP_Text tooltype;
    public Canvas canvas;

    public LayerMask plantLayer;
   void OnEnable()
    {
         EventBus.OnZombieDamaged += TakeDamageEvent;
        EventBus.OnZombieDied += HandleZombieDied;
        EventBus.OnGameExit += ExitGame;
    }
    void OnDisable()
    {
         EventBus.OnZombieDamaged -= TakeDamageEvent;
         EventBus.OnZombieDied -= HandleZombieDied;
         EventBus.OnGameExit -= ExitGame;
    }

    private void Awake()
    {
     anima = GetComponent<Animator>();
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
         finishLine = farmer.transform.position.y;
    }
     void Update()
    {
       if (farmer == null) return;
        if (!isAttacking)
        {
           Vector3 moveDirection = new Vector3(0, finishLine - transform.position.y, 0);
           transform.Translate(moveDirection.normalized * speed * Time.deltaTime);

          if (transform.position.y <= finishLine)
            {
               reachedFinish = true;
             }
          CheckForPlant();
        }
    }
    void CheckForPlant()
    {
       if (isAttacking) return;

       Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange * gridManager.cellSize, plantLayer);

        if (colliders.Length > 0)
       {
            foreach(var collider in colliders)
             {
                 if (collider.GetComponent<Plant>() != null)
                {
                  AttackPlant(collider.gameObject);
                   return;
                }
            }
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
       if (!isAttacking)
        {
           isAttacking = true;
           currentPlantTarget = target;
           StartCoroutine(AttackRoutine(target));
        }
   }
   IEnumerator AttackRoutine(GameObject target)
     {
         while (target != null && currentPlantTarget == target)
        {
            if (Time.time - lastAttackTime > attackCooldown)
            {
                 Plant plant = target.GetComponent<Plant>();
                if (plant != null)
               {
                    plant.TakeDamageEvent(plant.gameObject, attackDamage);
                    lastAttackTime = Time.time;
                    anima.Play("Attack");
                }
           }
            yield return null;
         }
       isAttacking = false;
       anima.Play("Run");
      currentPlantTarget = null;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        TMP_Text T = Instantiate(tooltype, transform.position, Quaternion.identity,canvas.transform);
        T.text = damage.ToString();
        T.color = Color.red;
         if (health <= 0)
        {
            Die();
        }
    }
    public virtual void TakeDamageEvent(GameObject zombie, float damage)
   {
       if (zombie == gameObject)
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
        if (zombie == gameObject)
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

   public void ExitGame()
   {
    Destroy(gameObject);
   }
}