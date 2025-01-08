using TMPro;
using UnityEngine;

public class ResourcePlant : Plant
{
    public float resourceGenerationRate = 2;
    public float generationInterval = 10f;
    private float lastGenerationTime;
    private float currentResource = 0;
    public TMP_Text resourseText;
    
    void Start()
    {
        lastGenerationTime = Time.time;
    }
    void Update()
    {
        if (Time.time - lastGenerationTime >= generationInterval)
        {
            anima.Play("Pik");
            ResourceManager.AddReactives(resourceGenerationRate);
            lastGenerationTime = Time.time;
            TMP_Text T = Instantiate(resourseText, transform.position, Quaternion.identity,canvas.transform);
            T.text = "+" + resourceGenerationRate;
            T.color = Color.white;
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
        ResourceManager.AddReactives(resource);
        return resource;
    }

}