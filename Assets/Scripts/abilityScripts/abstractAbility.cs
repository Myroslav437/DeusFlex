using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abstractAbility : ScriptableObject
{
    public  string abilityName;
    public float cooldownTime;
    public float activeTime;

    public virtual void activate(GameObject parent) { }
}
