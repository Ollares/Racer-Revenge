using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData 
{
    BaseController _source;
    public BaseController Source => _source;
    float Damage = 0;

    public AttackData(BaseController source)
    {
        _source = source;
    }
    //public AttackData()
    public void AddDamage(float amount)
    {
        Damage += amount;
    }

    public float GetDamage()
    {
        return Damage;
    }
}
