using System;
using System.Collections;
using System.Collections.Generic;
using PoolSystem;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public AmmoType Type;
    public GameObject projectileVisual;
    public Transform target;
    Collider targetCollider;
    bool isCollider = false;
    public Vector3 targetPosition;
    public Vector3 targetDirection;
    public bool explosion = false;
    public float radiusExplosion = 2f;
    private Vector3 startPosition;
    private Weapon _weapon;
    private float rayDistance = 2f;
    private Ray _ray;
    private RaycastHit[] _rayHits = new RaycastHit[5];
    private Collider[] _rayHitsExplosion = new Collider[10];
    //private float currentYArc = 0;
    private float progress = 0f;
    bool canMove = false;
    Coroutine delayCoroutine;
    public enum ProjectileType
    {
        None,
        Arc
    }

    public void Initialize(Weapon weapon)
    {
        if(target)
            targetCollider = target.GetComponent<Collider>();
        _weapon = weapon;
        projectileVisual.SetActive(true);
        gameObject.SetActive(true);
        startPosition = transform.position; 
        progress = 0f;
        canMove = true;
        ActiveDelay();
    }

    public void Return()
    {
        StopDelay();
        target = null;
        startPosition = Vector3.zero;
        canMove = false;
        PoolSystem.PoolController.Instance.ReturnProjectile(this);
        ResetProjectile();
    }
    void ActiveDelay()
    {
        StopDelay();
        delayCoroutine = StartCoroutine(DelayRoutine());
    }
    void StopDelay()
    {
        if(delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
        }
    }
    IEnumerator DelayRoutine()
    {
        yield return new WaitForSeconds(10);
        Return();
    }
    private void ResetProjectile()
    {
        gameObject.SetActive(false);
        projectileVisual.SetActive(false);
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        // if(canMove)
        // {
        //     MoveArc();
        // }
    }
    private void FixedUpdate()
    {
        if(canMove)
        {
            UpdateCollision();

            if(_weapon.targetAttack == TargetAttack.Direction)
                Move();
            else
                MoveArc();
        }
    }

    void Move()
    {
        transform.position += targetDirection.normalized * _weapon.weaponData._stats.speed * Time.fixedDeltaTime;
        if ((transform.position - startPosition).sqrMagnitude > 100f * 100f)
        {
            Return();
        }

    }
    void MoveArc()
    {
        progress = Mathf.MoveTowards(progress, 1f, _weapon.weaponData._stats.speed * Time.fixedDeltaTime);
        progress = Mathf.Clamp(progress, 0f, 1f);
        
        Vector3 endPoint = targetPosition;
        if(target != null)
        {
            endPoint = target.position;
        }

        Vector3 controlPoint = (startPosition + endPoint) / 2;
        controlPoint.y += _weapon.maxHeightCurve;
        
        var pos = CalculateBezierPoint(progress, startPosition, controlPoint, endPoint);

        var rotDir = pos - transform.position;
        if(rotDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(rotDir);

        transform.position = pos;

    }
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; // (1 - t)^2 * P0
        p += 2 * u * t * p1; // 2(1 - t)t * P1
        p += tt * p2; // t^2 * P2

        return p;
    }
    private void UpdateCollision()
    {
        var projectileDirection = transform.forward;
        _ray = new Ray(transform.position - projectileDirection, projectileDirection);
        // if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacle")))
        int hitAmount = Physics.RaycastNonAlloc(_ray, _rayHits, rayDistance, _weapon.hitLayer);
        //int hitAmount = Physics.OverlapSphereNonAlloc(transform.position, 0.5f, _rayHits, _weapon.hitLayer);
        Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, Color.red);

        if (hitAmount > 0)
        {
            //Debug.Log("hitAmount = " + hitAmount);
            Vector3 vfxPos = Vector3.one;
            var hit = _rayHits[0].collider.GetComponent<Collider>().GetComponent<BaseController>();
            if (hit)
            {
                _weapon.Attack(hit);
                if(explosion)
                    Explosion(hit);
                
            }
            _weapon.VfxHit(_rayHits[0].point);
            
            Return();
        }

        if(targetCollider)
        {
            if(targetCollider.enabled == false)
            {
                Return();
            }
        }
    }

    void Explosion(BaseController baseTarget)
    {
        var size = Physics.OverlapSphereNonAlloc(baseTarget.transform.position,
            radiusExplosion, _rayHitsExplosion, _weapon.hitLayer);
        if (size == 0)
            return;
        for (int i = 0; i < size; i++)
        {
            if (_rayHitsExplosion[i])
            {
                var explosionTarget = _rayHitsExplosion[i].GetComponent<BaseController>();
                if (explosionTarget && explosionTarget != baseTarget)
                {
                    //Debug.Log("EXPLOSION " + explosionTarget.name);
                    _weapon.Attack(explosionTarget);
                    //_rayHitsExplosion[i] = null;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector3 position = transform.position;
        if (target)
            position = target.position;
        Gizmos.DrawWireSphere(position, radiusExplosion);
    }
}