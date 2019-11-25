using System.Collections.Generic;
using UnityEngine;

namespace GamePlayScripts.PlayerBallController
{
    /// <summary>
    ///     Интерфейс логики контроллера шарика
    /// </summary>
    public interface IPlayerBallController
    {
        /// <summary> <see cref="Transform"/> компонента самого шарика </summary>
        Transform BallTransform { get; }

        /// <summary> Получить коллекцию <see cref="GameObject"/> с которыми столкнулся объект с учётом всех настроек: размер объекта, маска слоя и т.д. </summary>
        List<GameObject> CheckCollision();

        /// <summary> Сделать следующее перемещение шарика с учётом времени с последнего перемещения </summary>
        /// <param name="movementDurationInSeconds">Количество секунд, которое нужно учесть, то есть время в течении которого двигался шарик</param>
        /// <returns>Флаг того произошли ли столкновения с другими объектами во время движения</returns>
        bool MoveBallForward(float movementDurationInSeconds);

        /// <summary> Изменить скалярную величину движения шарика </summary>
        /// <param name="speedModificator">Значение на сколько должна изменится скорость</param>
        /// <returns>Флаг того произошли ли столкновения с другими объектами во время движения</returns>
        void ChangeSpeed(float speedModificator);
    }
}