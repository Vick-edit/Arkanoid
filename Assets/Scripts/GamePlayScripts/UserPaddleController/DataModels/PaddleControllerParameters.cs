using System;
using Components.GamePlayComponents;
using GamePlayScripts.PlayerBallController;
using UnityEngine;

namespace GamePlayScripts.UserPaddleController.DataModels
{
    /// <summary>
    ///     Класс с параметрами которые задаются в <see cref="PlayerPaddleMono"/> и прокидываются в <see cref="IPaddleController"/>
    ///     И т.к. это ссылочный тип, то мы его можем настраивать в едиторе, и наш чистый шарповый класс будет иметь всегда свежие данные
    ///     Что-то наподобии модели в концепции MVP/MVC
    /// </summary>
    [Serializable]
    public class PaddleControllerParameters
    {
        public Transform PaddleTransform;
        public SpriteRenderer PaddleSprite;

        public float StartPaddleSize;
        public float MinPaddleSize;
        public float MaxPaddleSize;

        [SerializeField] private Transform LeftBorder;
        [SerializeField] private Transform RightBorder;

        private float _leftBorderXCoordinate = float.NaN;
        public float LeftBorderXCoordinate
        {
            get
            {
                if (float.IsNaN(_leftBorderXCoordinate))
                    _leftBorderXCoordinate = LeftBorder.position.x;
                return _leftBorderXCoordinate;
            }
        }

        private float _rightBorderXCoordinate = float.NaN;
        public float RightBorderXCoordinate
        {
            get
            {
                if (float.IsNaN(_rightBorderXCoordinate))
                    _rightBorderXCoordinate = RightBorder.position.x;
                return _rightBorderXCoordinate;
            }
        }
    }
}