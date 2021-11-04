using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class testAbility : abstractAbility
{

    public GameObject cube;

    public override void activate(GameObject parent)
    {
        Instantiate(cube).transform.position = parent.transform.position;
    }
}
