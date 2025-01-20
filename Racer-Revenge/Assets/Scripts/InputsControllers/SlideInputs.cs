using UnityEngine;
using UnityEngine.Events;

public class SlideInputs : MonoBehaviour
{
    private Vector3 _startTouchPosition = Vector3.zero;
    private Vector3 _touchedPosition = Vector3.zero;
    private bool inputsEnabled = false;
    [SerializeField] float sensitivity = 5f;
    public void EnableInputs(bool enable)
    {
        inputsEnabled = enable;
    }

    private void Update()
    {
        if (inputsEnabled == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _touchedPosition = Input.mousePosition;
                _startTouchPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 direction = (Input.mousePosition - _touchedPosition).normalized;
                Vector3 deltaDirection = Input.mousePosition - _startTouchPosition;

                var delta = deltaDirection * sensitivity * Time.deltaTime;
                GameCore.Instance.PlayerController.carController.turretController.RotateTurret(delta);
                GameCore.Instance.PlayerController.carController.turretController.Attack();
                
                _startTouchPosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                GameCore.Instance.PlayerController.carController.turretController.RotateTurret(Vector3.zero);
            }
        }
    }
}
