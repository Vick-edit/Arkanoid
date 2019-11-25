using System;
using Utils.Dispatcher.EventParameters;

namespace Utils.Dispatcher.SubscriberWrappers
{
    /// <summary>
    ///     Враппер, который оборачивает ссылку на сам объект и на замыкание с колбэком с использованием <see cref="WeakReference"/>
    /// </summary>
    internal sealed class SubscriberWrapperWeak<THandler, TEventParamType> : ISubscriberWrapper<TEventParamType>
        where TEventParamType : BaseDispatcherEventParams
        where THandler : class
    {
        #region DataWrapping
        /// <summary> Слабая ссылка на подписавшийся объект, нужна, чтобы проверить не умер ли объект, пока ждал вызов эвента </summary>
        public readonly WeakReference<THandler> HandlerWR;

        /// <summary> Слабая ссылка на замыкание с методом, который нужно вызвать у объекта </summary>
        public readonly Func<THandler, Action<object, TEventParamType>> CallbackProvider;

        /// <summary> Хэш объекта, нужен, чтобы сортировать и идентифицировать подписавшийся объект </summary>
        private readonly int _instanceHash;


        /// <summary> Создать обёртку над подписавшимся на событие объектом <see cref="IDispatcher.WeakSubscribe{T}"/>  </summary>
        /// <param name="eventCallbackHash">Хэш - идентификатор подписавшегося объекта который будет использоваться при вызове <see cref="GetHashCode"/> данной обёртки</param>
        /// <param name="handler">Ссылка на подписавшийся объект</param>
        /// <param name="callback">Колбэк, который будет вызван при создании эвента <see cref="IDispatcher.Rise{T}"/></param>
        public SubscriberWrapperWeak(int eventCallbackHash, THandler handler, Func<THandler, Action<object, TEventParamType>> callbackProvider)
        {
            HandlerWR = new WeakReference<THandler>(handler);
            CallbackProvider = callbackProvider;
            _instanceHash = eventCallbackHash;
        }
        #endregion


        #region ISubscriberWrapper<>
        /// <inheritdoc />
        public int WrapperId => _instanceHash;

        /// <inheritdoc />
        public bool IsAlive()
        {
            return HandlerWR.TryGetTarget(out var handler);
        }

        /// <inheritdoc />
        public void Invoke(object source, TEventParamType eventParam)
        {
            if (!HandlerWR.TryGetTarget(out var handler))
                throw new NullReferenceException("Объект, хронящийся по слабой ссылке был уничтожен, невозможно вызввать колбэк");

            Action<object, TEventParamType> eventCallback = CallbackProvider.Invoke(handler);
            eventCallback.Invoke(source, eventParam);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _instanceHash;
        }
        #endregion
    }
}