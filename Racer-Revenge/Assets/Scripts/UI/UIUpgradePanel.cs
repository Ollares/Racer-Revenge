using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIUpgradePanel : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button fillbutton;
    [SerializeField] private Button button;
    [SerializeField] private Text buttonText;
    [SerializeField] private Text currencyText;
    [SerializeField] private Image currencyImage;
    public UnityAction OnUpgrade;
    private void Awake()
    {
        fillbutton.onClick.AddListener(OnButtonPressed);
    }
    public void UpdateTitle(string title)
    {
        titleText.text = title;
    }
    public void UpdateDescription(string description)
    {
        descriptionText.text = description;
    }
    public void UpdatePrice(int value)
    {
        currencyText.text = value.ToString();
    }

    private void OnButtonPressed()
    {
        if (OnUpgrade != null)
        {
            OnUpgrade.Invoke();
        }
    }
}
