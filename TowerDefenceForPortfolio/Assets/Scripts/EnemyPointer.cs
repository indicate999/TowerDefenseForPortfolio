using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointer : MonoBehaviour
{
    private int _enemyIndex;

    public int EnemyIndex { get { return _enemyIndex; } set { _enemyIndex = value; } }
}
