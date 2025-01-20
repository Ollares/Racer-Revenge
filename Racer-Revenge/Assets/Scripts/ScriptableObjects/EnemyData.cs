using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game Data/Enemy Data", order = -999)]
public class EnemyData : ScriptableObject
{
    public List<EnemyController> Enemies = new List<EnemyController>();
}
