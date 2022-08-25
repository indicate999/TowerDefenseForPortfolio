using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "TurretData", menuName = "My Assets/Turret Data")]
public class TurretData : ScriptableObject
{
    [SerializeField] private Turret[] _turretSeries;

    public Turret[] TurretSeries { get { return _turretSeries; } }

    [Serializable]
    public class Turret
    {
        public Sprite Sprite;
        public RuntimeAnimatorController AnimatorController;
        public float Damage;
        public float AttackSpeed;
        public float AttackRadius;
        public float PurchasePrice;
        public float SellingPrice;
    }

    
}
