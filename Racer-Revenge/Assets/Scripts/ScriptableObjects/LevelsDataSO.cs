using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "Game Data/Levels Data", order = -999)]
public class LevelsDataSO : ScriptableObject
{
    public List<Level> Levels = new List<Level>();
}
