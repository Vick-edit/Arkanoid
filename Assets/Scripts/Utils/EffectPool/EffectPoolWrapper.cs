using System;
using UnityEngine;

namespace Utils.EffectPool
{
    /// <summary>
    ///     Враппера над <see cref="IEffectPool"/>, который обеспечивает то, что в проекте будет один и только один пул эффектов
    /// 
    ///     Построен ввиде расширяющих методов к объектам и singelton'a
    /// </summary>
    public static class EffectPoolWrapper
    {
        private static readonly Lazy<IEffectPool> LazyInstance = new Lazy<IEffectPool>(() => new EffectPool());

        /// <summary> Singleton точка доступа к реализации <see cref="IEffectPool"/> </summary>
        public static IEffectPool Instance => LazyInstance.Value;

        #region StaticAccess
        /// <summary> Зарегистрировать запрос на отрисовыку эффекта </summary>
        /// <param name="effectPrefab">Префаб на базе которого будет строиться эффект</param>
        /// <param name="position">Позиция, где должен появиться эффект</param>
        /// <param name="rotation">Ориентация эффекта в пространстве</param>
        public static void AddEffectRequest(this MonoBehaviour monoBehaviour, ParticleSystem effectPrefab, Vector3 position, Quaternion rotation)
        {
            Instance.AddEffectRequest(effectPrefab, position, rotation);
        }

        /// <summary> Зарегистрировать запрос на отрисовыку эффекта </summary>
        /// <param name="monoBehaviour"><see cref="MonoBehaviour"/> к объекту которого должен быть прикреплён эффект</param>
        /// <param name="effectPrefab">Префаб на базе которого будет строиться эффект</param>
        /// <param name="position">Позиция в локальных координатах родителя, где должен появиться эффект</param>
        /// <param name="rotation">Ориентация эффекта в локальных координатах родителя</param>
        public static void AddEffectRequestWithAttaching(this MonoBehaviour monoBehaviour, ParticleSystem effectPrefab, Vector3 position, Quaternion rotation)
        {
            Instance.AddEffectRequest(effectPrefab, monoBehaviour.gameObject, position, rotation);
        }

        /// <summary> Создать, либо вытащить эффект из закешированыых, привести в активное состояние и вернуть, пул больше не будет отслеживать этот эффект </summary>
        /// <param name="monoBehaviour"><see cref="MonoBehaviour"/> к объекту которого должен быть прикреплён эффект</param>
        /// <param name="effectPrefab">Префаб на базе которого будет строиться эффект</param>
        /// <param name="position">Позиция, где должен появиться эффект</param>
        /// <param name="rotation">Ориентация эффекта в пространстве</param>
        /// <returns>Активированный эффект</returns>
        public static ParticleSystem GetEffect(this MonoBehaviour monoBehaviour, ParticleSystem effectPrefab, Vector3 position, Quaternion rotation)
        {
            return Instance.GetEffect(effectPrefab, position, rotation, monoBehaviour.transform);
        }
        #endregion
    }
}