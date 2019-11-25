using System;
using Utils.Dispatcher.EventParameters;

namespace Utils.Dispatcher.SubscriberWrappers
{
    /// <summary>
    ///     Враппер, который оборачивает ссылку на сам объект и на замыкание с колбэком без использования <see cref="WeakReference"/>
    /// </summary>
    internal sealed class SubscriberWrapperHard<TEventParamType> : ISubscriberWrapper<TEventParamType>
        where TEventParamType : BaseDispatcherEventParams
    {
        #region DataWrapping
        /// <summary> Ссылка на подписавшийся объект, нужна, чтобы проверить не умер ли объект, пока ждал вызов эвента </summary>
        public readonly object Handler;

        /// <summary> Ссылка на замыкание с методом, который нужно вызвать у объекта </summary>
        public readonly Action<object, TEventParamType> EventCallback;

        /// <summary> Хэш объекта, нужен, чтобы сортировать и идентифицировать подписавшийся объект </summary>
        private readonly int _instanceHash;


        /// <summary> Создать обёртку над подписавшимся на событие объектом <see cref="IDispatcher.WeakSubscribe{T}"/>  </summary>
        /// <param name="eventCallbackHash">Хэш - идентификатор подписавшегося объекта который будет использоваться при вызове <see cref="GetHashCode"/> данной обёртки</param>
        /// <param name="handler">Ссылка на подписавшийся объект</param>
        /// <param name="callback">Колбэк, который будет вызван при создании эвента <see cref="IDispatcher.Rise{T}"/></param>
        public SubscriberWrapperHard(int eventCallbackHash, object handler, Action<object, TEventParamType> callback)
        {
            Handler = handler;
            EventCallback = callback;

            _instanceHash = eventCallbackHash;
        }
        #endregion
        

        #region ISubscriberWrapper<>
        /// <inheritdoc />
        public int WrapperId => _instanceHash;

        /// <inheritdoc />
        public bool IsAlive()
        {
            return Handler != null && EventCallback != null;
        }

        /// <inheritdoc />
        public void Invoke(object source, TEventParamType eventParam)
        {
            EventCallback.Invoke(source, eventParam);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _instanceHash;
        }
        #endregion

    }
}