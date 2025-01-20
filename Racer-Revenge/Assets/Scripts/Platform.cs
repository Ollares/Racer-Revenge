using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform startPointPlatform;
    [SerializeField] private Transform endPointPlatform;
    
    public Transform StartPointPlatform => startPointPlatform;
    public Transform EndPointPlatform => endPointPlatform;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(startPointPlatform.position, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endPointPlatform.position, 0.5f);
    }
}
