using System;
using Tools;
using UnityEngine;
using Utils.EffectPool;

namespace Components.GamePlayComponents
{
    /// <summary>
    ///     Компонент, отвечающий за хронение и обновление <see cref="IEffectPool"/>
    /// </summary>
    public class EffectManagerMono : MonoBehaviour
    {
        private const float FLOAT_TOLERANCE = 0.0001f;

        [SerializeField, Header("Сколько времени в кадр можно тратить на эффекты")]
        private float _secondsForEffectsPerFrame = 0.004f;
        private float _prevTimeSetting;
        private IEffectPool _effectPool;

        private void Awake()
        {
            InitEffectPoolInstance();
        }

        private void Update()
        {
            if (Math.Abs(_prevTimeSetting - _secondsForEffectsPerFrame) > FLOAT_TOLERANCE)
                InitEffectPoolInstance();
            _effectPool.VisualizeRequestedEffects();
        }

        private void InitEffectPoolInstance()
        {
            _effectPool = _effectPool ?? DependencyResolver.GetCachedEffectPool();
            _effectPool.Initialize(gameObject, _secondsForEffectsPerFrame);
            _prevTimeSetting = _secondsForEffectsPerFrame;
        }
    }
}