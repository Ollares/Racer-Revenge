using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectilesData", menuName = "Game Data/Projectiles Data", order = -999)]
public class ProjectilesDataSO : ScriptableObject
{
    public List<Projectile> Projectiles = new List<Projectile>();
}
