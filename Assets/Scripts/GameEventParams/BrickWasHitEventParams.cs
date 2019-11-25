using Utils.Dispatcher.EventParameters;

namespace GameEventParams
{
    /// <summary>
    ///     Параметры для событий столкновения 
    /// </summary>
    public class BrickWasHitEventParams : BaseDispatcherEventParams
    {
        /// <summary> Число очков за попадание по кирпичику </summary>
        public int HitScore { get; }


        /// <inheritdoc />
        public BrickWasHitEventParams(int hitScore)
        {
            HitScore = hitScore;
        }
    }
}