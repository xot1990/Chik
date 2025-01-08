using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Plant : MonoBehaviour
{
    public string Name;
    public float health = 100f;
    public float sellCost = 10f;
    public LayerMask zombieLayer;
    protected GridManager gridManager;
    protected ObjectPlacer objectPlacer;
    protected Animator anima;
    public PlantData NextLevelTir;
    public TMP_Text tooltype;
    public Canvas canvas;
    void OnEnable()
    {
        EventBus.OnPlantDamaged += TakeDamageEvent;
    }
    void OnDisable()
    {
        EventBus.OnPlantDamaged -= TakeDamageEvent;
    }
    void Awake()
    {
        anima = GetComponent<Animator>();
        OnAwake();
    }
    protected virtual void OnAwake()
    {
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
    }
    protected virtual void Update()
    {
        Attack();
    }
    public virtual void Attack()
    {
        Debug.Log("Plant Attack");
    }
    public virtual void TakeDamageEvent(GameObject plant, float damage)
    {
        if (plant == gameObject)
        {
            TMP_Text T = Instantiate(tooltype, transform.position, Quaternion.identity,canvas.transform);
            T.text = damage.ToString();
            T.color = Color.red;
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }
    public virtual void OnLevelEnd()
    {
        if(!transform.CompareTag("Farmer"))
            Destroy(gameObject);
    }

    protected virtual void Die()
    {
        (int x, int y) gridPosition = gridManager.GetGridPosition(transform.position);
         if (objectPlacer.placedPlants.ContainsKey(gridPosition))
         {
             objectPlacer.placedPlants.Remove(gridPosition);
         }
        Destroy(gameObject);
    }
    protected bool IsTargetInCellRange(Vector3 targetPosition)
    {
        (int x, int y) targetGridPosition = gridManager.GetGridPosition(targetPosition);
        (int x, int y) selfGridPosition = gridManager.GetGridPosition(transform.position);
         return Mathf.Abs(targetGridPosition.x - selfGridPosition.x) <= 1 && Mathf.Abs(targetGridPosition.y - selfGridPosition.y) <= 1;
    }
     protected List<GameObject> GetZombiesInArea(float radius)
    {
        List<GameObject> zombies = new List<GameObject>();
         float worldRadius = radius * gridManager.cellSize;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, worldRadius, zombieLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Zombie>() != null)
            {
                zombies.Add(collider.gameObject);
            }
        }
         return zombies;
    }
}