using UnityEngine;

namespace GamePlayScripts.UserInput.Interfaces
{
    /// <summary>
    ///     Интерфейс доступа к пользовательскому инпуту для управления панелькой, отбивающей удары
    /// </summary>
    public interface IUserInputForPaddle
    {
        /// <summary> Значение позиции по горизонтали </summary>
        float GetInputPosition();

        /// <summary> Синхронизировать инпут с текущей позицией панельки, для обратной синхронизации инпута с учётом всех правил для панельки </summary>
        void SyncWithPaddlePosition(float currentPaddlePosition);
    }
}