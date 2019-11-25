using System;
using UnityEngine;

namespace Utils.Logging
{
    /// <summary>
    ///     Класс - обёртка вокруг обычного логера ошибок из Unity - позволяет создавать дополнительное представление данных
    /// </summary>
    public class UnityLogger : ILogger
    {
        /// <inheritdoc />
        public void LogInfo(string errorText)
        {
            Debug.Log(errorText);
        }

        /// <inheritdoc />
        public void LogWarning(string errorText)
        {
            Debug.LogWarning(errorText);
        }

        /// <inheritdoc />
        public void LogError(string errorText)
        {
            Debug.LogError(errorText);
        }

        /// <inheritdoc />
        public void LogError(Exception exception)
        {
            Debug.LogError(exception);
        }
    }
}