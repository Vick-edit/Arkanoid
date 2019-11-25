using Extensions;
using GameEventParams;
using Tools;
using UnityEditor;
using UnityEngine;
using Utils.Dispatcher;

namespace Components.GamePlayComponents
{
    /// <summary>
    ///     Компонент запускающий шарик в начале игры
    /// </summary>
    public class PlayerBallLauncherMono : MonoBehaviour
    {
        private readonly Vector2 _leftLaunchDirection = Quaternion.Euler(new Vector3(0, 0, 45)) * Vector2.up;
        private readonly Vector2 _rightLaunchDirection = Quaternion.Euler(new Vector3(0, 0, -45)) * Vector2.up;

        [SerializeField] private float _startAngelsRange;

        [SerializeField] private Vector3 _startingBallPosition;
        [SerializeField] private PlayerBallMono _sceanPlayerBall;
        [SerializeField] private PlayerBallMono _playerBallPrefab;
        private Transform _playerBallParentObject;

        [SerializeField] private Vector3 _startingPadPosition;
        [SerializeField] private Transform _playerControllerPad;

        private IDispatcher _dispatcher;
        public IDispatcher Dispatcher => _dispatcher ?? (_dispatcher = DependencyResolver.GetCachedDispatcher());

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
                var radius = 2;
                var halfRange = Mathf.Abs(_startAngelsRange / 2);
                Handles.DrawSolidArc(_startingBallPosition, Vector3.forward, _leftLaunchDirection, halfRange, radius);
                Handles.DrawSolidArc(_startingBallPosition, Vector3.forward, _leftLaunchDirection, -halfRange, radius);
                Handles.DrawSolidArc(_startingBallPosition, Vector3.forward, _rightLaunchDirection, halfRange, radius);
                Handles.DrawSolidArc(_startingBallPosition, Vector3.forward, _rightLaunchDirection, -halfRange, radius);
            #endif
        }

        public void LaunchBallFromSpawnPosition()
        {
            _playerBallParentObject = _playerBallParentObject ? _playerBallParentObject : _sceanPlayerBall.transform.parent;
            if (_sceanPlayerBall == null)
                _sceanPlayerBall = Instantiate(_playerBallPrefab, _startingBallPosition, Quaternion.identity, _playerBallParentObject);
            else
                _sceanPlayerBall.transform.position = _startingBallPosition;

            var ballMovementVector = GetRandomLaunchVector();
            _sceanPlayerBall.StartMovementInDirection(ballMovementVector);
            _playerControllerPad.transform.position = _startingPadPosition;
            Dispatcher.Rise(_playerControllerPad, new OnPaddleMovedEventParams(_startingPadPosition.x));
        }

        private Vector2 GetRandomLaunchVector()
        {
            var randomValue = Random.Range(-1, 1);
            var launchSide = randomValue < 0 ? _leftLaunchDirection : _rightLaunchDirection;
            return launchSide.RotateRandomByAngle(_startAngelsRange);
        }
    }
}