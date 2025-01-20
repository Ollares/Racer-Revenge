using UnityEngine;
using UnityEngine.UI;

public class UIResourceBar : MonoBehaviour
{
    [SerializeField] Image _iconImage;
    [SerializeField] Text _amountText;

    public void UpdateAmount(int amount)
    {
        _amountText.text = amount.ToString();
    }
    public void UpdateText(string text)
    {
        _amountText.text = text;
    }
}
