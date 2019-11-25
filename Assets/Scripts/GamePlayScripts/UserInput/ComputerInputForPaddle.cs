using GamePlayScripts.UserInput.Interfaces;
using UnityEngine;

namespace GamePlayScripts.UserInput
{
    /// <summary>
    ///     Реализация инпута для компьютера
    /// </summary>
    public class ComputerInputForPaddle : IUserInputForPaddle
    {
        private readonly Camera _mainCamera;
        private float _previusPaddlePosition = float.NaN;
        private float _previusMousePosition = float.NaN;

        /// <inheritdoc />
        public ComputerInputForPaddle(Camera mainCamera)
        {
            _mainCamera = mainCamera;
            _previusMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        }


        /// <inheritdoc />
        public float GetInputPosition()
        {
            if (float.IsNaN(_previusPaddlePosition))
            {
                _previusMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
                return float.NaN;
            }
                
            var currentMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
            var movementOffset = currentMousePosition - _previusMousePosition;
            var newPaddlePosition = _previusPaddlePosition + movementOffset;

            _previusMousePosition = currentMousePosition;
            return newPaddlePosition;
        }

        /// <inheritdoc />
        public void SyncWithPaddlePosition(float currentPaddlePosition)
        {
            _previusPaddlePosition = currentPaddlePosition;
        }
    }
}