using System.Linq;
using Extensions;
using UnityEngine;

namespace Components.GamePlayComponents.Bonuses
{
    /// <summary>
    ///     Компонент, добавляющий новые шарики
    /// </summary>
    public class NewBallsBonusMono : BaseBonusMono
    {
        [SerializeField] private PlayerBallMono _playerBallPrefab;
        [SerializeField] private float _newBallDeviation;

        /// <inheritdoc />
        protected override void ApplayBonus()
        {
            var allPlayerBalls = BonusManager.PlayerBallContainer.GetComponentsInChildren<PlayerBallMono>();
            var highestBall = allPlayerBalls.OrderByDescending(x => x.transform.position.y).First();

            var currentBallSpeed = highestBall.BallParameters.BallSpeed;
            var currentMovementVector = highestBall.BallParameters.NormalizedMovementVector;
            var currentPosition = highestBall.transform.position;

            var firstBallMovementDirection = currentMovementVector.RotateByAngle(_newBallDeviation).normalized;
            var secondBallMovementDirection = currentMovementVector.RotateByAngle(-_newBallDeviation).normalized;

            var firstBall = Instantiate(_playerBallPrefab, currentPosition, Quaternion.identity, BonusManager.PlayerBallContainer);
            var secondBall = Instantiate(_playerBallPrefab, currentPosition, Quaternion.identity, BonusManager.PlayerBallContainer);

            firstBall.ChangeSpeedByValue(currentBallSpeed - firstBall.BallParameters.BallSpeed);
            firstBall.StartMovementInDirection(firstBallMovementDirection);

            secondBall.ChangeSpeedByValue(currentBallSpeed - secondBall.BallParameters.BallSpeed);
            secondBall.StartMovementInDirection(secondBallMovementDirection);

            Destroy(this.gameObject);
        }
    }
}