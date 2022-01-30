using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride;
    public bool isLeftHandSlot;
    public bool isRightHandSlot;

    public GameObject currentWeaponModel;

    public void UnloadWeapon()
    {
        if (currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }

    public void UnloadWeaponAndDestroy()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }


    public void LoadWeaponModel(Weapon weapon)
    {
        UnloadWeaponAndDestroy();

        if (weapon == null)
        {
            UnloadWeapon();
            return;
        }

        GameObject model = Instantiate(weapon.modelPrefab);

        if (model != null)
        {
            if (parentOverride != null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localEulerAngles = new Vector3(-20.767f, -20.083f, -85.064f);
            //Sword values
            //model.transform.localEulerAngles = new Vector3(179.936f, -86.90601f, -101.764f);
            //model.transform.localRotation = Quaternion.identity;
           // model.transform.localScale = Vector3.one;
        }

        currentWeaponModel = model;
    }
}
