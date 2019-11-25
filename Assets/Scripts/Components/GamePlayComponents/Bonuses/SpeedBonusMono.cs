using UnityEngine;

namespace Components.GamePlayComponents.Bonuses
{
    /// <summary>
    ///     Компонент, отвечающий за применение бонуса скорости
    /// </summary>
    public class SpeedBonusMono : BaseBonusMono
    {
        [SerializeField] private float _speedModifier;

        /// <inheritdoc />
        protected override void ApplayBonus()
        {
            var allPlayerBalls = BonusManager.PlayerBallContainer.GetComponentsInChildren<PlayerBallMono>();
            foreach (var playerBall in allPlayerBalls)
            {
                playerBall.ChangeSpeedByValue(_speedModifier);
            }

            Destroy(this.gameObject);
        }
    }
}