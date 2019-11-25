using GamePlayScripts.GameBonus.DataModels;
using GamePlayScripts.GameBonus.Interfaces;
using UnityEngine;

namespace GamePlayScripts.GameBonus
{
    public class BonusMover : IBonusMover
    {
        private const float MINIMUM_MOVEMENT_STEP = 0.01f;      //Движения меньше этой величины не считаются
        private readonly BonusMoverParameters _movementParameters;


        /// <inheritdoc />
        public BonusMover(BonusMoverParameters movementParameters)
        {
            _movementParameters = movementParameters;
        }


        /// <inheritdoc />
        public bool MoveBonusDown(float movementDurationInSeconds)
        {
            //Если движение было слишком маленьким, то считаем, что его и не было
            var movementDistance = movementDurationInSeconds * _movementParameters.MovementSpeed;
            if (movementDistance < MINIMUM_MOVEMENT_STEP)
                return false;

            //Просчитываем потенциальное перемещение бонуса
            var startPosition = _movementParameters.BonusTransform.position;
            var movementDirection = Vector2.down;
            var movementVector = movementDistance * movementDirection;

            //Проверяем нет ли препятствий на пути, для осуществления такого преемещения
            var ballRadius = _movementParameters.BonusRadius;
            var collisionMask = _movementParameters.CollisionMask;
            var circleCastHit = Physics2D.CircleCast(startPosition, ballRadius, movementVector, movementDistance, collisionMask);
            var hitCollider = circleCastHit.collider;
            if (hitCollider == null)
            {
                //если припятствий не было, просто двигаем бонус
                _movementParameters.BonusTransform.Translate(movementVector);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}