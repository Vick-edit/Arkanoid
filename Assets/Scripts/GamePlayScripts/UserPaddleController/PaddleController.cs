using GameEventParams;
using GamePlayScripts.UserPaddleController.DataModels;
using UnityEngine;
using Utils.Dispatcher;

namespace GamePlayScripts.UserPaddleController
{
    public class PaddleController : IPaddleController
    {
        private readonly PaddleControllerParameters _paddleParameters;
        private readonly IDispatcher _dispatcher;

        /// <inheritdoc />
        public PaddleController(PaddleControllerParameters paddleParameters, IDispatcher dispatcher)
        {
            _paddleParameters = paddleParameters;
            _dispatcher = dispatcher;
        }


        /// <inheritdoc />
        public Vector2 MovePaddleHorizontal(Vector2 newPosition)
        {
            newPosition = ApplayPositionWithBordersCorrection(newPosition);
            return newPosition;
        }

        /// <inheritdoc />
        public float AddSize(float sizeDifference)
        {
            var paddleCurrentSize = _paddleParameters.PaddleSprite.size;
            var newXSize = paddleCurrentSize.x + sizeDifference;
            newXSize = Mathf.Clamp(newXSize, _paddleParameters.MinPaddleSize, _paddleParameters.MaxPaddleSize);

            var newSizeVector = new Vector2(newXSize, paddleCurrentSize.y);
            _paddleParameters.PaddleSprite.size = newSizeVector;
            ApplayPositionWithBordersCorrection(_paddleParameters.PaddleTransform.position); //После изменения размеров панельки мы могли вылезти за пределы экрана

            return newXSize;
        }

        /// <inheritdoc />
        public void ResetPaddleSize()
        {
            var sizeDifferenceWithStartSize = _paddleParameters.StartPaddleSize - _paddleParameters.PaddleSprite.size.x;
            AddSize(sizeDifferenceWithStartSize);
        }


        /// <summary> Поправить ожидаемую позицию панельки с учётом границ и применить к ней перемещение </summary>
        private Vector2 ApplayPositionWithBordersCorrection(Vector2 positionToCorrect)
        {
            var paddleTransform = _paddleParameters.PaddleTransform;
            var paddleHalfSize = _paddleParameters.PaddleSprite.size.x/2f;
            if (positionToCorrect.x - paddleHalfSize < _paddleParameters.LeftBorderXCoordinate)
            {
                positionToCorrect.x = _paddleParameters.LeftBorderXCoordinate + paddleHalfSize;
            }
            else if (positionToCorrect.x + paddleHalfSize > _paddleParameters.RightBorderXCoordinate)
            {
                positionToCorrect.x = _paddleParameters.RightBorderXCoordinate - paddleHalfSize;
            }
           
            paddleTransform.position = positionToCorrect;
            _dispatcher.Rise(this, new OnPaddleMovedEventParams(positionToCorrect.x));

            return positionToCorrect;
        }
    }
}