using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Unit
{

    new void Start() {
        base.Start();
        SetUnitStats(5, 5, 1, 1, 1.0f, 5f, true);
    }

}
