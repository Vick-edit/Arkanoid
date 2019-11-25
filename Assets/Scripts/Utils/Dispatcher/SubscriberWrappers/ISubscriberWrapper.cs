using Utils.Dispatcher.EventParameters;

namespace Utils.Dispatcher.SubscriberWrappers
{
    /// <summary>
    ///     Базовая обёртка над объектами, которые должны быть вызываны в <see cref="IDispatcher.Rise{T}"/>
    /// </summary>
    internal interface ISubscriberWrapper
    {
        /// <summary> Уникальный Value соответствующий подписавшемуся объекту и колбэку </summary>
        int WrapperId { get; }

        /// <summary> Проверить жив ли объект который должен быть вызван </summary>
        bool IsAlive();
    }

    /// <summary>
    ///     Обёртка над объектами, которые должны быть вызываны в <see cref="IDispatcher.Rise{T}"/>
    /// </summary>
    internal interface ISubscriberWrapper<in TEventParamType> : ISubscriberWrapper
        where TEventParamType : BaseDispatcherEventParams
    {
        /// <summary> Пробросить событие в объект, подписавшийся на событие <see cref="IDispatcher.Rise{T}"/> </summary>
        /// <param name="source">Источник события</param>
        /// <param name="eventParam">Параметры события</param>
        void Invoke(object source, TEventParamType eventParam);
    }
}