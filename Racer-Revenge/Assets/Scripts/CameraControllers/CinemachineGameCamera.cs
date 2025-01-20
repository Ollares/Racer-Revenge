using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CinemachineGameCamera : MonoBehaviour
{

    [SerializeField] protected CinemachineBrain brain;
    [SerializeField] protected CinemachineVirtualCamera virtualCamera;
   // [SerializeField] protected CinemachineConfiner confiner;
    [SerializeField] protected Camera main;
    public Camera Main => main;
    public Transform CameraTarget;

    public void SetFollowTarget(Transform target)
    {
        virtualCamera.Follow = target;
    }
    public void ClearFollowTarget()
    {
        virtualCamera.Follow = null;
    }
    public void SetOffset(Vector3 offset)
    {
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = offset;
    }
}
