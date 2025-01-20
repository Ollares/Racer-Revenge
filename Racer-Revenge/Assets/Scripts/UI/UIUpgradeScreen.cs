using DG.Tweening;
using UnityEngine;

public class UIUpgradeScreen : UIScreen
{
    [SerializeField] UIUpgradePanel testUpgradePanel;

    private int testPrice = 0;

    private void Awake()
    {
        testUpgradePanel.OnUpgrade = UpgradeTest;
    }

    public override void Open()
    {
        base.Open();
        UpdateUpgradePanel();
    }
    public override void Close()
    {
        base.Close();
    }


    private void UpgradeTest()
    {
        if (testPrice <= GameData.Instance.Money)
        {
            HapticManager.HapticLight();
            ButtonSuccess(testUpgradePanel.transform);
            //GameData.Instance.TestParameter += 1;
            GameData.Instance.Money -= testPrice;
            GameUI.Instance.MoneyBar.UpdateAmount(GameData.Instance.Money);
            UpdateUpgradePanel("stat_test");
        }
        else
        {
            ButtonFailed(testUpgradePanel.transform);
        }
    }
    /// <summary>
    /// Update upgrade panel after purchase. Update player/game parameters.
    /// </summary>
    /// <param name="statId">Used to specify wich parameter was upgraded for special vfx or sfx etc</param>
    private void UpdateUpgradePanel(string statId = null)
    {
        int basePrice = 30;
        int baseOffset = 5;
        //testPrice = (int)(baseOffset * Mathf.Pow(GameData.Instance.TestParameter, 2) + basePrice);

        //testUpgradePanel.UpdateDescription(string.Format("level {0}", GameData.Instance.TestParameter + 1));

        testUpgradePanel.UpdatePrice(testPrice);

        //update player stats etc
        //GameCore.Instance.UpdatePlayerStats(statId);
        /*
        switch (statId)
        {
            case "stat_test":
            //play vfx
            //play sfx
            //play animation
            default:
                break;
        }
        */
    }

    Sequence buttonFeedback;
    private void ButtonSuccess(Transform buttonTransform)
    {
        if (buttonFeedback != null)
        {
            buttonTransform.localScale = Vector3.one;
            buttonFeedback.Kill(true);
        }

        buttonFeedback = DOTween.Sequence();
        buttonFeedback.Append(buttonTransform.DOPunchScale(new Vector3(0.1f, 0.1f, 1), 0.15f).SetEase(Ease.OutBounce).SetUpdate(true));
    }
    private void ButtonFailed(Transform buttonTransform)
    {
        if (buttonFeedback != null)
        {
            buttonTransform.localEulerAngles = Vector3.zero;
            buttonFeedback.Kill(true);
        }
        buttonFeedback = DOTween.Sequence();
        buttonFeedback.Append(buttonTransform.DOPunchRotation(new Vector3(0, 0, 5f), 0.15f).SetEase(Ease.OutBounce).SetUpdate(true));
    }
}
