using System;
using System.Collections;
using System.Collections.Generic;
using PoolSystem;
using UnityEngine;
using UnityEngine.Events;

public class VfxObject : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    public VfxType Type;
    [HideInInspector] public float duration = 0f;
    float currentDuration = 0f;
    private bool isDisable = false;
    [HideInInspector] public bool canReturn = true;
    Vector3 initScale;
    Quaternion initRotation;
    public UnityEvent OnStop;
    private void Awake()
    {
        initScale = transform.localScale;
        initRotation = transform.rotation;
        _particleSystem = GetComponent<ParticleSystem>();
    }
    public void Inititalize()
    {
        transform.localScale = initScale;
        transform.rotation = initRotation;
        currentDuration = 0f;
        isDisable = false;
        gameObject.SetActive(true);
        _particleSystem.Play(true);
    }
    public void Play()
    {   
        Duration();
    }

    void Duration()
    {
        StopDuration();
        vfxCoroutine = StartCoroutine(VfxDuration());
    }

    void StopDuration()
    {
        if (vfxCoroutine != null)
            StopCoroutine(vfxCoroutine);
    }
    private Coroutine vfxCoroutine;
    IEnumerator VfxDuration()
    {
        yield return null;
        while (true)
        {
            if(duration > 0)
            {
                if(currentDuration >= duration)
                    break;
            }
            currentDuration += Time.deltaTime;
            if (_particleSystem.isStopped)
                break;
            
            yield return null;
        }

        Stop();
    }
    public void Stop()
    {
        if(isDisable)
            return;
            duration = 0f;
        isDisable = true;
        StopDuration();
        OnStop?.Invoke();
        _particleSystem.Stop(true);
        gameObject.SetActive(false);
        Return();
    }
    void Return()
    {
        if(canReturn)
        {
            PoolController.Instance.ReturnVfx(this);
        }
    }
}
