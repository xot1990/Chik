using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 9;  // Ширина сетки
    public int height = 5; // Высота сетки
    public float cellSize = 1.0f; // Размер ячейки
    public GameObject grassTilePrefab; // Префаб для плитки травы
    public GameObject borderTilePrefab; // Префаб для плитки границы

    private GameObject[,] grid;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Создаем клетку травы
                Vector3 cellPosition = new Vector3(x * cellSize, 0, y * cellSize);
                GameObject cell = Instantiate(grassTilePrefab, cellPosition, Quaternion.identity, transform);
                grid[x, y] = cell;
                cell.name = $"Cell_{x}_{y}";
                
                // Добавляем границы на краях
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    // Создаем клетку границы
                    GameObject borderCell = Instantiate(borderTilePrefab, cellPosition, Quaternion.identity, transform);
                    borderCell.name = $"Border_{x}_{y}";
                }

            }
        }
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return new Vector3(x * cellSize, 0, y * cellSize);
        }
        else
        {
            return Vector3.zero; // Возвращает нулевой вектор, если координаты невалидны
        }
    }

    public (int, int) GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int y = Mathf.RoundToInt(worldPosition.z / cellSize);
        
        return (x, y);
    }
}
