using System;
using UnityEngine;
using Utils.Dispatcher.EventParameters;

namespace Utils.Dispatcher
{
    /// <summary>
    ///     Враппера над <see cref="IDispatcher"/>, который обеспечивает то, что в проекте будет один и только один диспатчер
    ///     нужено, чтобы было проще дебажить код и отыскивать петли и другие проблемы, связанные с диспетчеризацией,
    /// 
    ///     Построен ввиде расширяющих методов к объектам и singelton'a
    /// </summary>
    public static class DispatcherWrapper
    {
        private static readonly Lazy<IDispatcher> LazyInstance = new Lazy<IDispatcher>(() => new Dispatcher());

        /// <summary> Singleton точка доступа к реализации <see cref="IDispatcher"/> </summary>
        public static IDispatcher Instance => LazyInstance.Value;


        #region StaticAccess
        /// <summary> Подписаться на событие </summary>
        /// <typeparam name="T">Тип параметра события <see cref="BaseDispatcherEventParams"/></typeparam>
        /// <param name="handler">Объект, который подписывается на событие</param>
        /// <param name="callback">Метод, который будет вызываться по эвенту, если объект <see cref="handler"/> ещё жив</param>
        public static void Subscribe<T>(this MonoBehaviour handler, Action<object, T> callback) where T : BaseDispatcherEventParams
        {
            Instance.Subscribe(handler, callback);
        }

        /// <summary>  Подписаться не создавая жёсткую ссылку, которая не даст объекту GC убрать объект, если будет такая необходимость </summary>
        /// <typeparam name="T">Тип параметра события <see cref="BaseDispatcherEventParams"/></typeparam>
        /// <typeparam name="H">Тип объекта на котором будет вызываться эвент</typeparam>
        /// <param name="handler">Объект, который подписывается на событие</param>
        /// <param name="callbackProvider">Колбэк, который вернёт метод, который будет вызываться по эвенту, если объект <see cref="handler"/> ещё жив</param>
        public static void WeakSubscribe<H, T>(this H handler, Func<H, Action<object, T>> callbackProvider) 
            where T : BaseDispatcherEventParams
            where H : MonoBehaviour
        {
            Instance.WeakSubscribe(handler, callbackProvider);
        }

        /// <summary>  Отписаться от некоторого события, которое должно было вызываться на объекте </summary>
        /// <typeparam name="T">Тип параметра события <see cref="BaseDispatcherEventParams"/></typeparam>
        /// <param name="handler">Объект, который подписывался на событие</param>
        /// <param name="callback">Метод, который должен был вызываться на объекте <see cref="handler"/></param>
        /// <returns>Прошла ли одписка от события успешно</returns>
        public static bool Unsubscribe<T>(this MonoBehaviour handler, Action<object, T> callback) where T : BaseDispatcherEventParams
        {
            return Instance.Unsubscribe(handler, callback);
        }

        /// <summary> Вызывать некоторое событие </summary>
        /// <typeparam name="T">Тип параметра события <see cref="BaseDispatcherEventParams"/></typeparam>
        /// <param name="source">Объект, который вызвал событие</param>
        /// <param name="eventParam">Параметры события</param>
        /// <returns>Было ли обработано событие хотя бы одним подписчиком</returns>
        public static bool Rise<T>(this MonoBehaviour source, T eventParams) where T : BaseDispatcherEventParams
        {
            return Instance.Rise(source, eventParams);
        }
        #endregion
    }
}