using System.Runtime.CompilerServices;
using Components.Helpers;
using GameEventParams;
using GamePlayScripts.UserInput.Interfaces;
using GamePlayScripts.UserPaddleController;
using GamePlayScripts.UserPaddleController.DataModels;
using Tools;
using UnityEngine;
using Utils.Dispatcher;

namespace Components.GamePlayComponents
{
    /// <summary>
    ///     Компонент управлени пользовательской панелькой
    /// </summary>
    public class PlayerPaddleMono : MonoBehaviour
    {
        [SerializeField] private UserInputToPaddlePositionMono _paddleInput;
        [SerializeField] private PaddleControllerParameters _paddleControllerParameters;
        private IPaddleController _paddleController;
        private IUserInputForPaddle _userInputForPaddle;
        private Transform _paddleTransform;

        private void Awake()
        {
            _paddleTransform = transform;
            _paddleController = DependencyResolver.GetPaddleController(_paddleControllerParameters);
            _paddleController.ResetPaddleSize();
            this.WeakSubscribe<PlayerPaddleMono, GameManagementEvent>(x => x.OnLevelReset);
        }

        private void OnDestroy()
        {
            this.Unsubscribe<GameManagementEvent>(OnLevelReset);
        }

        private void Update()
        {
            var xCoordinate = _paddleInput.GetPaddlePositionByInput();
            if (float.IsNaN(xCoordinate))
                return;
            var yCoordinate = _paddleTransform.position.y;
            _paddleController.MovePaddleHorizontal(new Vector2(xCoordinate, yCoordinate));
        }

        private void OnLevelReset(object source, GameManagementEvent gameManagementEvent)
        {
            if (gameManagementEvent.GameEvent == GameManagementEvent.GameEvents.LevelReset)
                _paddleController.ResetPaddleSize();
        }

        public void ChangePaddleSize(float modificator)
        {
            _paddleController.AddSize(modificator);
        }
    }
}