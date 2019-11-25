using System;
using UnityEngine;

namespace GamePlayScripts.GameBonus.DataModels
{
    [Serializable]
    public class BonusMoverParameters
    {
        public Transform BonusTransform;
        public float MovementSpeed;
        public float BonusRadius;
        public LayerMask CollisionMask;
    }
}