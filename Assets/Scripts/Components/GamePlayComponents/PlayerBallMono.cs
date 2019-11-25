using System.Linq;
using GameEventParams;
using GamePlayScripts.PlayerBallController;
using GamePlayScripts.PlayerBallController.DataModels;
using Tools;
using UnityEngine;
using Utils.Dispatcher;

namespace Components.GamePlayComponents
{
    /// <summary>
    ///     Компонент снаряда
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class PlayerBallMono : MonoBehaviour
    {
        [SerializeField] private PlayerBallParameters _ballParameters = new PlayerBallParameters();
        public PlayerBallParameters BallParameters => _ballParameters;

        private readonly Color _gizmoInactiveColor = new Color(1f, 0f, 0f, 0.5f);
        private readonly Color _gizmoActiveColor = new Color(0f, 1f, 0f, 0.5f);
        private IPlayerBallController _playerBallController;
        private bool _isCollided;


        private void Awake()
        {
            SetUpController();
            this.Rise(GameManagementEvent.OnBallSpawned());
        }

        private void OnDestroy()
        {
            this.Rise(GameManagementEvent.OnBallDestroyed());
        }



        private void Update()
        {
            _isCollided = _playerBallController.MoveBallForward(Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            if (_playerBallController == null)
            {
                if (!Application.isEditor)
                    return;
                SetUpController();
            }
            _isCollided = Application.isEditor ? _playerBallController.CheckCollision().Any() : _isCollided;

            var save = Gizmos.color;
            Gizmos.color = _isCollided ? _gizmoActiveColor : _gizmoInactiveColor;
            Gizmos.DrawSphere(_playerBallController.BallTransform.position, _ballParameters.BallRadius);
            Gizmos.color = save;
        }


        public void StartMovementInDirection(Vector2 direction)
        {
            direction.Normalize();
            _ballParameters.NormalizedMovementVector = direction;
        }

        public void ChangeSpeedByValue(float speedModificator)
        {
            _playerBallController.ChangeSpeed(speedModificator);
        }

        private void SetUpController()
        {
            _ballParameters.BallTransform = transform; //На всякий случай, если в эдиторе криво настроят
            _playerBallController = DependencyResolver.GetPlayerBallController(_ballParameters);
        }
    }
}