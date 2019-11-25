using System;
using Utils.Dispatcher.EventParameters;

namespace GameEventParams
{
    public class GameManagementEvent : BaseDispatcherEventParams
    {
        [Serializable]
        public enum GameEvents
        {
            BrickSpawned,
            BallSpawned,
            BallDestroyed,
            AllBallsDestroyed,
            LevelEnded,
            LevelReset,
        }

        public GameEvents GameEvent { get; }

        public GameManagementEvent(GameEvents gameEvent)
        {
            GameEvent = gameEvent;
        }


        public static GameManagementEvent OnBrickSpawned()
        {
            return new GameManagementEvent(GameEvents.BrickSpawned);
        }

        public static GameManagementEvent OnBallSpawned()
        {
            return new GameManagementEvent(GameEvents.BallSpawned);
        }

        public static GameManagementEvent OnBallDestroyed()
        {
            return new GameManagementEvent(GameEvents.BallDestroyed);
        }

        public static GameManagementEvent OnAllBallsDestroyed()
        {
            return new GameManagementEvent(GameEvents.AllBallsDestroyed);
        }

        public static GameManagementEvent OnLevelEnded()
        {
            return new GameManagementEvent(GameEvents.LevelEnded);
        }
    }
}