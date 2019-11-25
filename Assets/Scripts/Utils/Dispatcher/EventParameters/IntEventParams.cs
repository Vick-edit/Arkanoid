namespace Utils.Dispatcher.EventParameters
{
    /// <summary>
    ///     Пример простого эвента, когда нужно передать только одно число в качестве параметра эвента
    /// </summary>
    public class IntEventParams : BaseDispatcherEventParams
    {
        public int Value { get; set; }

        /// <inheritdoc />
        public IntEventParams(){}

        /// <inheritdoc />
        public IntEventParams(int value)
        {
            Value = value;
        }
    }
}