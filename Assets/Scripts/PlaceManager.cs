using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    public float startSun = 100f;
    public List<PlantData> plantDataList = new List<PlantData>();
    public Transform plantMenuParent;
    public GameObject plantButtonPrefab;
    public TMP_Text sunText;
    public GameObject restartButtonPrefab;
    public GameObject endButtonPrefab;
     public Transform buttonsParent;


    private GridManager gridManager;
    public Dictionary<(int, int), GameObject> placedPlants = new Dictionary<(int, int), GameObject>();
    private (int, int) selectedCell = (-1, -1);
    private GameObject plantMenu;
    private Vector3 placementPosition;
     private PlantData selectedPlant = null;

    private GameObject restartButton;
    private GameObject endButton;

    void Start()
    {
        ResourceManager.InitializeSun(startSun);
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("Не найден GridManager в сцене!");
            enabled = false;
        }

        if (plantMenuParent == null)
        {
            Debug.LogError("Не найден Transform для меню растений!");
            enabled = false;
        }
        if (buttonsParent == null)
        {
            Debug.LogError("Не найден Transform для кнопок управления!");
            enabled = false;
        }
        // Создаем меню с вариантами растений (скрытое)
        plantMenu = new GameObject("PlantMenu");
        plantMenu.transform.SetParent(plantMenuParent);
        plantMenu.SetActive(false);

       foreach (var plantData in plantDataList)
        {
             GameObject button = Instantiate(plantButtonPrefab, plantMenu.transform);
            // Настраиваем кнопку

           Button buttonComponent = button.GetComponent<Button>();
           TMP_Text textComponent = button.GetComponentInChildren<TMP_Text>(); // Получаем текст внутри кнопки

            if (textComponent)
            {
                textComponent.text = plantData.plantName; // Выводим имя префаба
            }

            buttonComponent.onClick.AddListener(() => SelectPlant(plantData)); // Добавляем функцию, когда кнопка нажимается
         }


           if (sunText == null)
        {
            Debug.LogError("Не найден Text для отображения количества солнца!");
            enabled = false;
        }
        else
        {
            UpdateSunText();
        }
            // Создаем кнопки перезапуска и конца уровня
         restartButton = Instantiate(restartButtonPrefab, buttonsParent);
          Button restartButtonComponent = restartButton.GetComponent<Button>();
           restartButtonComponent.onClick.AddListener(RestartLevel);

           endButton = Instantiate(endButtonPrefab, buttonsParent);
           Button endButtonComponent = endButton.GetComponent<Button>();
           endButtonComponent.onClick.AddListener(EndLevel);
    }
    void SelectPlant(PlantData plantData)
    {
        selectedPlant = plantData;
        Debug.Log($"Выбрано растение: {plantData.plantName}");
    }

    void Update()
    {
         if (sunText == null) return;
          UpdateSunText();
        // Проверяем касание экрана
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Получаем первое касание

            if (touch.phase == TouchPhase.Began) // Проверяем начало касания
           {
               SelectCell(touch.position);
           }
        }
    }

    void SelectCell(Vector2 touchPosition)
    {
        // Создаем луч от камеры из позиции касания
       Ray ray = Camera.main.ScreenPointToRay(touchPosition);
       RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
           (int x, int y) gridPosition = gridManager.GetGridPosition(hit.point);

             if (gridPosition.x < 0 || gridPosition.x >= gridManager.width || gridPosition.y < 0 || gridPosition.y >= gridManager.height)
            {
                Debug.Log("Невозможно выбрать объект за границами сетки");
                CloseMenu();
                return;
            }

            selectedCell = gridPosition;
             placementPosition = gridManager.GetWorldPosition(gridPosition.x, gridPosition.y);
            // Проверяем есть ли на клетке растение
           if (placedPlants.ContainsKey(gridPosition))
           {
             // Открываем меню продажи
               OpenSellMenu();
           } else {
             // открываем меню выбора растения
               OpenPlantMenu();
           }
        } else
        {
            CloseMenu();
        }
    }
    void OpenPlantMenu()
    {
      CloseMenu();
      plantMenu.transform.position = placementPosition;
      plantMenu.SetActive(true);

    }

     void OpenSellMenu()
    {
        CloseMenu();
          GameObject sellButton = Instantiate(plantButtonPrefab, plantMenu.transform);
          Button sellButtonComponent = sellButton.GetComponent<Button>();
           TMP_Text sellTextComponent = sellButton.GetComponentInChildren<TMP_Text>();
            if (sellTextComponent)
            {
                 sellTextComponent.text = "Sell Plant";
            }
              sellButtonComponent.onClick.AddListener(SellPlant);
      plantMenu.transform.position = placementPosition;
      plantMenu.SetActive(true);
    }
     void SellPlant()
    {
        if (placedPlants.ContainsKey(selectedCell))
            {
              ResourceManager.AddSun(plantDataList.Find(x => x.plantPrefab == placedPlants[selectedCell].gameObject).sellCost);
                 Destroy(placedPlants[selectedCell]);
                 placedPlants.Remove(selectedCell);
                CloseMenu();
           }
    }
    void PlacePlant()
    {
       if (selectedPlant == null)
       {
          Debug.Log("Выберите растение");
            return;
       }

        if (ResourceManager.TrySpendSun(selectedPlant.placementCost))
         {
           if (!placedPlants.ContainsKey(selectedCell))
            {
             GameObject plant = Instantiate(selectedPlant.plantPrefab, placementPosition, Quaternion.identity);
               placedPlants.Add(selectedCell, plant);
                CloseMenu();
               selectedPlant = null;
           } else
           {
             Debug.Log("Уже есть растение в этой клетке!");
           }
       } else
       {
            Debug.Log("Недостаточно солнца!");
       }
   }
    void CloseMenu()
    {
        plantMenu.SetActive(false);

         // Удаляем все дочерние обьекты у меню (т.к. меню продажи их создает каждый раз при вызове)
         foreach (Transform child in plantMenu.transform)
          {
            Destroy(child.gameObject);
        }
    }
      void UpdateSunText()
    {
      sunText.text = $"Sun: {ResourceManager.Sun}";
    }

    public void RestartLevel()
    {
        // Очищаем все растения
        foreach (var plant in placedPlants.Values)
         {
           if(plant != null)
           {
            plant.GetComponent<Plant>().OnLevelEnd();
           }
        }
        placedPlants.Clear();

        // Сбрасываем ресурсы (можно и другие параметры)
        ResourceManager.InitializeSun(startSun);
        UpdateSunText();
    }
     public void EndLevel()
    {
         // Очищаем все растения
        foreach (var plant in placedPlants.Values)
           {
             if (plant != null)
             {
             plant.GetComponent<Plant>().OnLevelEnd();
             }
         }
          placedPlants.Clear();

        // Дополнительные действия при завершении уровня
         Debug.Log("Уровень окончен");
    }
}