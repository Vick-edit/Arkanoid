using ScriptableObjects;
using UnityEngine;
using Utils.Dispatcher.EventParameters;

namespace GameEventParams
{
    /// <summary>
    ///     Параметр эвента, при разрушении кирпича от столкновения с шариком
    /// </summary>
    public class BrickWasDestroyedEventParams : BaseDispatcherEventParams
    {
        /// <summary> Сколько очков стоил этот кирпичик </summary>
        public int ScoreCost { get; }

        /// <summary> Координаты положения кирпичика </summary>
        public Vector3 BrickPosition { get; }

        /// <inheritdoc />
        public BrickWasDestroyedEventParams(int scoreCost, Vector3 brickPosition)
        {
            ScoreCost = scoreCost;
            BrickPosition = brickPosition;
        }
    }
}