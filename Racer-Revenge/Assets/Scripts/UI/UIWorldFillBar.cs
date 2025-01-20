using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace UISystem
{
    public class UIWorldFillBar : MonoBehaviour
    {
        public enum BarType
        {
            Canvas,
            World
        }
        public enum FillType
        {
            Slider,
            Image
        }
        public enum FillTurn 
        {
            Start,
            End
        }
        public enum TimeType
        {
            Delta,
            UnscaledDelta
        }
        [SerializeField] protected TimeType timeType;
        [SerializeField] BarType barType;
        [SerializeField] FillType typeFill;
        [SerializeField] protected FillTurn fillTurn;
        [SerializeField] Canvas _canvas;
        public float _updateTime;
        [SerializeField] RectTransform _fillBar;
        public RectTransform FillBar => _fillBar;
        [SerializeField] Image _fillImage;
        [SerializeField] private Slider slider;
        [SerializeField] Gradient _gradient;
        [SerializeField] TMP_Text textValue;
        [SerializeField] private RectTransform indicator;        
        [SerializeField] float maxLimitIndicatorPosition;
        Coroutine fillCoroutine;
        Coroutine disableCoroutine;

        public UnityEvent OnFill;
        
        private void Awake()
        {
            if(barType == BarType.World)
                _canvas.worldCamera = GameCore.Instance.cinemachineGameCamera.Main;
        }
        
        public void SetValue(float value)
        {
            if(typeFill == FillType.Slider)
                slider.value = value;
            else if(typeFill == FillType.Image)
                _fillImage.fillAmount = value;
            
            if(indicator)
                indicator.anchoredPosition = new Vector2(Mathf.Lerp(-maxLimitIndicatorPosition, maxLimitIndicatorPosition, value), 0);
            //_fillImage.color = _gradient.Evaluate(value);
        }

        public void UpdateValue(float value)
        {
            if (gameObject.activeInHierarchy)
            {
                if (fillCoroutine != null)
                {
                    StopCoroutine(fillCoroutine);
                }
                fillCoroutine = StartCoroutine(UpdateWithDelay(value));
            }
            else
            {
                //_fillImage.fillAmount = value;
            }
        }
        public void SetColorFillImage(Color color)
        {
            _fillImage.color = color;
        }
        IEnumerator UpdateWithDelay(float newValue)
        {
            float startValue = 0;
            if(typeFill == FillType.Slider)
                startValue = slider.value;
            else if(typeFill == FillType.Image)
                startValue = _fillImage.fillAmount;
            float elapsed = 0;
            while (elapsed < _updateTime)
            {
                float v = Mathf.Lerp(startValue, newValue, elapsed / _updateTime);
                
                if(typeFill == FillType.Slider)
                    slider.value = v;
                else if(typeFill == FillType.Image)
                    _fillImage.fillAmount = v;

                //_fillImage.color = _gradient.Evaluate(newValue);
                if(timeType == TimeType.Delta)
                    elapsed += Time.deltaTime;
                else if(timeType == TimeType.UnscaledDelta)
                    elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            if(typeFill == FillType.Slider)
                slider.value = newValue;
            else if(typeFill == FillType.Image)
                _fillImage.fillAmount = newValue;
            
            if(newValue >= 1f)
            {
                OnFill?.Invoke();
            }
            fillCoroutine = null;
        }
        public void SetText(string value)
        {
            if(textValue)
                textValue.text = value;
        }
        public void EnableBar(bool enable)
        {
           // Debug.Log(name + " EnableBar : " + enable);
            if(enable)
            {
                if(disableCoroutine != null)
                {
                    StopCoroutine(disableCoroutine);
                    disableCoroutine = null;
                }
                //gameObject.SetActive(true);
                _fillBar.gameObject.SetActive(true);
            }
            else
            {
                // if (gameObject.activeInHierarchy && disableCoroutine == null)
                // {
                //     disableCoroutine = StartCoroutine(DisableBar());
                // }  
                // else
                // {
                //     gameObject.SetActive(false);
                // }
                _fillBar.gameObject.SetActive(false);
            }
        }

        IEnumerator DisableBar()
        {
            while(fillCoroutine != null)
            {
                yield return null;
            }
            yield return new WaitForSecondsRealtime(0.1f);
            disableCoroutine = null;
            gameObject.SetActive(false);
         
        }

        private void LateUpdate()
        {
            if(barType == BarType.World)
                _fillBar.forward = GameCore.Instance.cinemachineGameCamera.transform.forward;
        }
    }


}
