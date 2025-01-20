using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneratorPlatform : MonoBehaviour
{
    [SerializeField] private List<Platform> platforms;
    [SerializeField] Platform startPlatformPrefab;
    [SerializeField] Platform idlePlatformPrefab;
    [SerializeField] Platform endPlatformPrefab;
    public List<Platform> Platforms => platforms;
    [SerializeField] int lengthPlatforms = 10;

    private void Start() 
    {
        Initialize();
    }
    public void Initialize()
    {
        var startPlatform = SpawnPlatform(startPlatformPrefab);
        platforms.Add(startPlatform);
        for (int i = 0; i < lengthPlatforms; i++)
        {
            SetPlatform();
        }

        var endPlatform = SpawnPlatform(endPlatformPrefab);
        SetPosition(endPlatform);
        platforms.Add(endPlatform);
    }
    void SetPlatform()
    {
        var platform = SpawnPlatform(idlePlatformPrefab);
        SetPosition(platform);
        platforms.Add(platform);
    }

    void SetPosition(Platform platform)
    {
        platform.transform.position =
            platforms[platforms.Count - 1].EndPointPlatform.position - platform.StartPointPlatform.localPosition;
    }
    
    
    Platform SpawnPlatform(Platform locPlatform = null)
    {
        var platform = Instantiate(locPlatform, transform);
        return platform;
    }
}
