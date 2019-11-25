using GamePlayScripts;
using GamePlayScripts.BrickController;
using GamePlayScripts.BrickController.DataModels;
using GamePlayScripts.GameBonus;
using GamePlayScripts.GameBonus.DataModels;
using GamePlayScripts.GameBonus.Interfaces;
using GamePlayScripts.PlayerBallController;
using GamePlayScripts.PlayerBallController.DataModels;
using GamePlayScripts.UserInput;
using GamePlayScripts.UserInput.Interfaces;
using GamePlayScripts.UserPaddleController;
using GamePlayScripts.UserPaddleController.DataModels;
using UnityEngine;
using UnityEngine.UI;
using Utils.Dispatcher;
using Utils.EffectPool;
using Utils.Logging;
using Utils.PreprocessorDirectives;
using IEffectPool = Utils.EffectPool.IEffectPool;
using ILogger = Utils.Logging.ILogger;

namespace Tools
{
    /// <summary>
    ///     Класс, который знает, какому интерфейсу соответствует какой класс и как его строить,
    ///     по сути заменя конфигу/фабрике/DI есть недостатки, такие, как большач кодо генерация и быстрое разрастание класса,
    ///     но так же даёт гибкость, заменить реализацию интерфейса в проекте можно в одном месте, при этом
    ///     не используется рефлекшен, как стандартном DI, а значит можно без опаски использовать в апдейт и прочих часто вызываемых местах,
    ///     главное не создавать контейнеров данных, ну разве что только на слабых ссылках, как временный кэш объектов
    /// </summary>
    public static class DependencyResolver
    {
        /// <summary> Получить логгер </summary>
        public static ILogger GetLogger()
        {
            if(IsIt.Editor)
                return new UnityLogger();
            else
                return new MockLogger();
        }

        /// <summary> Получить инстанс диспатчера </summary>
        public static IDispatcher GetCachedDispatcher()
        {
            return DispatcherWrapper.Instance;
        }

        /// <summary> Получить инстанс пула эффектов </summary>
        public static IEffectPool GetCachedEffectPool()
        {
            return EffectPoolWrapper.Instance;
        }

        /// <summary> Получить контроллер кирпичика </summary>
        public static IBrickController GetBrickController(BrickControllerParameters brickParameters)
        {
            var dispatcher = GetCachedDispatcher();
            var effectPool = GetCachedEffectPool();
            var logger = GetLogger();
            return new BrickController(brickParameters, dispatcher, effectPool, logger);
        }

        /// <summary> Получить контроллер шарика </summary>
        public static IPlayerBallController GetPlayerBallController(PlayerBallParameters ballParameters)
        {
            return new PlayerBallController(ballParameters);
        }

        /// <summary> Получить контроллер панельки, которой управляет пользователь </summary>
        public static IPaddleController GetPaddleController(PaddleControllerParameters paddleParameters)
        {
            var dispatcher = GetCachedDispatcher();
            return new PaddleController(paddleParameters, dispatcher);
        }

        /// <summary> Получить класс инпута для компьютера </summary>
        public static IUserInputForPaddle GetUserInput(Camera mainCamera)
        { 
            return new ComputerInputForPaddle(mainCamera);
        }

        /// <summary> Получить класс инпута для телефона </summary>
        public static IUserInputForPaddle GetUserInput(Slider slider)
        {
            return new MobileInputForPaddle(slider);
        }

        public static IBonusMover GetBonusMover(BonusMoverParameters moverParameters)
        {
            return new BonusMover(moverParameters);
        }
    }
}