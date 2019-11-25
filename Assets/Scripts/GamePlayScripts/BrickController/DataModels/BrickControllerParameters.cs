using System;
using GamePlayScripts.PlayerBallController;
using UnityEngine;

namespace GamePlayScripts.BrickController.DataModels
{
    /// <summary>
    ///     Класс с параметрами которые задаются в <see cref="BrickController"/> и прокидываются в <see cref="IPlayerBallController"/>
    ///     И т.к. это ссылочный тип, то мы его можем настраивать в едиторе, и наш чистый шарповый класс будет иметь всегда свежие данные
    ///     Что-то наподобии модели в концепции MVP/MVC
    /// </summary>
    [Serializable]
    public class BrickControllerParameters
    {
        /// <summary> Трансформ компонента кирпичика </summary>
        public Transform BrickTransform;

        /// <summary> Компонент, отрисовывающий спрайт кирпичика </summary>
        public SpriteRenderer BrickSpriteHolder;

        /// <summary> Число очков за разрушение кирпичика </summary>
        public int BrickLiveScore;

        /// <summary> Эффект разрушения кирпичика </summary>
        public ParticleSystem DeathEffect;

        /// <summary> Сведения о числе жизней шарика </summary>
        public BrickLives[] Lives;

        [Serializable]
        public class BrickLives
        {
            /// <summary> Количество очков за попадание по кирпичику </summary>
            public int hitScore = 15;

            /// <summary> Спрайт жизни, устанавливается, пока текущая жизнь активна </summary>
            public Sprite LiveSprite;
        }
    }
}