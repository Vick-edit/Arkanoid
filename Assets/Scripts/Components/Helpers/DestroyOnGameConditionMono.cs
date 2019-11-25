using GameEventParams;
using UnityEngine;
using Utils.Dispatcher;

namespace Components.Helpers
{

    /// <summary>
    ///     Компонент, уничтожающий объект, на событие сброса уровня
    /// </summary>
    public class DestroyOnGameConditionMono : MonoBehaviour
    {
        [SerializeField] private GameManagementEvent.GameEvents _conditionToDestroy;

        private void Awake()
        {
            this.WeakSubscribe<DestroyOnGameConditionMono, GameManagementEvent>(x => x.DestroyOnReset);
        }

        private void OnDestroy()
        {
            this.Unsubscribe<GameManagementEvent>(DestroyOnReset);
        }


        private void DestroyOnReset(object source, GameManagementEvent gameManagementEvent)
        {
            if (gameManagementEvent.GameEvent == _conditionToDestroy)
                DestroyImmediate(this.transform.gameObject);
        }
    }
}