using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "VFXData", menuName = "Game Data/VFX Data", order = -999)]
public class VFXData : ScriptableObject
{
    public List<VfxObject> Vfxs = new List<VfxObject>();
    public VfxObject vfxConfetti;
}
