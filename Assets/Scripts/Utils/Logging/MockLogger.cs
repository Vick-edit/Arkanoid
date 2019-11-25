using System;

namespace Utils.Logging
{
    /// <summary>
    ///     Класс заглушка над логгером <see cref="ILogger"/> может применяться, когда нет возможности/необходимости выводить сообщения, а только создавать видимость их обработки
    /// </summary>
    public class MockLogger : ILogger
    {
        /// <inheritdoc />
        public void LogInfo(string errorText){}

        /// <inheritdoc />
        public void LogWarning(string errorText){}

        /// <inheritdoc />
        public void LogError(string errorText){}

        /// <inheritdoc />
        public void LogError(Exception exception){}
    }
}