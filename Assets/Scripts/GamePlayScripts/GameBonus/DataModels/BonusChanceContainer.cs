using System;
using Components.GamePlayComponents.Bonuses;

namespace GamePlayScripts.GameBonus.DataModels
{
    /// <summary>
    ///     Класс - контейнер бонусов с их вероятностью срабатывания
    /// </summary>
    [Serializable]
    public class BonusChanceContainer
    {
        public int ChanceValue;
        public BaseBonusMono BonusPrefab;
    }
}