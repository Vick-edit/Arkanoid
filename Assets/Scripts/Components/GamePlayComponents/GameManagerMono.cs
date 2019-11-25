using System;
using Components.Helpers;
using GameEventParams;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils.Dispatcher;

namespace Components.GamePlayComponents
{
    /// <summary>
    ///     Основной компонент, отвечающий за управление состоянием игры
    ///     в нём нет сложной логики, в основном управление состояниям и сообщениями, поэтому у него нет отдельного контроллера
    ///     но, если логика будет усложняться, то можно будет рефакторить класс
    /// </summary>
    public class GameManagerMono : MonoBehaviour
    {
        [Header("UI элементы отображения статуса игры")]
        [SerializeField] private ScoreToTextMono _scoreToText;
        [SerializeField] private ScoreToTextMono _leftBrickCounter;
        [SerializeField] private ScoreToTextMono _totalBricksCounter;

        [Header("Страницы окончания игры")]
        [SerializeField] private Transform _winScreen;
        [SerializeField] private Transform _levelComplited;
        [SerializeField] private Transform _deathScreen;

        [Header("Страницы окончания игры")]
        [SerializeField] private Transform _levelContainer;
        [SerializeField] private Tilemap[] _gameLevels = new Tilemap[3];

        [Header("Вспомогательные компоненты")]
        [SerializeField] private PlayerBallLauncherMono _ballLauncher;

        private int _currentScore;
        private int _leftedBrick;
        private int _totalBricks;
        private int _playerBalls;
        private int _nextLevelId;

        private bool _onLevelLoading;

        private void Awake()
        {
            _currentScore = 0;
            _nextLevelId = 1;
            _scoreToText.SetScore(_currentScore);

            this.WeakSubscribe<GameManagerMono, GameManagementEvent>(gm => gm.OnGameEvent);
            this.WeakSubscribe<GameManagerMono, BrickWasHitEventParams>(gm => gm.OnBrickHit);
            this.WeakSubscribe<GameManagerMono, BrickWasDestroyedEventParams>(gm => gm.OnBrickDestroyed);

            _ballLauncher.LaunchBallFromSpawnPosition();
        }


        #region OnGameEvent
        private void OnGameEvent(object source, GameManagementEvent gameManagementEvent)
        {
            switch (gameManagementEvent.GameEvent)
            {
                case GameManagementEvent.GameEvents.BrickSpawned:
                    OnBrickSpawned();
                    return;
                case GameManagementEvent.GameEvents.BallSpawned:
                    _playerBalls++;
                    return;
                case GameManagementEvent.GameEvents.BallDestroyed:
                    _playerBalls--;
                    CheckLoosCondition();
                    return;
            }
        }

        private void OnBrickSpawned()
        {
            _leftedBrick++;
            _leftBrickCounter.SetScore(_leftedBrick);
            _totalBricks++;
            _totalBricksCounter.SetScore(_totalBricks);
        }

        private void OnBrickHit(object source, BrickWasHitEventParams brickHitEventParams)
        {
            _currentScore += brickHitEventParams.HitScore;
            _scoreToText.SetScore(_currentScore);
        }

        private void OnBrickDestroyed(object source, BrickWasDestroyedEventParams brickDestroyedEventParams)
        {
            if (_onLevelLoading)
                return;

            _currentScore += brickDestroyedEventParams.ScoreCost;
            _scoreToText.SetScore(_currentScore);

            _leftedBrick--;
            _leftBrickCounter.SetScore(_leftedBrick);

            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            if (_onLevelLoading)
                return;

            if (_leftedBrick == 0)
            {
                if (_nextLevelId < _gameLevels.Length)
                    _levelComplited.gameObject.SetActive(true);
                else
                    _winScreen.gameObject.SetActive(true);

                this.Rise(GameManagementEvent.OnLevelEnded());
            }
        }

        private void CheckLoosCondition()
        {
            if (_onLevelLoading)
                return;

            if (_playerBalls == 0)
            {
                _deathScreen.gameObject.SetActive(true);
                this.Rise(GameManagementEvent.OnAllBallsDestroyed());
            }
        }
        #endregion


        #region OnUiButtons
        public void ResumeGame()
        {
            _winScreen.gameObject.SetActive(false);
            _deathScreen.gameObject.SetActive(false);
            _ballLauncher.LaunchBallFromSpawnPosition();
        }

        public void LoadNextLevel()
        {
            _onLevelLoading = true;
            this.Rise(new GameManagementEvent(GameManagementEvent.GameEvents.LevelReset));

            _playerBalls = 0;
            _leftedBrick = 0;
            _totalBricks = 0;

            var levelPrefab = _gameLevels[_nextLevelId];
            Instantiate(levelPrefab, _levelContainer);
            _nextLevelId++;
            _onLevelLoading = false;

            _winScreen.gameObject.SetActive(false);
            _levelComplited.gameObject.SetActive(false);
            _deathScreen.gameObject.SetActive(false);
            _ballLauncher.LaunchBallFromSpawnPosition();
        }

        public void ResetGame()
        {
            _nextLevelId = 0;
            _currentScore = 0;
            _scoreToText.SetScore(_currentScore);

            LoadNextLevel();
        }
        #endregion

    }
}