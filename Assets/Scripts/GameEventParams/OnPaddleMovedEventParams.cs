using Utils.Dispatcher.EventParameters;

namespace GameEventParams
{
    /// <summary>
    ///     Параметры для событий о смещении панельки пользователя
    /// </summary>
    public class OnPaddleMovedEventParams : BaseDispatcherEventParams
    {
        public float NewXCoordinate { get; }

        public OnPaddleMovedEventParams(float newXCoordinate)
        {
            NewXCoordinate = newXCoordinate;
        }
    }
}