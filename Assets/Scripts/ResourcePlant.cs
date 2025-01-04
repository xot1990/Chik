using UnityEngine;

public class ResourcePlant : Plant
{
    public float resourceGenerationRate = 2;
    public float generationInterval = 10f;
    private float lastGenerationTime;
    private float currentResource = 0;
    void Start()
    {
        lastGenerationTime = Time.time;
    }
    void Update()
    {
        if (Time.time - lastGenerationTime >= generationInterval)
        {
            currentResource += resourceGenerationRate;
            lastGenerationTime = Time.time;
            Debug.Log($"ResourcePlant generated {resourceGenerationRate} resource points. Current Resource: {currentResource}");
        }
    }
    public override void OnLevelEnd()
    {
        Destroy(gameObject);
    }
    public float GetResource()
    {
        float resource = currentResource;
        currentResource = 0;
        return resource;
    }

}