using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum AmmoType
{
    None,
    Grenade,
    Bullet,
    Missile,
    Shell,
    Flame
}
public class WeaponData : MonoBehaviour
{
    [Serializable]
    public class Stats
    {
        public float damage;
        public float speed;
        //public float range;
        public float rate;
        
        public void Copy(Stats other)
        {
            damage = other.damage;
            speed = other.speed;
            //range = other.range;
            rate = other.rate;
        }
    }
    //[Serializable]
    // public class SpreadStat
    // {
    //     public int countProjectile = 1;
    //     public float angleP = 150f;
    //     public float offsetX = 0.7f;
    // }
    public Stats _baseStats = new Stats();
    public Stats _stats = new Stats();
    //public SpreadStat spreadStat = new SpreadStat();
    public AmmoType ammoType;
    public VfxType vfxTypeMuzzle;
    public VfxType vfxTypeHit;
    
    float maxDamage = 0f;
    float maxRate = 0f;

    Weapon baseWeapon;
    public void Initialize(Weapon weapon)
    {
        baseWeapon = weapon;
        _stats.Copy(_baseStats);
        maxDamage = _stats.damage;
        maxRate = _stats.rate;
    }
    public void Attack(BaseController target, BaseController source)
    {
        AttackData attackData = new AttackData(source);
        attackData.AddDamage(_stats.damage);
        target.botData.Damage(attackData);
    }
    
}
