using GamePlayScripts.BrickController;
using GamePlayScripts.BrickController.DataModels;
using GamePlayScripts.PlayerBallController;
using Tools;
using UnityEngine;

namespace Components.GamePlayComponents
{
    /// <summary>
    ///     Компонент для кирпичика, отвечает за задание параметров через эдитор и прокидывании
    ///     сообщений Unity контроллеру кирпичика
    /// </summary>
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(OnBallCollisionMono))]
    public class BrickComponentMono : MonoBehaviour
    {
        [SerializeField] private BrickControllerParameters _brickParameters;
        private IBrickController _brickController;

        private void Awake()
        {
            _brickParameters.BrickTransform = transform; //На всякий случай, если в эдиторе криво настроят
            _brickController = DependencyResolver.GetBrickController(_brickParameters);
            _brickController.InitBrick();
        }

        /// <summary> Метод для использования в качестве колбэка <see cref="OnBallCollisionMono.OnBallCollision"/> </summary>
        public void OnBallHit(IPlayerBallController ballController)
        {
            _brickController.OnBallHit(ballController);
        }
    }
}