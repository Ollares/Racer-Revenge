using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PoolSystem;
using UISystem;
using UnityEngine;
using UnityEngine.AI;

public enum TypeCombat
{
    StayCombat,
    DistanceCombat
}
public enum TypeAgent
{
    TransformAgent,
    NavmeshAgent
}
[RequireComponent(typeof(BotData))]
public class BaseController : MonoBehaviour
{
    public BotData botData;
    public AnimatorHandler animator;
    // [SerializeField] protected CharacterTool characterTool;
    // public CharacterTool CharacterTool => characterTool;
    [Space(5)]
    [SerializeField] private Collider colliderController;
    [SerializeField] Renderer skinned;
    [SerializeField] GameObject mainObjectShake;
    Vector3 mainObjectScale;

    [Space(5)]
    [SerializeField] protected UIWorldFillBar healthBar;
    
    [Header("Combat component")]
    [Space(5)]
    [SerializeField] TypeCombat typeCombat;
    [SerializeField] private CharacterArea characterArea;
    [SerializeField] private Transform pointCombatArea;
    [SerializeField] private CharacterWeapon characterWeapon;
    public CharacterWeapon CharacterWeapon => characterWeapon;
    [SerializeField] bool isExplosionController = false;
    [SerializeField] VfxType vfxTypeExplosion;
    //public Transform centerPointDamage;
    protected BaseController baseTargetAttack;
    Collider[] targetColliders = new Collider[5];
    private float currentShootTime;
    private float shootTime;

    [Space(5)]
    [Header("Rotation component")]
    [SerializeField] float rotationSpeed = 90f;
    [SerializeField] Vector3 vectorRotate = Vector3.one;
    [SerializeField] Vector3 limitRotate = new Vector3(360f,360f,360f);
    GameObject rotationObject;
    [Space(5)]
    [Header("Move component")]
    [SerializeField] TypeAgent typeAgent;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] float stopDistance = 1.5f;
    protected GameObject baseTargetMove;
    
    protected bool isInitialized = false;
    protected bool isMoving = false;
    protected bool isDead = false;
    public bool IsMoving => isMoving;
    public bool IsDead => isDead;
    
    bool isAttack = false;

    Coroutine attackCoroutine;
    Tween shakeTween;
    
    public virtual void Initialize()
    {
        botData.Initialize(this);
        if(characterWeapon)
            characterWeapon.Initialize(this);
        EnableCollider(true);
        EnableNavMesh(true);
        UpdateHealthBar(1f);
        SetSpeed(botData._stats.moveSpeed);
        SetStopDistance(stopDistance);

        if(mainObjectShake != null)
            mainObjectScale = mainObjectShake.transform.localScale;
        
        isDead = false;
        isInitialized = true;
    }
    public virtual void UpdateStats()
    {
        
    }
    protected void EnableHealthBar(bool enable)
    {
        if (healthBar == null)
            return;

        healthBar.EnableBar(enable);
    }

    public void UpdateHealthBar(float amount)
    {
        EnableHealthBar(true);
        if(healthBar)
        {
            healthBar.UpdateValue(amount);
            
            healthBar.SetText($"{botData._stats.health}/{botData._baseStats.health}");
        } 
            
    }
    public void ShakeController()
    {
        mainObjectShake.transform.localScale = mainObjectScale;
        shakeTween.Kill();
        shakeTween = mainObjectShake.transform.DOShakeScale(0.3f,0.4f);
    }
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 original = mainObjectShake.transform.localScale;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float z = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            mainObjectShake.transform.localScale = new Vector3(original.x + x, original.y + y, original.z + z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        mainObjectShake.transform.localScale = original;
    }

    public void HitHighlight()
    {
        if (skinned != null)
        {
            if (highlightCoroutine != null)
            {
                StopCoroutine(highlightCoroutine);
            }

            highlightCoroutine = StartCoroutine(HighlightPulseCoroutine(skinned));
        }
    }

    Coroutine highlightCoroutine;

    IEnumerator HighlightPulseCoroutine(Renderer skinnedMeshRenderer)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        float t = 0.25f; //time
        float f = 1f; //frequency 
        float e = 0;
        while (e <= t)
        {
            skinnedMeshRenderer.GetPropertyBlock(block);
            float time = 0.5f * (1 + Mathf.Sin(2 * Mathf.PI * e * f));
            float s = Mathf.Lerp(0, -0.5f, time);
            float v = Mathf.Lerp(0, 0.5f, time);
            block.SetColor("_EmissionColor", new Color(v,v,v));
            //block.SetFloat("_HSV_V", v);
            skinnedMeshRenderer.SetPropertyBlock(block);
            e += Time.deltaTime;
            yield return null;
        }

        skinnedMeshRenderer.GetPropertyBlock(block);
        block.SetColor("_EmissionColor", new Color(0,0,0));
        skinnedMeshRenderer.SetPropertyBlock(block);
    }
    public void EnableCollider(bool value)
    {
        if(colliderController == null)
            return;
        colliderController.enabled = value;
    }
    public void SetBaseTarget(BaseController target)
    {
        if (baseTargetAttack != target)
        {
            baseTargetAttack = target;
        }
    }
    public void SetTargetMove(GameObject target)
    {
        if (baseTargetMove != target)
        {
            baseTargetMove = target;
        }
    }
    public void SetStopDistance(float distance)
    {
        if(navMeshAgent && navMeshAgent.enabled)
        {
            navMeshAgent.stoppingDistance = distance;
        }
    }
    public void SetSpeed(float speed)
    {
        if(navMeshAgent && navMeshAgent.enabled)
        {
            navMeshAgent.speed = speed;
        }
    }
    public void SetNavMeshDestination(Vector3 position)
    {
        if(navMeshAgent && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(position);
        }
    }
    public void ResetPath()
    {
        if(navMeshAgent && navMeshAgent.enabled)
        {
            navMeshAgent.ResetPath();
        }
    }
    public void EnableNavMesh(bool value)
    {
        if(navMeshAgent)
        {
            navMeshAgent.enabled = value;
        }
    }
    void HandleAgent()
    {
        if(isAttack)
            return;


        if (baseTargetMove != null)
        {
            if (typeAgent == TypeAgent.NavmeshAgent)
            {
                if (Time.frameCount % 10 == 0 && navMeshAgent != null && navMeshAgent.enabled)
                    SetNavMeshDestination(baseTargetMove.transform.position);
            }
            else if (typeAgent == TypeAgent.TransformAgent)
            {
                var dist = baseTargetMove.transform.position - transform.position;
                if (dist.magnitude > stopDistance + 0.5f)
                    transform.position = Vector3.MoveTowards(transform.position, baseTargetMove.transform.position, botData._stats.moveSpeed * Time.deltaTime);
            }

        }
        float magnitude = 0f;
        if(typeAgent == TypeAgent.NavmeshAgent)
        {
            if(navMeshAgent != null && navMeshAgent.enabled)
                magnitude = navMeshAgent.velocity.magnitude;
        }
        else if(typeAgent == TypeAgent.TransformAgent)
        {
            if (baseTargetMove != null)
                magnitude = (baseTargetMove.transform.position - transform.position).magnitude - stopDistance + 0.5f;
        }
        if (magnitude < 0.15f)
        {
            animator.SetAnimatorBool(AnimationParameters.Moving, false);
            animator.SetAnimatorFloat(AnimationParameters.Vertical, 0);
            isMoving = false;
        }
        else
        {
            isMoving = true;
            float t = Mathf.Clamp01(botData._stats.moveSpeed);
            //float s = Mathf.Clamp(data.stats.moveSpeed - 1, 1, 2);
            //animator.SetAnimatorFloat(AnimationParameters.MoveSpeed, s);
            animator.SetAnimatorBool(AnimationParameters.Moving, true);
            animator.SetAnimatorFloat(AnimationParameters.Vertical, t);
        }
    }
    #region COMBAT

    public void UpdateCombat()
    {
        HandleAgent();
        if(baseTargetAttack != null)
        {
            if(typeCombat == TypeCombat.StayCombat)
            {
                RotateTarget();
                Attack();
            }
            else if(typeCombat == TypeCombat.DistanceCombat)
            {
                SetTargetMove(baseTargetAttack.gameObject);
                if (baseTargetMove != null)
                {
                    Vector3 dist = Vector3.zero;
                    if (typeAgent == TypeAgent.NavmeshAgent)
                    {
                        dist = baseTargetMove.transform.position - navMeshAgent.transform.position;
                    }
                    else if (typeAgent == TypeAgent.TransformAgent)
                    {
                        dist = baseTargetMove.transform.position - transform.position;
                    }
                    RotateTarget();
                    if (dist.magnitude <= stopDistance + 0.5f)
                    {
                        Attack();
                    }
                }
            }
        }
        else
        {
            SetTargetMove(null);
            if(typeCombat == TypeCombat.StayCombat)
            {
                Patrol();
            }
            else if(typeCombat == TypeCombat.DistanceCombat)
            {
                
            }
            ResetAttack(false);
        }
    }
    void ExplosionController()
    {
        if(isExplosionController == false)
            return;
        VfxHit(transform.position + new Vector3(0,1,0));
        AttackData attackData = new AttackData(this);
        attackData.AddDamage(botData._stats.health);
        botData.Damage(attackData);
    }
    public void Attack()
    {
        if (characterWeapon == null || characterWeapon.currentWeapon == null)
            return;

        if (currentShootTime >= shootTime && isAttack == false)
        {
            isAttack = true;
            HapticManager.HapticLight();
            AttackTarget();
            currentShootTime = 0;
        }
        currentShootTime += Time.deltaTime;
    }
    void AttackTarget()
    {
        if(animator)
            animator.TriggerAttack(botData._animationComponent.attackType);
        if(botData._stats.attackSpeed <= 0.05f)
        {
            characterWeapon.currentWeapon.Attacked(baseTargetAttack);
            ResetAttack(true);

            ExplosionController();
            return;
        }
        attackCoroutine = StartCoroutine(AttackAnimationRoutine());
    }
    public void ResetAttack(bool loop)
    {
        if(isAttack)
            shootTime = 1f / characterWeapon.currentWeapon.weaponData._stats.rate;
        isAttack = false;

        if(attackCoroutine == null)
            return;

        if(attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        attackCoroutine = null;
        
        animator.TriggerAttack(AttackType.None);
    }
    private IEnumerator AttackAnimationRoutine()
    {
        var t = 0f;
        var timer = botData._stats.attackSpeed;
        bool isActive = false;
        while (t < timer)
        {
            t += Time.deltaTime;
            float linearT = t / timer;
            
            animator.SetAnimatorFloat(AnimationParameters.TimerAttack, linearT);
            if (linearT >= 0.4f && !isActive)
            {
                isActive = true;
                characterWeapon.currentWeapon.Attacked(baseTargetAttack);
            }
            yield return null;
        }
        ExplosionController();
        ResetAttack(true);
    }
    public void FindTarget(int layerMask)
    {
        GetCombatTarget(layerMask);
    }
    public void GetCombatTarget(int layerMask)
    {
        if(botData == null || pointCombatArea == null)
            return;
        float range = botData._stats.rangeAttack;
        if(characterArea)
            characterArea.SetRadius(range + 0.3f);
        Vector3 origin = pointCombatArea.position;
        int count = Physics.OverlapSphereNonAlloc(origin, range, targetColliders, layerMask);
        SetBaseTarget(GetClosestCombatTarget(targetColliders));
    }
    private BaseController GetClosestCombatTarget(Collider[] colliders)
    {
        BaseController bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] && colliders[i].gameObject.TryGetComponent<BaseController>(out BaseController target))
            {
                if (target.IsDead == false)
                {
                    Vector3 directionToTarget = colliders[i].transform.position - currentPosition;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        bestTarget = target;
                        targetColliders[i] = null;
                    }
                }
            }
        }
        return bestTarget;
    }
    public void SetRotationObject(GameObject rotObj)
    {
        rotationObject = rotObj;
    }
    public void RotateTarget()
    {
        if(rotationObject == null)
            return;
        Vector3 targetDirection = Vector3.Normalize(baseTargetAttack.transform.position - rotationObject.transform.position);
        Vector3 smoothedLookInputDirection = Vector3
            .Slerp(rotationObject.transform.forward, targetDirection, 1 - Mathf.Exp(-rotationSpeed * Time.deltaTime)).normalized;
        var scale = Vector3.Scale(smoothedLookInputDirection, vectorRotate);
        rotationObject.transform.rotation = Quaternion.LookRotation(scale);

        Vector3 eulerAngles = rotationObject.transform.rotation.eulerAngles;

        eulerAngles.x = (eulerAngles.x > 180) ? eulerAngles.x - 360 : eulerAngles.x;
        eulerAngles.y = (eulerAngles.y > 180) ? eulerAngles.y - 360 : eulerAngles.y;
        eulerAngles.z = (eulerAngles.z > 180) ? eulerAngles.z - 360 : eulerAngles.z;
        
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, -limitRotate.x, limitRotate.x);
        eulerAngles.y = Mathf.Clamp(eulerAngles.y, -limitRotate.y, limitRotate.y);
        eulerAngles.z = Mathf.Clamp(eulerAngles.z, -limitRotate.z, limitRotate.z);

        rotationObject.transform.rotation = Quaternion.Euler(eulerAngles);
    }

    public void Patrol()
    {
        if(rotationObject == null)
            return;
        float rotationAngle = 180;

        var yR = Mathf.SmoothStep(-rotationAngle, rotationAngle, Mathf.PingPong(Time.time * 0.15f, 1));
        rotationObject.transform.rotation = Quaternion.Euler(0, yR, 0);
    }
    
    #endregion
    
    public virtual void Death()
    {
        isDead = true;
        DeactivatedController();
        if(animator)
            animator.TriggerDeath(DeathType.Death2);
    }
    public virtual void DeactivatedController()
    {
        if(characterWeapon && characterWeapon.currentWeapon)
            characterWeapon.currentWeapon.ResetWeapon();
        EnableCollider(false);
        EnableNavMesh(false);
        baseTargetAttack = null;
        baseTargetMove = null;
        isInitialized = false;
        //DisableTool();
        if (animator)
        {
            animator.SetAnimatorBool(AnimationParameters.Moving, false);
            animator.SetAnimatorTrigger(AnimatorTrigger.InstantSwitchTrigger);
        }
    }
    protected void DeathAnimation(Action endAction)
    {
        deathCoroutine = StartCoroutine(DeathRoutine(endAction));
    }
    private Coroutine deathCoroutine;
    IEnumerator DeathRoutine(Action endAction)
    {
        yield return new WaitForSecondsRealtime(3f);
        float t = 3f;
        float e = 0;
        Vector3 startPosition = transform.position;
        while (e <= t)
        {
            transform.position = Vector3.LerpUnclamped(startPosition, startPosition + Vector3.down * 5, e / t);
            e += Time.deltaTime;
            yield return null;
        }
        endAction?.Invoke();
    }
    public virtual void Return(){}

    public void VfxHit(Vector3 position)
    {
        var vfx = PoolController.Instance.GetVfx(vfxTypeExplosion);
        if (vfx)
        {
            vfx.transform.position = position;
            vfx.Inititalize();
            vfx.Play();
        }
    }
    private void OnDrawGizmos()
    {
        if(pointCombatArea == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointCombatArea.position, botData._baseStats.rangeAttack);
    }
}
