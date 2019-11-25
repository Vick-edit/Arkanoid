using UnityEngine;

namespace Components.GamePlayComponents.Bonuses
{
    /// <summary>
    ///     Бонус, отвечающий за размер площадки, управляемой пользователем
    /// </summary>
    public class PaddleSizeBonusMono : BaseBonusMono
    {
        [SerializeField] private float _sizeModifier;

        /// <inheritdoc />
        protected override void ApplayBonus()
        {
            var paddle = BonusManager.PlayerPaddle;
            paddle.ChangePaddleSize(_sizeModifier);

            Destroy(this.gameObject);
        }
    }
}