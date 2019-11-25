using System;
using Components.GamePlayComponents;
using UnityEngine;

namespace GamePlayScripts.PlayerBallController.DataModels
{
    /// <summary>
    ///     Класс с параметрами которые задаются в <see cref="PlayerBallMono"/> и прокидываются в <see cref="IPlayerBallController"/>
    ///     И т.к. это ссылочный тип, то мы его можем настраивать в едиторе, и наш чистый шарповый класс будет иметь всегда свежие данные
    ///     Что-то наподобии модели в концепции MVP/MVC
    /// </summary>
    [Serializable]
    public class PlayerBallParameters
    {
        /// <summary> Трансформ шарика на котором были установлены эти параметры </summary>
        public Transform BallTransform;

        /// <summary> Радиус шарика </summary>
        public float BallRadius;

        /// <summary> Тэг контроллера пользователя </summary>
        public string PlayerControllerTag;

        /// <summary> Мксимальная величина смещения, при ударе о контроллер пользователя </summary>
        public float OnPlayerHitMaxOffset;

        /// <summary> Мксимальная величина смещения, при ударе о контроллер пользователя </summary>
        public float OnPlayerHitDeadZoneInCenter;

        /// <summary> Маска объектов с которыми может сталкиваться шарик </summary>
        public LayerMask CollisionMask;

        /// <summary> Скорость шарика </summary>
        public float BallSpeed;
        public float MinBallSpeed;
        public float MaxBallSpeed;

        /// <summary> Нормализованный вектор движения шарика </summary>
        public Vector2 NormalizedMovementVector { get; set; } = Vector2.zero;
    }
}