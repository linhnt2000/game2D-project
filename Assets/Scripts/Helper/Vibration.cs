using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vibration
{
    public static void VibrateLightImpact()
    {
        if (GameData.IsVibrateEnabled)
        {
            //MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
    }

    public static void VibrateWarning()
    {
        if (GameData.IsVibrateEnabled)
        {
            //MMVibrationManager.Haptic(HapticTypes.Warning);
        }
    }
}
