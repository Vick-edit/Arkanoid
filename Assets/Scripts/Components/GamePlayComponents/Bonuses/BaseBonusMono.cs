using GamePlayScripts.GameBonus.DataModels;
using GamePlayScripts.GameBonus.Interfaces;
using Tools;
using UnityEngine;

namespace Components.GamePlayComponents.Bonuses
{
    /// <summary>
    ///     Базовый класс для компонент бонусов
    /// </summary>
    public abstract class BaseBonusMono : MonoBehaviour
    {
        [SerializeField]private BonusMoverParameters _moverParameters;
        public BonusManagerMono BonusManager { get; set; }
        private IBonusMover _mover;
        private bool isCollided;

        private void Awake()
        {
            _moverParameters.BonusTransform = this.transform;
            _mover = DependencyResolver.GetBonusMover(_moverParameters);
        }

        private void Update()
        {
            if(isCollided)
                return;

            var movementDuration = Time.deltaTime;
            if (_mover.MoveBonusDown(movementDuration))
            {
                isCollided = true;
                ApplayBonus();
            }
        }
        

        protected abstract void ApplayBonus();
    }
}