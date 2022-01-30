using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISaveable
{
    object CaptureState();

    void RestoreState(object state);
}

public interface ICombat
{
    void DoAttack(List<GameObject> hitList);

    void GetAttacked(float damage, Vector3 forceDir);
}

[Serializable]
public class ElementalResistance
{
    public ElementsEnum element;
    public float resistance;
}
