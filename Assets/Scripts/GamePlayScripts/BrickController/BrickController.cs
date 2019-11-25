using GameEventParams;
using GamePlayScripts.BrickController.DataModels;
using GamePlayScripts.PlayerBallController;
using UnityEngine;
using Utils.Dispatcher;
using Utils.EffectPool;
using ILogger = Utils.Logging.ILogger;

namespace GamePlayScripts.BrickController
{
    public class BrickController : IBrickController
    {
        private readonly BrickControllerParameters _brickParameters;
        private readonly IDispatcher _messageDispatcher;
        private readonly IEffectPool _effectPool;
        private readonly ILogger _logger;
        private int _currentLiveId = 0;

        /// <inheritdoc />
        public BrickController(BrickControllerParameters brickParameters, IDispatcher messageDispatcher, IEffectPool effectPool, ILogger logger)
        {
            _brickParameters = brickParameters;
            _messageDispatcher = messageDispatcher;
            _effectPool = effectPool;
            _logger = logger;
        }


        /// <inheritdoc />
        public void InitBrick()
        {
            if (IsDead())
            {
                _logger.LogWarning("Был создан кирпичик без единой жизни, он будет сразу же разрушен");
                Object.Destroy(_brickParameters.BrickTransform.gameObject);
            }
            else
            {
                var currentLive = _brickParameters.Lives[_currentLiveId];
                _brickParameters.BrickSpriteHolder.sprite = currentLive.LiveSprite;
                _messageDispatcher.Rise(this, GameManagementEvent.OnBrickSpawned());
            }
        }

        /// <inheritdoc/>
        public void OnBallHit(IPlayerBallController ballController)
        {
            if (IsDead())
                return;

            var currentLiveHint = _brickParameters.Lives[_currentLiveId].hitScore;
            var brickHintEventParams = new BrickWasHitEventParams(currentLiveHint);
            _messageDispatcher.Rise(this, brickHintEventParams);

            _currentLiveId++;
            if (IsDead())
            {
                var deathScore = _brickParameters.BrickLiveScore;
                var brickPosition = _brickParameters.BrickTransform.position;
                var brickDeathsEventParams = new BrickWasDestroyedEventParams(deathScore, brickPosition);

                var effectPosition = _brickParameters.BrickTransform.position;
                var effectRotation = _brickParameters.BrickTransform.rotation;
                _effectPool.AddEffectRequest(_brickParameters.DeathEffect, effectPosition, effectRotation);

                _messageDispatcher.Rise(this, brickDeathsEventParams);
                Object.Destroy(_brickParameters.BrickTransform.gameObject);
            }
            else
            {
                var currentLive = _brickParameters.Lives[_currentLiveId];
                _brickParameters.BrickSpriteHolder.sprite = currentLive.LiveSprite;
            }
        }

        private bool IsDead()
        {
            return _currentLiveId >= _brickParameters.Lives.Length;
        }
    }
}