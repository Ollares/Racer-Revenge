using System.Collections.Generic;
using UnityEngine;
namespace PoolSystem
{
    public class PoolController : MonoBehaviour
    {
        private static PoolController _Instance;
        public static PoolController Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<PoolController>();
                }
                return _Instance;
            }
        }

        private Dictionary<int, ObjectPool<VfxObject>> vfxPoolList = new Dictionary<int, ObjectPool<VfxObject>>();
        private Dictionary<int, ObjectPool<EnemyController>> enemiesPoolList = new Dictionary<int, ObjectPool<EnemyController>>();
        private Dictionary<int, ObjectPool<Projectile>> projectilesPoolList = new Dictionary<int, ObjectPool<Projectile>>();


        public void Initialize()
        {
            for (int i = 0; i < GameData.Instance.EnemyData.Enemies.Count; i++)
            {
                var enemy = GameData.Instance.EnemyData.Enemies[i];                          
                int key = (int)enemy.enemyType;
                if (enemiesPoolList.ContainsKey(key) == false)
                {
                    var tempPool = new ObjectPool<EnemyController>(this, enemy.gameObject, 150);
                    enemiesPoolList.Add(key, tempPool);
                }
               
               
            }
            for (int i = 0; i < GameData.Instance.VFXData.Vfxs.Count; i++)
            {
                var vfx = GameData.Instance.VFXData.Vfxs[i];
                int key = (int)vfx.Type;
                if (vfxPoolList.ContainsKey(key) == false)
                {
                    var tempPool = new ObjectPool<VfxObject>(this, vfx.gameObject, 30);
                    vfxPoolList.Add(key, tempPool);
                }

            }
            for (int i = 0; i < GameData.Instance.ProjectilesData.Projectiles.Count; i++)
            {
                var projectile = GameData.Instance.ProjectilesData.Projectiles[i];
                int key = (int)projectile.Type;
                if (projectilesPoolList.ContainsKey(key) == false)
                {
                    var tempPool = new ObjectPool<Projectile>(this, projectile.gameObject, 50);
                    projectilesPoolList.Add(key, tempPool);
                }

            }
        }

        #region  Effect
        
        public VfxObject GetVfx(VfxType type)
        {
            if (type == VfxType.None)
                return null;
            int key = (int)type;

            if (vfxPoolList.ContainsKey(key) == false)
            {
                Debug.LogError(string.Format("Error! id: {0}, index: {1}, count: {2}", type, key, vfxPoolList.Count));
            }
            var result = vfxPoolList[key].Get();
            //result.transform.SetParent(null);
            result.name = "VFX " + type;
            return result;
        }
        public void ReturnVfx(VfxObject item)
        {
            //item.transform.localScale = Vector3.one;
            if (item.Type >= 0)
            {
                vfxPoolList[(int)item.Type].Return(item);
            }
        }

        #endregion
        #region Enemy Pool
        public EnemyController GetEnemy(EnemyType type)
        {
            int key = (int)type;

            if (enemiesPoolList.ContainsKey(key) == false)
            {
                Debug.LogError(string.Format("Error! id: {0}, index: {1}, count: {2}", type, key, enemiesPoolList.Count));
            }
            var result = enemiesPoolList[key].Get();
            result.transform.SetParent(null);
            result.name = "Enemy" + type;
            return result;
        }
        public void ReturnEnemy(EnemyController item)
        {
            //item.transform.localScale = Vector3.one;
            if (item.enemyType >= 0)
            {
                enemiesPoolList[(int)item.enemyType].Return(item);
            }
        }
        #endregion

        #region Projectile Pool
        public Projectile GetProjectile(AmmoType type)
        {
            int key = (int)type;

            if (projectilesPoolList.ContainsKey(key) == false)
            {
                Debug.LogError(string.Format("Error! id: {0}, index: {1}, count: {2}", type, key, projectilesPoolList.Count));
            }
            var result = projectilesPoolList[key].Get();
            result.name = "Projectile" + type;
            return result;
        }
        public void ReturnProjectile(Projectile item)
        {
            //item.transform.localScale = Vector3.one;
            if (item.Type >= 0)
            {
                projectilesPoolList[(int)item.Type].Return(item);
            }
        }
        #endregion


    
        public void ResetPools()
        {
            foreach (var pool in enemiesPoolList.Values)
            {
                pool.Reset();
            }
            foreach (var pool in projectilesPoolList.Values)
            {
                if(pool != null)
                    pool.Reset();
            }
            foreach (var pool in vfxPoolList.Values)
            {
                if(pool != null)
                    pool.Reset();
            }
        }
    }
}