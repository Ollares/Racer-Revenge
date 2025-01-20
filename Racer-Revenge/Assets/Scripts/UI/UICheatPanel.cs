using UnityEngine;
using UnityEngine.UI;

public class UICheatPanel : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [Space]
    [Header("Level")]
    [SerializeField] Text levelText;
    [SerializeField] Button levelPrevButton;
    [SerializeField] Button levelNextButton;
    [Header("Gamma")]
    [SerializeField] Text gammaText;
    [SerializeField] Button gammaPrevButton;
    [SerializeField] Button gammaNextButton;
    [Space]
    [Header("Money")]
    [SerializeField] InputField moneyInputField;
    [Space]
    [Header("Slider")]
    [SerializeField] Slider slider;
    [Space]
    [Header("Toggle")]
    [SerializeField] Toggle toggle;
    private void Awake()
    {
        levelPrevButton.onClick.RemoveAllListeners();
        levelPrevButton.onClick.AddListener(PrevLevel);
        levelNextButton.onClick.RemoveAllListeners();
        levelNextButton.onClick.AddListener(NextLevel);

        gammaPrevButton.onClick.RemoveAllListeners();
        gammaPrevButton.onClick.AddListener(PrevGamma);
        gammaNextButton.onClick.RemoveAllListeners();
        gammaNextButton.onClick.AddListener(NextGamma);

        moneyInputField.onValueChange.AddListener(OnMoneyInput);
        moneyInputField.onValueChanged.AddListener(OnMoneyInput);

        slider.onValueChanged.AddListener(SliderChanged);

        toggle.onValueChanged.AddListener(ToggleChanged);
    }
    public void Open()
    {
        panel.SetActive(!panel.activeInHierarchy);
    }
    private void ToggleChanged(bool value)
    {
       
    }
    private void SliderChanged(float value)
    {
        
    }
    private void PrevGamma()
    {
        //int lvl = GameCore.Instance.CheatPreviousGamma();
        //gammaText.text = "GAMMA " + lvl;
    }
    private void NextGamma()
    {
        //int lvl = GameCore.Instance.CheatNextGamma();
        //gammaText.text = "GAMMA " + lvl;
    }

    private void PrevLevel()
    {
        //GameCore.Instance.CheatPreviousLevel();
        //levelText.text = "LEVEL " + GameData.Instance.Level;
    }
    private void NextLevel()
    {
        //GameCore.Instance.CheatNextLevel();
        //levelText.text = "LEVEL " + GameData.Instance.Level;
    }
 
    private void OnMoneyInput(string value)
    {
        int money = 0;
        if (int.TryParse(value, out money))
        {
            GameData.Instance.Money = money;
        }
        GameUI.Instance.MoneyBar.UpdateAmount(GameData.Instance.Money);
    }
}
