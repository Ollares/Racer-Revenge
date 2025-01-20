using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    private BaseController _baseController;
    [SerializeField] private List<Weapon> allWeapons;
    [SerializeField] private TypeWeapon activeWeapon;
    public Weapon currentWeapon;

    public void Initialize(BaseController baseController)
    {
        _baseController = baseController;
        if (currentWeapon == null)
            SwitchWeapon(activeWeapon);

    }
    public void SwitchWeapon(TypeWeapon typeWeapon)
    {
        foreach (var item in allWeapons)
        {
            item.gameObject.SetActive(false);
        }
        currentWeapon = allWeapons.Find((value) => value.typeWeapon == typeWeapon);
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.Initialize(_baseController);
    }
    
}
