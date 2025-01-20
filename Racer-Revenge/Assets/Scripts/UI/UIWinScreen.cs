using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIWinScreen : UIScreen
{
    [SerializeField] Button button;
    [SerializeField] Button backgroundButton;
    [SerializeField] RectTransform titleRectTransform;
    [SerializeField] Text titleText;

    public UnityAction OnNext;
    private void Awake()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ButtonPressed);
        backgroundButton.onClick.RemoveAllListeners();
        backgroundButton.onClick.AddListener(ButtonPressed);
    }

    public override void Open()
    {
        base.Open();
    }
    public override void Close()
    {
        base.Close();
    }

    private void ButtonPressed()
    {
        Debug.Log("UIWinScreen : Next!");
        if (OnNext != null)
        {
            OnNext.Invoke();
        }
    }
}
