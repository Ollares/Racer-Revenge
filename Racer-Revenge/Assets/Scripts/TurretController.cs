using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : BaseController
{
    [SerializeField] GameObject laserObj;
    void Start()
    {
        Initialize();
    }
    public void ActiveLaser(bool value)
    {
        if(laserObj)
            laserObj.gameObject.SetActive(value);
    }
    public override void Initialize()
    {
        SetBaseTarget(this); // set default
        base.Initialize();
    }
    public void RotateTurret(Vector3 delta)
    {
        var newRotation = Quaternion.Euler(new Vector3(0, delta.x, 0));
        transform.localRotation *= newRotation;
    }
}
