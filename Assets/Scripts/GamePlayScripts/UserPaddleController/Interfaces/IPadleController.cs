
using UnityEngine;

namespace GamePlayScripts.UserPaddleController
{
    /// <summary>
    ///     Интерфейс контроллера площадки, которой управляет пользователь
    /// </summary>
    public interface IPaddleController
    {
        /// <summary> Сместить панельку пользователя по горизонтальной оси </summary>
        /// <param name="newPosition">Новая ожидаемая координата панельки</param>
        /// <returns>Позиция в которую установится панелька, с учетом всех ограничений</returns>
        Vector2 MovePaddleHorizontal(Vector2 newPosition);

        /// <summary> Изменить ширину панельки на указанное значение </summary>
        /// <param name="sizeDifference">Число на которое изменяется ширина панельки, если отрицательное, то она сужается</param>
        /// <returns>Изменённая ширина панельки с учётом всех ограничений</returns>
        float AddSize(float sizeDifference);

        /// <summary> Сбросить ширину панельки до дефолтного значения</summary>
        void ResetPaddleSize();
    }
}