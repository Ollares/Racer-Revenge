using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;

public class HapticManager : MonoBehaviour
{
    public static void HapticHeavy() // crash
    {
        if (!GameData.Instance.Vibration) return;
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);

        //MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
    }

    public static void HapticLight() // light
    {
        if (!GameData.Instance.Vibration) return;
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }

    public static void HapticMedium() // action
    {
        if (!GameData.Instance.Vibration) return;
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
    }

    public static void HapticLevelPassed() // success
    {
        if (!GameData.Instance.Vibration) return;
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
    }

    public static void HapticGameOver() // gameover
    {
        if (!GameData.Instance.Vibration) return;

        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
    }

    public static void HapticSuperLight() // selection
    {
        if (!GameData.Instance.Vibration) return;

        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
    }
}