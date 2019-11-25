using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Dispatcher.EventParameters;
using Utils.Dispatcher.SubscriberWrappers;

namespace Utils.Dispatcher
{
        /// <summary>
        ///     Базовая и единственная реализация <see cref="IDispatcher"/>
        /// </summary>
        internal class Dispatcher : IDispatcher
        {
            /// <summary> Скрытый конструктор, который должен вызываться только из враппера </summary>
            internal Dispatcher(){}

            /// <summary> Токен для обеспечения потокобезопасности </summary>
            private static readonly object AccessToken = new object();

            /// <summary> Словарь Тип параметра эвента - список хэндлеров подробнее почему выбранно такое хронилище можно посмотреть: <see cref="IDispatcher"/></summary>
            private readonly Dictionary<Type, List<ISubscriberWrapper>> _eventHandlers = new Dictionary<Type, List<ISubscriberWrapper>>();
            


            /// <inheritdoc />
            public void Subscribe<T>(object handler, Action<object, T> callback) where T : BaseDispatcherEventParams
            {
                //валидация входных параметров на предмет возможности создания подписки на событие
                ValidateSubscriptionParameters(handler, callback);

                //добавление записи о новом хэндлере эвента, только если уже нет подписчика на этот эвент с таким ID
                var eventParamType = typeof(T);
                var subscriptionId = GetHashForSubscription(handler, callback);
                var allSubscribersWithId = _eventHandlers[eventParamType].Where(x => x.WrapperId == subscriptionId);
                if (!allSubscribersWithId.Any())
                {
                    lock (AccessToken)
                    {
                        //создаём обёртку над хэндлером эвента с жёсткой ссылкой
                        ISubscriberWrapper<T> subscriptionWrapper = new SubscriberWrapperHard<T>(subscriptionId, handler, callback);
                        _eventHandlers[eventParamType].Add(subscriptionWrapper);
                    }
                }
            }

            /// <inheritdoc />
            public void WeakSubscribe<H, T>(H handler, Func<H, Action<object, T>> callbackProvider)
                where T : BaseDispatcherEventParams
                where H : class
            {
                //валидация входных параметров на предмет возможности создания подписки на событие
                var callback = callbackProvider.Invoke(handler);
                ValidateSubscriptionParameters(handler, callback);

                //добавление записи о новом хэндлере эвента, только если уже нет подписчика на этот эвент с таким ID
                var eventParamType = typeof(T);
                var subscriptionId = GetHashForSubscription(handler, callback);
                var allSubscribersWithId = _eventHandlers[eventParamType].Where(x => x.WrapperId == subscriptionId);
                if (!allSubscribersWithId.Any())
                {
                    lock (AccessToken)
                    {
                        //создаём обёртку над хэндлером эвента со слабой ссылкой, чтобы не блокировать GC
                        ISubscriberWrapper<T> subscriptionWrapper = new SubscriberWrapperWeak<H, T>(subscriptionId, handler, callbackProvider);
                        _eventHandlers[eventParamType].Add(subscriptionWrapper);
                    }
                }
            }

            /// <inheritdoc />
            public bool Unsubscribe<T>(object handler, Action<object, T> callback) where T : BaseDispatcherEventParams
            {
                //валидация входных параметров на предмет возможности одписки от события
                ValidateSubscriptionParameters(handler, callback, false);

                //удаление хэндлера из подписавшихся обработчиков события по его ID - типу параметра эвента
                var eventParamType = typeof(T);
                if(!_eventHandlers.ContainsKey(eventParamType))
                    return false;
                var subscriptionId = GetHashForSubscription(handler, callback);                     
                var allSubscribersWithId = _eventHandlers[eventParamType]
                    .Where(x => x.WrapperId == subscriptionId)
                    .ToList();
                if(!allSubscribersWithId.Any())
                    return false;
                if(allSubscribersWithId.Count > 1)
                    throw new Exception("При попытке отписаться от события было обнаружено более одной сущности, соответствующей входным параметрам");
                lock (AccessToken)
                {
                    _eventHandlers[eventParamType].Remove(allSubscribersWithId.Single());
                }
                return true;
            }

            /// <inheritdoc />
            public bool Rise<T>(object source, T eventParam) where T : BaseDispatcherEventParams
            {
                //валидация входных параметров
                var eventParamType = typeof(T);
                if (source == null)
                    throw new NullReferenceException($"Источник эвента '{nameof(source)}' не может быть null");
                if (eventParam == null)
                    throw new NullReferenceException($"Параметры эвента '{nameof(eventParam)}' не могут быть null");
                if (eventParamType.IsAbstract)
                    throw new NullReferenceException(
                        $"Тип данных эвента не может быть абстрактным классом '{eventParamType}'");

                //проверка, что у этого события есть подписчики
                if (!_eventHandlers.ContainsKey(eventParamType))
                    return false;
                //выбираем всех подписчиков и вызываем на них эвент, если они живы и удаляем их, если они умерли
                var isEventWasCalled = false;
                var allEventHandlers = _eventHandlers[eventParamType].ToArray(); //Работаем с копией, т.к. реакция на эвент может изменить список подписчиков
                var notAliveObjects = new List<ISubscriberWrapper>();
                foreach (var eventHandlerWrapper in allEventHandlers)
                {
                    if (eventHandlerWrapper.IsAlive())
                    {
                        var castedWrapper = (ISubscriberWrapper<T>) eventHandlerWrapper;
                        castedWrapper.Invoke(source, eventParam);
                        isEventWasCalled = true;
                    }
                    else
                    {
                        notAliveObjects.Add(eventHandlerWrapper);
                    }
                }

                lock (AccessToken)
                {
                    foreach (var notAliveHandler in notAliveObjects)
                        _eventHandlers[eventParamType].Remove(notAliveHandler);
                }

                return isEventWasCalled;
            }


            private int GetHashForSubscription<T>(object handler, Action<object, T> callback) where T : BaseDispatcherEventParams
            {
                var handlerTypeName = handler.GetType().FullName ?? "";
                var callbackName = callback.Method.Name;

                /*
                 * Microsoft не гарантирует, что GetHashCode вернёт разные значения для объектов разных типов, поэтому
                 * используем в качестве дополнительной "степени свободы" полное имя типа
                 * Ну и Хэш из нескольких хэшей принято вычислять ксором, но это всё равно не даёт 100% гарантии
                 * Но пока оставим так
                 */
                return handlerTypeName.GetHashCode() ^ handler.GetHashCode() ^ callbackName.GetHashCode();
            }

            private void ValidateSubscriptionParameters<T>(object handler, Action<object, T> callback, bool createNewEventGroup = true) where T : BaseDispatcherEventParams
            {
                //валидация входных параметров
                var eventParamType = typeof(T);
                if (handler == null)
                    throw new NullReferenceException($"Хнэлер эвента '{nameof(handler)}' не может быть null");
                if (callback == null)
                    throw new NullReferenceException($"Колбэк эвента '{nameof(callback)}' не может быть null");
                if (eventParamType.IsAbstract)
                    throw new NullReferenceException($"Тип данных эвента не может быть абстрактным классом '{eventParamType}'");

                //создание точки расширения в словаре, если это новый эвент
                if (createNewEventGroup && !_eventHandlers.ContainsKey(eventParamType))
                    _eventHandlers.Add(eventParamType, new List<ISubscriberWrapper>());
            }
        }
}