using System.Collections;
using System.Collections.Generic;
using PoolSystem;
using UnityEngine;
public enum TypeWeapon
{
    None,
    Turret,
    Hit
}

public enum TargetAttack
{
    Object,
    Point,
    Direction
}
public enum TypeAttack
{
    Shoot,
    Melee
}
[RequireComponent(typeof(WeaponData))]
public class Weapon : MonoBehaviour
{
    public TypeWeapon typeWeapon;
    public TargetAttack targetAttack;
    public TypeAttack typeAttack;
    public WeaponData weaponData;
    public float maxHeightCurve = 1;
    [SerializeField] private Transform[] muzzlePoint;
    private BaseController _baseController;
    public int hitLayer;
    public void Initialize(BaseController baseController)
    {
        _baseController = baseController;
        if (_baseController is TurretController)
        {
            hitLayer = GameCore.LayerEnemy | GameCore.LayerGround;
        }
        else if (_baseController is EnemyController)
        {
            hitLayer = GameCore.LayerPlayer | GameCore.LayerUnit | GameCore.LayerGround; /*| GameCore.LayerGround|GameCore.Build;*/
        }
        weaponData.Initialize(this);
    }

    public void Attack(BaseController target)
    {
        weaponData.Attack(target, _baseController);
    }
    void HitTarget(BaseController target)
    {
        Attack(target);
        VfxHit(transform.position);
    }
    void ShootTarget(Transform targetPosition)
    {
        switch (targetAttack)
        {
            case TargetAttack.Object:
                SpawnProjectile(targetPosition);
                break;
            case TargetAttack.Point:
                SpawnProjectilePoint(targetPosition.position);
                break;
            case TargetAttack.Direction:
                SpawnProjectileDirection(targetPosition.position);
                break;
        }
    }
    public void Attacked(BaseController target)
    {
        if(target == null)
            return;
        if(typeAttack == TypeAttack.Shoot)
        {
            ShootTarget(target.transform);
        }
        else if(typeAttack == TypeAttack.Melee)
        {
            HitTarget(target);
        }
    }
    protected void SpawnProjectile(Transform target)
    {
        for (int i = 0; i < muzzlePoint.Length; i++)
        {
            Projectile projectile = PoolController.Instance.GetProjectile(weaponData.ammoType);
            projectile.transform.position = muzzlePoint[i].position;
            projectile.target = target;
            projectile.transform.forward = target.position;
            projectile.Initialize(this);
        }

        VfxMuzzle();
    }
    protected void SpawnProjectilePoint(Vector3 target)
    {
        for (int i = 0; i < muzzlePoint.Length; i++)
        {
            Projectile projectile = PoolController.Instance.GetProjectile(weaponData.ammoType);
            projectile.transform.position = muzzlePoint[i].position;
            if(weaponData.ammoType == AmmoType.Missile)
                target = RandomPointInAnnulus(target, -3f, 3f);
            projectile.targetPosition = target;
            projectile.transform.forward = target;
            projectile.Initialize(this);
        }
        // var spreadPosition = SetSpreadShoot(target, weaponData.spreadStat.countProjectile);
        // for (int i = 0; i < spreadPosition.Count; i++)
        // {
        //     Projectile projectile = PoolController.Instance.GetProjectile(weaponData.ammoType);
        //     projectile.transform.position = muzzlePoint.position;
        //     projectile.targetPosition = spreadPosition[i];
        //     projectile.transform.forward = spreadPosition[i];
        //     projectile.Initialize(this);
        // }

        VfxMuzzle();
    }
    protected void SpawnProjectileDirection(Vector3 target)
    {
        for (int i = 0; i < muzzlePoint.Length; i++)
        {
            Projectile projectile = PoolController.Instance.GetProjectile(weaponData.ammoType);
            projectile.transform.position = muzzlePoint[i].position;
            var direction = muzzlePoint[i].transform.forward;

            projectile.targetDirection = direction;
            projectile.transform.forward = direction;
            projectile.Initialize(this);
        }

        VfxMuzzle();
    }
    void VfxMuzzle()
    {
        for (int i = 0; i < muzzlePoint.Length; i++)
        {
            var vfx = PoolController.Instance.GetVfx(weaponData.vfxTypeMuzzle);
            if (vfx)
            {
                vfx.transform.position = muzzlePoint[i].position;
                vfx.Inititalize();
                vfx.Play();
            }
        }
    }
    public void VfxHit(Vector3 position)
    {
        var vfx = PoolController.Instance.GetVfx(weaponData.vfxTypeHit);
        if (vfx)
        {
            vfx.transform.position = position;
            vfx.Inititalize();
            vfx.Play();
        }
    }
    
    public void ResetWeapon()
    {
        ResetAllVfx();
    }
    #region VFX
    List<VfxObject> vfxObjects = new List<VfxObject>();
    
    public void VfxController(VfxType type,  float duration, Vector3 position, Transform parent)
    {
        if(type == VfxType.None)
            return;
        ResetVfx(type);
        var vfx = PoolController.Instance.GetVfx(type);
        if (vfx)
        {
            vfxObjects.Add(vfx);
            vfx.transform.SetParent(parent);
            vfx.transform.localPosition = position;
            vfx.transform.localRotation = Quaternion.identity;
            vfx.duration = duration;
            //vfx.OnStop.AddListener(()=>{ResetVfx(type);});
            vfx.Inititalize();
            vfx.Play();
        }
    }
    public void ResetVfx(VfxType type)
    {
        if(vfxObjects.Count == 0)
            return;
        var vfx = vfxObjects.Find(v => v.Type == type);
        if(vfx)
        {
            vfx.Stop();
            vfxObjects.Remove(vfx);
        }
    }
    void ResetAllVfx()
    {
        foreach (var vfx in vfxObjects)
        {
            vfx.Stop();
        }
        vfxObjects.Clear();
    }
    #endregion
    // private List<Vector3> SetSpreadShoot(Vector3 target, int lines)
    // {
    //     List<Vector3> directions = new List<Vector3>();
    //     float angle = weaponData.spreadStat.angleP / (lines);
    //     float currentAngle = angle;
    //     float offsetX = weaponData.spreadStat.offsetX;

    //     Quaternion rayRotation = Quaternion.AngleAxis(currentAngle * (0), Vector3.up);
    //     Vector3 direction = rayRotation * _baseController.transform.forward;
    //     Vector3 position = target + direction - _baseController.transform.forward;
    //     directions.Add(position);

    //     for (int x = 1; x < lines; x++)
    //     {
    //         rayRotation = Quaternion.AngleAxis(currentAngle * (x), Vector3.up);
    //         direction = rayRotation * _baseController.transform.forward;
    //         position = target + direction - _baseController.transform.forward + (_baseController.transform.right * x * offsetX);
    //         directions.Add(position);

    //         rayRotation = Quaternion.AngleAxis(-currentAngle * (x), Vector3.up);
    //         direction = rayRotation * _baseController.transform.forward;
    //         position = target + direction - _baseController.transform.forward + (_baseController.transform.right * x * -offsetX); ;

    //         directions.Add(position);
    //     }
    //     return directions;

    // }
    protected Vector3 RandomPointInAnnulus(Vector3 origin, float minRadius, float maxRadius)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized; // There are more efficient ways, but well
        Vector3 direction3D = new Vector3(randomDirection.x, 0, randomDirection.y);
        float minRadius2 = minRadius * minRadius;
        float maxRadius2 = maxRadius * maxRadius;
        float randomDistance = Mathf.Sqrt(Random.value * (maxRadius2 - minRadius2) + minRadius2);
        return origin + direction3D * randomDistance;
    }
}
