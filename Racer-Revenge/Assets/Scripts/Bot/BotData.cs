using System;
using UnityEngine;
using Random = UnityEngine.Random;
public enum StatType
{
    None,
    Speed,
    Health,
    CountUnit,
    Strength
}
public class BotData : MonoBehaviour
{

    [Serializable]
    public class Stats
    {
        public float health;
        public float moveSpeed;
        public float attackSpeed;
        public float rangeAttack;
        public float strength;
        
        public void Copy(Stats other)
        {
            health = other.health;
            moveSpeed = other.moveSpeed;
            strength = other.strength;
            attackSpeed = other.attackSpeed;
            rangeAttack = other.rangeAttack;
        }
    }
    [Serializable]
    public class AnimationComponent
    {
        public AttackType attackType;
    }
    public Stats _baseStats = new Stats();
    public Stats _stats = new Stats();
    
    public AnimationComponent _animationComponent = new AnimationComponent();
    
    private BaseController _baseController;
    private float maxHealth;
    private float maxMoveSpeed;
    private float maxStrength;
    private bool isStun = false;
    private bool isShield = false;
    private bool isMoveSpeed = false;

    float damageModifier = 0;
    float moveSpeedModifier = 0;
    public void Initialize(BaseController source)
    {
        _baseController = source;
        _stats.Copy(_baseStats);

        if(Random.value > 0.5f)
            _stats.health -= _stats.health / 2;
        
        maxHealth = _stats.health;
        maxMoveSpeed = _stats.moveSpeed;
        maxStrength = _stats.strength;
        
        UpgradeMoveSpeed((_stats.moveSpeed * moveSpeedModifier) / 100f);
    }
    #region UPGRADE STAT
    public void UpgradeHealth(float value)
    {
        _stats.health += value;
        maxHealth = _stats.health;
    }
    public void UpgradeMoveSpeed(float value)
    {
        _stats.moveSpeed += value;
        maxMoveSpeed = _stats.moveSpeed;
    }
    public void UpgradeStrength(float value)
    {
        _stats.strength += value;
        maxStrength = _stats.strength;
    }
    #endregion

    public void UpdateDamagerModifier(float value)
    {
        damageModifier = value;
    }
    public void UpdateMoveSpeedModifier(float value)
    {
        moveSpeedModifier = value;
    }
    public void SetStrength(float value)
    {
        _baseStats.strength = value;
    }
    public void Damage(AttackData attackData)
    {
        HapticManager.HapticMedium();
        if(isShield)
            return;
        if(_stats.health <= 0f)
            return;

        var modifier = (attackData.GetDamage() * damageModifier) / 100f;
        var updateDamage = attackData.GetDamage() - modifier;
        _baseController.HitHighlight();
        _baseController.ShakeController();
        Heal(-updateDamage);
        
        if (_stats.health <= 0f)
        {
            _baseController.Death();
            return;
        }
    }
    public void UpdateHealth()
    {
        Heal(maxHealth);
    }
    void Heal(float value)
    {
        _stats.health += value;
        _stats.health = Mathf.Clamp(_stats.health, 0f, maxHealth);
        var percent = _stats.health / maxHealth;
        ChangeHealth(percent);
    }
    void ChangeHealth(float amout)
    {
        _baseController.UpdateHealthBar(amout);
    }
}

