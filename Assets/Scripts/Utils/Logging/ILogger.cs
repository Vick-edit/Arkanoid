using System;

namespace Utils.Logging
{
    /// <summary>
    ///     Интерфейс логгера сообщений
    /// </summary>
    public interface ILogger
    {
        /// <summary> Зарегистрировать информационное сообщение </summary>
        void LogInfo(string errorText);

        /// <summary> Сообщить о событие, которое не является ошибкой, но может оказать влияние на дальнейшую стабильную работу приложения </summary>
        void LogWarning(string errorText);

        /// <summary> Сообщить о сбое в работе приложения </summary>
        void LogError(string errorText);

        /// <summary> Сообщить о возникшей ошибке </summary>
        void LogError(Exception exception);
    }
}