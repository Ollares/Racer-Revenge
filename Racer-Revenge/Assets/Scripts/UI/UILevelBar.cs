using UnityEngine;
using UnityEngine.UI;

public class UILevelBar : MonoBehaviour
{
    [SerializeField] Text levelText;

    public void UpdateLevelText(string text)
    {
        levelText.text = text;
    }
}
