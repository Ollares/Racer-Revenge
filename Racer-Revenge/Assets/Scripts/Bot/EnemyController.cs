using PoolSystem;
using UnityEngine;
public enum EnemyType
    {
        None,
        Default,
        Easy,
        Medium,
        Hard,
        Boss1
    }
public class EnemyController : BaseController
{
    
    [Space(20)]
    public EnemyType enemyType;
    public int EnemyCost;

    private void Start() {
        Initialize();
    }
    public override void Initialize()
    {
        SetRotationObject(gameObject);
        base.Initialize();
        EnableHealthBar(false);
    }

    void Update()
    {
        if(isInitialized)
        {
            FindTarget(GameCore.LayerPlayer);
            UpdateCombat();
        }
    }
    public override void Death()
    {
        base.Death();
        //DeathAnimation(()=> 
        //{
            Return();
        //});
    }
    public override void Return()
    {
        PoolController.Instance.ReturnEnemy(this);
        base.Return();
    }

}
