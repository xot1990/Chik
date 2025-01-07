using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 9;  // Ширина сетки
    public int height = 5; // Высота сетки
    public float cellSize = 1.0f; // Размер ячейки
    public GameObject grassTilePrefab; // Префаб для плитки травы

    private GameObject[,] grid;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new GameObject[width, height];

        // Вычисляем начальную позицию смещения для центрирования сетки
        float startX = -((float)width / 2) * cellSize + cellSize / 2;
        float startY = -((float)height / 2) * cellSize + cellSize / 2;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Создаем клетку травы
                Vector3 cellPosition = new Vector3(startX + x * cellSize, startY + y * cellSize, 0);
                GameObject cell = Instantiate(grassTilePrefab, cellPosition, Quaternion.identity, transform);
                grid[x, y] = cell;
                cell.name = $"Cell_{x}_{y}";
            }
        }
    }
     public Vector3 GetWorldPosition(int x, int y)
    {
        // Вычисляем начальную позицию смещения для центрирования сетки
        float startX = -((float)width / 2) * cellSize + cellSize / 2;
        float startY = -((float)height / 2) * cellSize + cellSize / 2;

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return new Vector3(startX + x * cellSize, startY + y * cellSize, 0);
        }
        else
        {
            return Vector3.zero;
        }
    }

    public (int, int) GetGridPosition(Vector3 worldPosition)
    {
        // Вычисляем начальную позицию смещения для центрирования сетки
        float startX = -((float)width / 2) * cellSize + cellSize / 2;
        float startY = -((float)height / 2) * cellSize + cellSize / 2;

        int x = Mathf.RoundToInt((worldPosition.x - startX) / cellSize);
        int y = Mathf.RoundToInt((worldPosition.y - startY) / cellSize);

        return (x, y);
    }
}