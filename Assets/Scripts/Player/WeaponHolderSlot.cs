using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride;
    public bool isLeftHandSlot;
    public bool isRightHandSlot;

    public GameObject currentWeaponModel;

    public void UnloadWeaponAndDestroy()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void LoadNewWeaponModel(GameObject weapon)
    {
        UnloadWeaponAndDestroy();

        if (weapon == null)
            return;
        

        GameObject model = Instantiate(weapon);

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
            model.transform.localRotation = Quaternion.identity;
        }

        currentWeaponModel = model;
        ShowCurrentWeapon(false);
    }

    public void ShowCurrentWeapon(bool showWeapon)
    {
        if (currentWeaponModel)
            currentWeaponModel.SetActive(showWeapon);
    }
}
