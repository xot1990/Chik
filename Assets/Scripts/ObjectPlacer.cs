using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ObjectPlacer : MonoBehaviour
{
    public float startSun = 100f;
    public float startReactives = 100f;
    public List<PlantData> plantDataList = new List<PlantData>();
    public Transform plantMenuParent;
    public GameObject plantButtonPrefab;
    public TMP_Text sunText;
    public TMP_Text reactiveText;
    public GameObject restartButtonPrefab;
    public GameObject endButtonPrefab;
    public Transform buttonsParent;
    public LayerMask plantLayers;
    public LayerMask uiLayers;
    public GameObject notEnoughResourcesPanel;


    private GridManager gridManager;
    public Dictionary<(int, int), GameObject> placedPlants = new Dictionary<(int, int), GameObject>();
    private (int, int) selectedCell = (-1, -1);
    private GameObject plantMenu;
    private List<GameObject> plantButtons = new List<GameObject>();
    private (int, int) menuCell = (-1, -1); // координаты ячейки, где было открыто меню
    private Vector3 placementPosition;
    private PlantData selectedPlant = null;

    private GameObject restartButton;
    private GameObject endButton;
    void OnEnable()
    {
        EventBus.OnSunChange += UpdateSunText;
        EventBus.OnReactiveChange += UpdateReactiveText;
        EventBus.OnPlantPlaced += HandlePlantPlaced;
        EventBus.OnPlantSold += HandlePlantSold;
    }

    void OnDisable()
    {
        EventBus.OnSunChange -= UpdateSunText;
        EventBus.OnReactiveChange -= UpdateReactiveText;
        EventBus.OnPlantPlaced -= HandlePlantPlaced;
        EventBus.OnPlantSold -= HandlePlantSold;
    }

    void Start()
    {
        ResourceManager.InitializeResources(startSun, startReactives);
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

        plantMenu = new GameObject("PlantMenu");
        plantMenu.transform.SetParent(plantMenuParent);
        plantMenu.SetActive(false);
        plantMenu.transform.localScale = Vector3.one;

        // Add Vertical Layout Group
        VerticalLayoutGroup verticalLayoutGroup = plantMenu.AddComponent<VerticalLayoutGroup>();
        verticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        verticalLayoutGroup.childForceExpandHeight = false;
        verticalLayoutGroup.childForceExpandWidth = false;

        // Add Content Size Fitter
        ContentSizeFitter contentSizeFitter = plantMenu.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;


        CreatePlantMenuButtons();

        if (sunText == null)
        {
            Debug.LogError("Не найден Text для отображения количества солнца!");
            enabled = false;
        }
        else
        {
            UpdateSunText(ResourceManager.Sun);
        }
        if (reactiveText == null)
        {
            Debug.LogError("Не найден Text для отображения количества реактивов!");
            enabled = false;
        }
        else
        {
            UpdateReactiveText(ResourceManager.Reactives);
        }
        restartButton = Instantiate(restartButtonPrefab, buttonsParent);
        Button restartButtonComponent = restartButton.GetComponent<Button>();
        restartButtonComponent.onClick.AddListener(RestartLevel);

        endButton = Instantiate(endButtonPrefab, buttonsParent);
        Button endButtonComponent = endButton.GetComponent<Button>();
        endButtonComponent.onClick.AddListener(EndLevel);
    }
    void CreatePlantMenuButtons()
    {
          foreach (var plantData in plantDataList)
        {
            GameObject button = Instantiate(plantButtonPrefab, plantMenu.transform);
            Button buttonComponent = button.GetComponent<Button>();

            Transform textContainer = button.transform.Find("TextContainer");
            Transform plantName = textContainer.Find("PlantNameText");
            TMP_Text plantNameText = plantName.GetComponent<TMP_Text>();

            Transform costContainer = button.transform.Find("CostContainer");
            Transform sunCost = costContainer.Find("SunCost");
            Transform sunTextTransform = sunCost.Find("Text");
            TMP_Text sunCostText = sunTextTransform.GetComponent<TMP_Text>();

            Transform reactCost = costContainer.Find("ReactCost");
            Transform reactTextTransform = reactCost.Find("Text");
            TMP_Text reactCostText = reactTextTransform.GetComponent<TMP_Text>();

            if (plantNameText)
            {
                plantNameText.text = plantData.plantName;
            }
            if (sunCostText)
            {
                sunCostText.text = plantData.placementCost.ToString();
            }
            if (reactCostText)
            {
               reactCostText.text = plantData.reactivePlacementCost.ToString();
            }
            buttonComponent.onClick.AddListener(() => SelectPlantButton(plantData));
            plantButtons.Add(button);
        }
    }
    void SelectPlantButton(PlantData plantData)
    {
        if (selectedCell == (-1,-1)) return;

        if (ResourceManager.TrySpendSun(plantData.placementCost) && ResourceManager.TrySpendReactives(plantData.reactivePlacementCost))
        {
             selectedPlant = plantData;
             PlacePlant();
              Debug.Log($"Выбрано растение: {plantData.plantName}");
        }
        else
        {
           notEnoughResourcesPanel.SetActive(true);
            Invoke("HideNotification", 1f);
        }
         // CloseMenu();
    }
    void HideNotification()
        {
            notEnoughResourcesPanel.SetActive(false);
        }

    void Update()
    {
        if (sunText == null) return;
        if (reactiveText == null) return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                SelectCell(touch.position);
            }
        }
    }

    void SelectCell(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, uiLayers);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("PlantButton"))
            {
                return;
            }
        }

          hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, plantLayers);
        if (hit.collider != null)
        {
            (int x, int y) gridPosition = gridManager.GetGridPosition(hit.point);
             if (gridPosition.x < 0 || gridPosition.x >= gridManager.width || gridPosition.y < 0 || gridPosition.y >= gridManager.height)
             {
                 Debug.Log("Невозможно выбрать объект за границами сетки");
                 CloseMenu();
                 return;
             }
              menuCell = gridPosition;
             selectedCell = gridPosition;
             placementPosition = gridManager.GetWorldPosition(gridPosition.x, gridPosition.y);

              if (placedPlants.ContainsKey(gridPosition))
             {
                 OpenSellMenu();
             }
             else
             {
                OpenPlantMenu();
             }
         }
         else
        {
           CloseMenu();
        }
    }


    void OpenPlantMenu()
    {
        CloseMenu();
         plantMenu.transform.position = placementPosition;
         plantMenu.SetActive(true);
        foreach (var button in plantButtons)
         {
            button.SetActive(true);
         }
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
            GameObject plant = placedPlants[selectedCell];
           EventBus.RaiseOnPlantSold(plant);
            ResourceManager.AddSun(plantDataList.Find(x => x.plantPrefab == placedPlants[selectedCell].gameObject).sellCost);
            Destroy(placedPlants[selectedCell]);
             placedPlants.Remove(selectedCell);
              CloseMenu();
        }
     }
      void HandlePlantPlaced(GameObject plant, Vector3 position)
      {
           Debug.Log("Растение размещено!");
      }
    void HandlePlantSold(GameObject plant)
    {
          Debug.Log("Растение продано!");
     }
   void PlacePlant()
    {
        if (selectedPlant == null)
        {
            Debug.Log("Выберите растение");
            return;
        }

       if (!placedPlants.ContainsKey(menuCell))
        {
             GameObject plant = Instantiate(selectedPlant.plantPrefab, placementPosition, Quaternion.identity);
            placedPlants.Add(menuCell, plant);
             EventBus.RaiseOnPlantPlaced(plant, placementPosition);
             selectedPlant = null;
              CloseMenu();
         }
       else
        {
             Debug.Log("Уже есть растение в этой клетке!");
            selectedPlant = null;
             CloseMenu();
        }
    }

    void CloseMenu()
    {
        plantMenu.SetActive(false);
        foreach (var button in plantButtons)
        {
           button.SetActive(false);
        }
    }
     void UpdateSunText(float sun)
    {
        sunText.text = $"Sun: {sun}";
    }
    void UpdateReactiveText(float reactive)
    {
        reactiveText.text = $"Reactives: {reactive}";
    }
     public void RestartLevel()
    {
        EventBus.RaiseOnLevelRestart();
         foreach (var plant in placedPlants.Values)
        {
            if (plant != null)
            {
               plant.GetComponent<Plant>().OnLevelEnd();
            }
        }
        placedPlants.Clear();
        ResourceManager.InitializeResources(startSun, startReactives);
     }
     public void EndLevel()
    {
       foreach (var plant in placedPlants.Values)
        {
            if (plant != null)
           {
                 plant.GetComponent<Plant>().OnLevelEnd();
           }
        }
         placedPlants.Clear();
          EventBus.RaiseOnLevelEnd(true);
         Debug.Log("Уровень окончен");
    }
}