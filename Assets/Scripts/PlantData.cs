using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant Data")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public GameObject plantPrefab;
    public float placementCost;
    public float reactivePlacementCost; // �������� ����� ����
    public float sellCost;
    public float sellCostReact;
    public Sprite Icon;
}