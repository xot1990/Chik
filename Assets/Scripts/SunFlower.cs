using UnityEngine;

public class SunFlower : Plant
{
    public float sunGenerationRate = 5f;
    public float generationInterval = 5f;
    private float lastGenerationTime;

    void Start()
    {
        lastGenerationTime = Time.time;
    }

    void Update()
    {
        if (Time.time - lastGenerationTime >= generationInterval)
        {
            ResourceManager.AddSun(sunGenerationRate);
            lastGenerationTime = Time.time;
        }
    }

    public override void OnLevelEnd()
    {
        Destroy(gameObject);
    }

}