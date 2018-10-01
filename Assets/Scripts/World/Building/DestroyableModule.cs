using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableModule : MonoBehaviour
{
    public int HP;

    public List<Enums.ModuleDamageType> WeaponsThatCanDamage = new List<Enums.ModuleDamageType>();

    public void DoDamage(int damageCount, Enums.ModuleDamageType source)
    {
        if (!WeaponsThatCanDamage.Contains(source))
        {
            return;
        }
        HP -= damageCount;
        if (HP <= 0)
        {
            this.gameObject.AddComponent<Rigidbody>();
        }
    }
}
