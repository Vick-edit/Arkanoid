using GamePlayScripts.UserInput.Interfaces;
using UnityEngine.UI;

namespace GamePlayScripts.UserInput
{
    /// <summary>
    ///     Реализация инпута для компьютера
    /// </summary>
    public class MobileInputForPaddle : IUserInputForPaddle
    {
        private readonly Slider _sliderInput;


        /// <inheritdoc />
        public MobileInputForPaddle(Slider sliderInput)
        {
            _sliderInput = sliderInput;
        }


        /// <inheritdoc />
        public float GetInputPosition()
        {
            return _sliderInput.value;
        }

        /// <inheritdoc />
        public void SyncWithPaddlePosition(float currentPaddlePosition)
        {
            _sliderInput.value = currentPaddlePosition;
        }
    }
}