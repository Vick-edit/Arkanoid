using System.Linq;
using GameEventParams;
using GamePlayScripts.GameBonus.DataModels;
using GamePlayScripts.PlayerBallController.DataModels;
using UnityEngine;
using Utils.Dispatcher;

namespace Components.GamePlayComponents
{
    /// <summary>
    ///     Компонент управления бонусами
    /// </summary>
    public class BonusManagerMono : MonoBehaviour
    {
        public Transform PlayerBallContainer;
        public PlayerPaddleMono PlayerPaddle;

        [SerializeField] private BonusChanceContainer[] _bonuseWithDropRate = new BonusChanceContainer[5];



        private Transform _myTransform;
        private int _allRangesSum;

        private void Awake()
        {
            PlayerPaddle = PlayerPaddle ? PlayerPaddle : FindObjectOfType<PlayerPaddleMono>();
            PlayerBallContainer = PlayerBallContainer ? PlayerBallContainer : PlayerPaddle.transform.parent;

            _myTransform = transform;
            _allRangesSum = _bonuseWithDropRate.Sum(x => x.ChanceValue);
            this.WeakSubscribe<BonusManagerMono, BrickWasDestroyedEventParams>(x => x.OnBrickDestroyed);
        }

        private void OnDestroy()
        {
            this.Unsubscribe<BrickWasDestroyedEventParams>(OnBrickDestroyed);
        }

        private void OnBrickDestroyed(object source, BrickWasDestroyedEventParams eventParams)
        {
            var spawnPosition = eventParams.BrickPosition;

            var cumulativePart = 0;
            var randomValue = Random.Range(0, _allRangesSum);
            foreach (var bonusContainer in _bonuseWithDropRate)
            {
                cumulativePart += bonusContainer.ChanceValue;
                if (cumulativePart >  randomValue)
                {
                    if (bonusContainer.BonusPrefab != null)
                    {
                        var bonus = Instantiate(bonusContainer.BonusPrefab, spawnPosition, Quaternion.identity, _myTransform);
                        bonus.BonusManager = this;
                    }
                    return;
                }
            }
        }
    }
}