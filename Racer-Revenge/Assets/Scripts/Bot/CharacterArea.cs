using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterArea : MonoBehaviour
{
    [SerializeField] GameObject circle;
    [SerializeField] GameObject LinePrefab;
    [SerializeField] int countLines;
    List<GameObject> listLines = new List<GameObject>();
    float radiusLines = -5f;
    
    [SerializeField] private float rotationSpeed = 0;
    [SerializeField] private float scaleSpeed = 0;
    private Vector3 originEuler = new Vector3(0f, 0f, 0f);

    private void Awake()
    {
        InitializeLines();
    }
    void InitializeLines()
    {
        for (int i = 0; i < countLines; i++)
        {
            var line = Instantiate(LinePrefab, transform.position, Quaternion.identity, circle.transform);
            line.gameObject.SetActive(true);
            listLines.Add(line);
        }

    }
    void UpdateRadiusLine(float radius)
    {
        for (int i = 0; i < listLines.Count; i++)
        {
            float angle = i * (2 * Mathf.PI / listLines.Count);

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 pos = new Vector3(circle.transform.position.x + x, 0f, circle.transform.position.z + z);

            listLines[i].transform.position = pos;
            listLines[i].transform.rotation = Quaternion.LookRotation(circle.transform.position - listLines[i].transform.position);
        }
    }
    public void SetRadius(float value)
    {
        if(value == radiusLines)
            return; 
        radiusLines = value;
        UpdateRadiusLine(radiusLines);
    }


    void LateUpdate()
    {
        if(rotationSpeed != 0)
        {
            originEuler.y += Time.unscaledDeltaTime * rotationSpeed;
            circle.transform.rotation = Quaternion.Euler(originEuler);
        }
        // if (circle.transform.localScale != Vector3.one)
        // {
        //     if (scaleSpeed == 0)
        //     {
        //         circle.transform.localScale = Vector3.one;
        //     }
        //     else
        //     {
        //         circle.transform.localScale = Vector3.MoveTowards(circle.transform.localScale, Vector3.one, Time.unscaledDeltaTime * scaleSpeed);
        //     }
            
        // }
    }
}
