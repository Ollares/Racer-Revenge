using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    [Header("Bar")]
    [SerializeField] Image _fillImage;
    //bar move update time
    [SerializeField] private float _updateTime = 0.1f;
    [SerializeField] private bool TEST;
    float testValue = 0f;
    private void Start()
    {
        _fillImage.fillAmount = 0;
    }

    private void LateUpdate()
    {
        if (TEST)
        {
            testValue += Time.deltaTime*0.5f;
            if(testValue > 1.0)
            {
                testValue = 0;
            }
            UpdateProgress(testValue);
        }
    }
    public void SetFillValue(float value)
    {
        _fillImage.fillAmount = value;
    }
    /// <summary>
    /// Updates progress bar with value 0.0-1.0, delay activates smoothing
    /// </summary>
    /// <param name="value"></param>
    /// <param name="delayed"></param>
    public void UpdateProgress(float value, bool delayed = true)
    {
        if(delayed == true && _updateTime > 0 && gameObject.activeInHierarchy == true)
        {
            StartCoroutine(UpdateWithDelay(_fillImage, value));
        }
        else
        {
            SetFillValue(value);
        }
     
    }

    IEnumerator UpdateWithDelay(Image fillImage, float value)
    {
        float tempValue = fillImage.fillAmount;
        float elapsed = 0;
        while (elapsed < _updateTime)
        {
            SetFillValue(Mathf.Lerp(tempValue, value, elapsed / _updateTime));
            elapsed += Time.deltaTime;
            yield return null;
        }
        SetFillValue(value);
    }
}
