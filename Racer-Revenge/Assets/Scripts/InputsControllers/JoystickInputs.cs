using UnityEngine;
using UnityEngine.Events;

public class JoystickInputs : MonoBehaviour
{
    [SerializeField] VariableJoystick _variableJoystick;

    //assign to player
    public UnityAction<Vector3, Vector3, float> OnMove;
    private Vector3 _moveDirection;
    private float _inputSqrMagnitude;
    private Vector3 _velocity;
    private bool inputsEnabled = true;
    public void EnableInputs(bool enable)
    {
        _variableJoystick.gameObject.SetActive(enable);
        inputsEnabled = enable;
    }

    private void Update()
    {
        if(inputsEnabled == true)
        {
            Move();
        }
    }
    private void Move()
    {
        _moveDirection = new Vector3(_variableJoystick.Horizontal, 0, _variableJoystick.Vertical);
        _inputSqrMagnitude = _moveDirection.sqrMagnitude;
        _moveDirection = Vector3.ClampMagnitude(_moveDirection, 1f);
        _velocity = new Vector3(_moveDirection.x, 0f, _moveDirection.z);


        if (OnMove != null)
        {
            OnMove.Invoke(_moveDirection, _velocity, _inputSqrMagnitude);
        }
    }

    /*
 *NOTE: OnMove assign listener. Add this to your character controller

 *Move controller

 */
}
