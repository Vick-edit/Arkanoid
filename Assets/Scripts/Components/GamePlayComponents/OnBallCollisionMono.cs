using GamePlayScripts.PlayerBallController;
using UnityEngine;
using UnityEngine.Events;

namespace Components.GamePlayComponents
{
    /// <summary>
    ///     Компонент в который могут быть переданы сведения о том, что произошла коллизия с мячиком пользователя
    /// </summary>
    public class OnBallCollisionMono : MonoBehaviour
    {
        public OnBallCollisionEvent OnBallCollision = new OnBallCollisionEvent();

        [System.Serializable]
        public class OnBallCollisionEvent : UnityEvent<IPlayerBallController>
        {
        }
    }
}