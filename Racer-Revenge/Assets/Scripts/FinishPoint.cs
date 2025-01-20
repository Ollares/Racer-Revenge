using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameCore.Instance.Level.finishPoint = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
