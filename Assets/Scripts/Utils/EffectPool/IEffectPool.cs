using UnityEngine;

namespace Utils.EffectPool
{
    /// <summary>
    ///     Интерфейс пула эффектов. По задумке, когда нужен эффект мы не создаём новый,
    ///     а обращаемся к пулу, он регистрирует наш запрос, а потом, когда приходит команда
    ///     он начинает их отрисовывать в соответствии с поступившими запросами, но только в пределах установленного времени
    ///     в <see cref="Initialize(GameObject, float)"/>, если за это время он не успеет воспроизвести все эффекты,
    ///     то он попытается сделать этой в следующее окно, нужно это, чтобы не создавать просадок в FPS из-за обилия понадобившихся эффектов
    ///     в какой-то момент времени, если эффект задержится на пару-тройку кадров, то ничего страшного, главное, если эффект должен двигаться за объектом
    ///     указать, что координаты нужно использовать локальные, чтобы не создалось смещение эффекта из-за задержки отрисовки
    /// </summary>
    public interface IEffectPool
    {
        /// <summary> Инициализировать пул, без предварительной инициализации пул может работать некорректно </summary>
        /// <param name="gameObject">Объект в который будут складываться все эффекты</param>
        /// <param name="iterationTime">Время в секундах, которое не должен привысить пул, пока будет отрисовывать эффекты</param>
        void Initialize(GameObject gameObject, float iterationTime);

        /// <summary> Зарегистрировать запрос на отрисовыку эффекта </summary>
        /// <param name="effectPrefab">Префаб на базе которого будет строиться эффект</param>
        /// <param name="position">Позиция, где должен появиться эффект</param>
        /// <param name="rotation">Ориентация эффекта в пространстве</param>
        void AddEffectRequest(ParticleSystem effectPrefab, Vector3 position, Quaternion rotation);

        /// <summary> Зарегистрировать запрос на отрисовыку эффекта </summary>
        /// <param name="effectPrefab">Префаб на базе которого будет строиться эффект</param>
        /// <param name="parentForEffect"><see cref="GameObject"/> к которому должен быть прикреплён эффект</param>
        /// <param name="position">Позиция в локальных координатах родителя, где должен появиться эффект</param>
        /// <param name="rotation">Ориентация эффекта в локальных координатах родителя</param>
        void AddEffectRequest(ParticleSystem effectPrefab, GameObject parentForEffect, Vector3 position, Quaternion rotation);

        /// <summary> Создать, либо вытащить эффект из закешированыых, привести в активное состояние и вернуть, пул больше не будет отслеживать этот эффект </summary>
        /// <param name="effectPrefab">Префаб на базе которого будет строиться эффект</param>
        /// <param name="position">Позиция, где должен появиться эффект</param>
        /// <param name="rotation">Ориентация эффекта в пространстве</param>
        /// <param name="parent"><see cref="GameObject"/> к которому должен быть прикреплён эффект</param>
        /// <returns>Активированный эффект</returns>
        ParticleSystem GetEffect(ParticleSystem effectPrefab, Vector3 position, Quaternion rotation, Transform parent);

        /// <summary> Обработать все запросы на отрисовку эффектов </summary>
        /// <returns> Число отрисованных эффектов </returns>
        int VisualizeRequestedEffects();
    }
}