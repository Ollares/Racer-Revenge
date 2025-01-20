using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : BaseController
{
    public TurretController turretController;
    float currentMoveSpeed = 0f;
    bool start = false;
    Vector3 startPosition;
    void Start()
    {
        GameCore.Instance.cinemachineGameCamera.SetFollowTarget(transform);
        startPosition = transform.position;
    }
    public override void Initialize()
    {
        base.Initialize();
    }
    public void StartController()
    {
        Initialize();
        turretController.ActiveLaser(true);
        start = true;
    }
    void Update()
    {
        if(start)
        {
            UpdateDistancePregress();
            MoveCharacter();

            var finishPosition = GameCore.Instance.Level.finishPoint.transform.position;
            var distanceToFinishPoint = Vector3.Distance(transform.position, finishPosition);
            if(distanceToFinishPoint <= 1f)
            {
                Win();
            }
        }
    }
    void UpdateDistancePregress()
    {
        var finishPosition = GameCore.Instance.Level.finishPoint.transform.position;
        var timer = finishPosition - startPosition;
        var currentTimer = finishPosition - transform.position;
        var time = currentTimer.z / timer.z;
        
        GameUI.Instance.ProgressBar.SetValue(1f - time);
    }
    void MoveCharacter()
    {
        float duration = 10f;
        currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, botData._stats.moveSpeed, duration * Time.deltaTime);

        Vector3 moveDir = transform.position + transform.forward;
        transform.position = Vector3.MoveTowards(transform.position, moveDir, currentMoveSpeed * Time.deltaTime);
    }
    public override void Death()
    {
        start = false;
        turretController.ActiveLaser(false);
        base.Death();
        GameCore.Instance.SetStateGame(GameState.Lose);
    }

    public override void DeactivatedController()
    {
        start = false;
        turretController.ActiveLaser(false);
        base.DeactivatedController();
    }

    void Win()
    {
        DeactivatedController();
        GameCore.Instance.SetStateGame(GameState.Win);
    }
}
