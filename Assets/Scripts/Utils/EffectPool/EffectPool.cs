using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils.EffectPool
{
    internal class EffectPool : IEffectPool
    {
        private readonly Stopwatch _visualizationTimer = new Stopwatch();
        private readonly Queue<EffectRequestContainer> _effectsRequestQue = new Queue<EffectRequestContainer>();
        private readonly Dictionary<int, List<ParticleSystem>> _inactiveEffects = new Dictionary<int, List<ParticleSystem>>();
        private readonly Dictionary<int, List<ParticleSystem>> _playingEffects = new Dictionary<int, List<ParticleSystem>>();

        private Transform _effectContainer;
        private double _iterationTime = double.NaN;
        private bool _isInitialized;

        /// <inheritdoc />
        public void Initialize(GameObject gameObject, float iterationTime)
        {
            _effectContainer = gameObject.transform;
            _iterationTime = iterationTime;
            _isInitialized = true;
        }

        /// <inheritdoc />
        public void AddEffectRequest(ParticleSystem effectPrefab, Vector3 position, Quaternion rotation)
        {
            CheckInitialization();
            var newEffectRequest = new EffectRequestContainer(effectPrefab, position, rotation);
            _effectsRequestQue.Enqueue(newEffectRequest);
        }

        /// <inheritdoc />
        public void AddEffectRequest(ParticleSystem effectPrefab, GameObject parentForEffect, Vector3 position, Quaternion rotation)
        {
            CheckInitialization();
            var newEffectRequest = new EffectRequestContainer(effectPrefab, parentForEffect, position, rotation);
            _effectsRequestQue.Enqueue(newEffectRequest);
        }

        /// <inheritdoc />
        public ParticleSystem GetEffect(ParticleSystem effectPrefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            CheckInitialization();

            var particlesEffect = GetParticleSystemNewOrCached(effectPrefab, position, rotation, parent);
            particlesEffect.Play(true);
            return particlesEffect;
        }

        /// <inheritdoc />
        public int VisualizeRequestedEffects()
        {
            CheckInitialization();
            _visualizationTimer.Restart();

            foreach (var playingEffectsGroup in _playingEffects)
            {
                var allEffectInstances = playingEffectsGroup.Value;
                var effectsCount = allEffectInstances.Count;
                for (var i = effectsCount - 1; i >= 0; i--)
                {
                    var effectInstance = allEffectInstances[i];
                    if (effectInstance.IsAlive())
                        continue;

                    allEffectInstances.Remove(effectInstance);
                    effectInstance.transform.SetParent(_effectContainer);
                    _inactiveEffects[playingEffectsGroup.Key].Add(effectInstance);

                    //Если время истекло, пока мы перекладывали активные эффекты обратно в пул, то, что поделать - прерываем итерацию
                    if (_visualizationTimer.Elapsed.TotalSeconds > _iterationTime)
                        return 0;
                }
            }

            var initializedEffects = 0;
            while (_effectsRequestQue.Count > 0)
            {
                var effectRequest = _effectsRequestQue.Dequeue();
                var particlesEffect = GetParticleSystemNewOrCached(effectRequest);
                PutToActivePooledEffects(effectRequest.EffectPrefab, particlesEffect);
                particlesEffect.Play(true);
                initializedEffects++;

                //Если время истекло, то прерываем отрисовку эффектов
                if (_visualizationTimer.Elapsed.TotalSeconds > _iterationTime) { }
                break;
            }

            _visualizationTimer.Stop();
            return initializedEffects;
        }




        private ParticleSystem GetParticleSystemNewOrCached(EffectRequestContainer effectRequest)
        {
            return GetParticleSystemNewOrCached(effectRequest.EffectPrefab, effectRequest.Position, effectRequest.Rotation, effectRequest.ParentTransform);
        }

        private ParticleSystem GetParticleSystemNewOrCached(ParticleSystem effectPrefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var effectId = effectPrefab.GetInstanceID();
            if (!_inactiveEffects.ContainsKey(effectId))
                _inactiveEffects.Add(effectId, new List<ParticleSystem>());
            var cachedEffects = _inactiveEffects[effectId];

            var freeEffect = cachedEffects.LastOrDefault();
            if (freeEffect == null)
                freeEffect = Object.Instantiate(effectPrefab, _effectContainer);
            else
                cachedEffects.Remove(freeEffect);

            var effectTransform = freeEffect.gameObject.transform;
            effectTransform.position = position;
            effectTransform.rotation = rotation;
            if (parent != null)
                effectTransform.SetParent(parent, true);
            return freeEffect;
        }
        private void PutToActivePooledEffects(ParticleSystem effectPrefab, ParticleSystem particlesEffect)
        {
            var effectId = effectPrefab.GetInstanceID();
            if (!_playingEffects.ContainsKey(effectId))
                _playingEffects.Add(effectId, new List<ParticleSystem>());
            _playingEffects[effectId].Add(particlesEffect);
        }

        private void CheckInitialization()
        {
            if(!_isInitialized)
                throw new Exception("Невозможно пользоваться пулом эффектов до его инициализации");
        }
    }
}