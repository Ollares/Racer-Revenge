using System;
using System.Collections;
using System.Collections.Generic;
//using EditorAttributes;
using PoolSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public enum BoostTypeBot
{
    None,
    Player,
    Unit
}
[Serializable]
public class BoostData
{
    public float valueModifier;
}
public class Boost : MonoBehaviour
{
    
    public BoostData boostData;
    public BoostTypeBot boostTypeBot;
    [SerializeField] Collider collider;
    [SerializeField] GameObject visual;
    [SerializeField] bool canBlocked = false;
    [SerializeField] bool deactivateAround = false;

    [SerializeField] float radius = 3f;
    [SerializeField] LayerMask layer;
    [HideInInspector] public bool isUnblocked = false;
    [SerializeField] bool CanRespawn = false;
    //[ShowField(nameof(CanRespawn))]
    [SerializeField] float duration = 10f;
    float currentDuration = 0;

    [SerializeField] VfxObject vfxObject;
    
    public bool isDeactivate;
    private void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        if(isDeactivate)
            return;
        currentDuration = 0;
        EnableVisual(true);
        EnableInteraction(true);
        //gameObject.SetActive(true);
        if(deactivateAround)
            DeactivateCutObjectAround();
        if (canBlocked)
            updateCoroutine = StartCoroutine(UpdateRoutine());
    }
    public void Disable()
    {
        var vfx = vfxObject;
        if (vfx)
        {
            vfx.transform.SetParent(transform.parent);
            vfx.transform.position = visual.transform.position;
            vfx.canReturn = false;
            vfx.Inititalize();
            vfx.Play();
        }
        Deactive();
        if(CanRespawn)
            StartCoroutine(TimerRoutine());
        //gameObject.SetActive(false);
        //Debug.Log("IS TAKE");
    }
    public void Deactive()
    {
        isDeactivate = true;
        EnableVisual(false);
        EnableInteraction(false);

        //Debug.Log("DEACTIVE");
    }
    void EnableVisual(bool value)
    {
        if(visual)
            visual.gameObject.SetActive(value);
    }
    void EnableInteraction(bool value)
    {
        collider.enabled = value;
    }
    Coroutine updateCoroutine;
    IEnumerator UpdateRoutine()
    {
        //Debug.Log("Initialize");
        isUnblocked = false;
        EnableVisual(isUnblocked);
        EnableInteraction(isUnblocked);
        yield return new WaitForSeconds(0.5f);
        UpdateCutObjectMaterial();
        while (!isUnblocked)
        {
            var detectObject = Physics.OverlapSphere(transform.position, radius, layer);
            if (detectObject.Length == 0)
            {
                isUnblocked = true;
            }
            yield return null;
        }
        EnableVisual(isUnblocked);
        EnableInteraction(isUnblocked);
    }
    IEnumerator TimerRoutine()
    {
        while (currentDuration < duration)
        {
            currentDuration += Time.deltaTime;
            yield return null;
        }
        isDeactivate = false;
        Initialize();
    }
    void UpdateCutObjectMaterial()
    {
        var detectObject = Physics.OverlapSphere(transform.position, radius, layer);
        if (detectObject.Length > 0)
        {
            foreach (var detect in detectObject)
                if (detect)
                {
                    // var cut = detect.GetComponent<CutObject>();
                    // if(cut)
                    //     cut.SetBlockMaterial();
                }
        }
    }
    void DeactivateCutObjectAround()
    {
        StartCoroutine(DelayDeactCutObjectAround());
    }
    IEnumerator DelayDeactCutObjectAround()
    {
        yield return new WaitForSeconds(0.15f);

        var detectObject = Physics.OverlapSphere(transform.position, radius, layer);
        if (detectObject.Length > 0)
        {
            foreach (var detect in detectObject)
                if (detect)
                {
                    // var cut = detect.GetComponent<CutObject>();
                    // if(cut)
                    //     cut.Deactivate();
                }
        }
    }
    private void OnDrawGizmos()
    {
        if (canBlocked == false && deactivateAround == false)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
