using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasAnimator : MonoBehaviour
{
    [SerializeField] UIAnimatorComponent[] uiAnimators;

    private void Awake()
    {
        for (int i = 0; i < uiAnimators.Length; i++)
        {
            uiAnimators[i].Initialize();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < uiAnimators.Length; i++)
        {
            uiAnimators[i].Update();
        }
    }
}

[System.Serializable]
public class UIAnimatorComponent
{
    public RectTransform rectTransform;

    public bool PositionComponentEnabled;
    public UIPositionComponent PositionComponent;
    [Space]
    public bool ScaleComponentEnabled;
    public UIScaleComponent ScaleComponent;
    [Space]
    public bool RotationComponentEnabled;
    public UIRotationComponent RotationComponent;

    protected Vector3 originPosition;
    protected Vector3 originEulers;
    protected Vector3 originScale;

    public void Initialize()
    {
        originPosition = rectTransform.localPosition;
        originEulers = rectTransform.localEulerAngles;
        originScale = rectTransform.localScale;
    }

    public void Update()
    {
        if(PositionComponentEnabled)
        {
            PositionComponent.Update(rectTransform, originPosition);
        }
        if(ScaleComponentEnabled)
        {
            ScaleComponent.Update(rectTransform);
        }
        if(RotationComponentEnabled)
        {
            RotationComponent.Update(rectTransform, originEulers);
        }
    }

    [System.Serializable]
    public class UIPositionComponent
    {
        public float positionDelta = 1;
        public float moveFrequency = 1;
        public PositionAxis axis;
        public enum PositionAxis
        {
            Y, X, Z
        }
        public void Update(RectTransform parent, Vector3 origin)
        {
            Vector3 p = origin;
            switch (axis)
            {
                case PositionAxis.X:
                    p.x += positionDelta * Mathf.Cos(Time.time * moveFrequency);
                    parent.localPosition = p;
                    break;
                case PositionAxis.Y:
                    p.y += positionDelta * Mathf.Cos(Time.time * moveFrequency);
                    parent.localPosition = p;
                    break;
                case PositionAxis.Z:
                    p.z += positionDelta * Mathf.Cos(Time.time * moveFrequency);
                    parent.localPosition = p;
                    break;
                default:
                    break;
            }
        }
    }
    [System.Serializable]
    public class UIScaleComponent
    {
        public bool loop = true;
        public float scaleSpeed = 1;
        public float scaleLength = 1;
        public float targetScale = 1;
        private float scalingTime;
        private float scalingElapsed = 0;
        public void Update(RectTransform parent)
        {
            scalingTime = Time.time * scaleSpeed;
            if(loop)
            {
                parent.localScale = new Vector3(
                Mathf.PingPong(scalingTime, scaleLength) + targetScale,
                Mathf.PingPong(scalingTime, scaleLength) + targetScale,
                Mathf.PingPong(scalingTime, scaleLength) + targetScale
                );
            }
            else
            {
                var scale = parent.localScale;
                if (scale.x < targetScale)
                {
                    parent.localScale = Vector3.MoveTowards(scale, Vector3.one * targetScale, Time.deltaTime * scaleSpeed); 
                }

            }
         
        }
    }
    [System.Serializable]
    public class UIRotationComponent
    {
        public float degreesPerSecond = 1;
        public float rotationAngle = 45;
        public bool limitRotation;
        public RotationAxis axis;
        private Vector3 rotation;
        public enum RotationAxis
        {
            Y, X, Z
        }

        public void Update(RectTransform parent, Vector3 origin)
        {
            if (limitRotation)
            {
                switch (axis)
                {
                    case RotationAxis.X:
                        var xR = Mathf.SmoothStep(-rotationAngle, rotationAngle, Mathf.PingPong(Time.time * degreesPerSecond, 1));
                        parent.localRotation = Quaternion.Euler(xR, 0, 0);
                        break;
                    case RotationAxis.Y:
                        var yR = Mathf.SmoothStep(-rotationAngle, rotationAngle, Mathf.PingPong(Time.time * degreesPerSecond, 1));
                        parent.localRotation = Quaternion.Euler(0, yR, 0);
                        break;
                    case RotationAxis.Z:
                        var zR = Mathf.SmoothStep(-rotationAngle, rotationAngle, Mathf.PingPong(Time.time * degreesPerSecond, 1));
                        parent.localRotation = Quaternion.Euler(0, 0, zR);
                        break;
                    default:
                        break;
                }
            }
            else
            {
               //= origin;
                switch (axis)
                {
                    case RotationAxis.X:
                        rotation.x += Time.deltaTime * degreesPerSecond;
                        rotation.y = 0;
                        rotation.z = 0;
                        break;
                    case RotationAxis.Y:
                        rotation.x = 0;
                        rotation.y += Time.deltaTime * degreesPerSecond;
                        rotation.z = 0;
                        break;
                    case RotationAxis.Z:
                        rotation.x = 0;
                        rotation.y = 0;
                        rotation.z += Time.deltaTime * degreesPerSecond;
                        break;
                    default:
                        break;
                }
                parent.localEulerAngles = rotation;
            }
        }
    }
}
