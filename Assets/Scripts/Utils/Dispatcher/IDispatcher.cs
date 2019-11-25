using System;
using Utils.Dispatcher.EventParameters;

namespace Utils.Dispatcher
{
    /// <summary>
    ///     Основной интерфейс работы диспатчера на основе разделения событий по объекту-параметру эвента
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        ///     Подписаться на какое-то событие
        /// </summary>
        /// <typeparam name="T">Тип параметра события <see cref="BaseDispatcherEventParams"/></typeparam>
        /// <param name="handler">Объект, который подписывается на событие</param>
        /// <param name="callback">Метод, который будет вызываться по эвенту, если объект <see cref="handler"/> ещё жив</param>
        void Subscribe<T>(object handler, Action<object, T> callback) where T : BaseDispatcherEventParams;

        /// <summary>
        ///     Подписаться не создавая жёсткую ссылку, которая не даст объекту GC убрать объект, если будет такая необходимость
        /// </summary>
        /// <typeparam name="T">Тип параметра события <see cref="BaseDispatcherEventParams"/></typeparam>
        /// <typeparam name="H">Тип объекта на котором будет вызываться эвент</typeparam>
        /// <param name="handler">Объект, который подписывается на событие</param>
        /// <param name="callbackProvider">Колбэк, который вернёт метод, который будет вызываться по эвенту, если объект <see cref="handler"/> ещё жив</param>
        void WeakSubscribe<H, T>(H handler, Func<H, Action<object, T>> callbackProvider) 
            where T : BaseDispatcherEventParams
            where H : class;

        /// <summary>
        ///     Отписаться от некоторого события, которое должно было вызываться на объекте
        /// </summary>
        /// <typeparam name="T">Тип параметра события <see cref="BaseDispatcherEventParams"/></typeparam>
        /// <param name="handler">Объект, который подписывался на событие</param>
        /// <param name="callback">Метод, который должен был вызываться на объекте <see cref="handler"/></param>
        /// <returns>Прошла ли одписка от события успешно</returns>
        bool Unsubscribe<T>(object handler, Action<object, T> callback) where T : BaseDispatcherEventParams;

        /// <summary>
        ///     Вызывать некоторое событие
        /// </summary>
        /// <typeparam name="T">Тип параметра события <see cref="BaseDispatcherEventParams"/></typeparam>
        /// <param name="source">Объект, который вызвал событие</param>
        /// <param name="eventParam">Параметры события</param>
        /// <returns>Было ли обработано событие хотя бы одним подписчиком</returns>
        bool Rise<T>(object source, T eventParam) where T : BaseDispatcherEventParams;
    }
}