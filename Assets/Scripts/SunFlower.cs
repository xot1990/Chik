using TMPro;
using UnityEngine;

public class SunFlower : Plant
{
    public float sunGenerationRate = 5f;
    public float generationInterval = 5f;
    private float lastGenerationTime;
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
            ResourceManager.AddSun(sunGenerationRate);
            lastGenerationTime = Time.time;
            TMP_Text T = Instantiate(resourseText, transform.position, Quaternion.identity,canvas.transform);
            T.text = "+" + sunGenerationRate;
            T.color = Color.white;
        }
    }

    public override void OnLevelEnd()
    {
        Destroy(gameObject);
    }

}