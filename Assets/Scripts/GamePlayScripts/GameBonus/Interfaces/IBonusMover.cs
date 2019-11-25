namespace GamePlayScripts.GameBonus.Interfaces
{
    /// <summary>
    ///     Интерфейс обработки фищики бонуса
    /// </summary>
    public interface IBonusMover
    {
        /// <summary> Сделать следующее перемещение бонуса с учётом времени с последнего перемещения </summary>
        /// <param name="movementDurationInSeconds">Количество секунд, которое нужно учесть, то есть время в течении которого двигался шарик</param>
        /// <returns>Флаг был ли бонус пойман панелькой</returns>
        bool MoveBonusDown(float movementDurationInSeconds);
    }
}