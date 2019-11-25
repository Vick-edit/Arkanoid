using GamePlayScripts.PlayerBallController;

namespace GamePlayScripts.BrickController
{
    /// <summary>
    ///    Контроллер со всей логикой работы кирпичика
    /// </summary>
    public interface IBrickController
    {
        /// <summary> Задать начальное состояние кирпичика </summary>
        void InitBrick();

        /// <summary> Обработка удара шариком по кирпичику </summary>
        /// <param name="ballController">Объект шарика с которым произошло столкновение</param>
        void OnBallHit(IPlayerBallController ballController);
    }
}