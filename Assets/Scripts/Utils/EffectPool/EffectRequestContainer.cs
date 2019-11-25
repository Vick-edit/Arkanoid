using System;
using UnityEngine;

namespace Utils.EffectPool
{
    /// <summary>
    /// Контейнер данных о запросе на отрисовку 
    /// </summary>
    internal class EffectRequestContainer
    {
        /// <summary> Префаб эффекта </summary>
        public ParticleSystem EffectPrefab { get; }
        

        /// <summary> Нужно ли у задать эффекту родителя из запроса </summary>
        public bool IsEffectHaseOwnParrent => _parentWeakReference != null;

        private readonly WeakReference<Transform> _parentWeakReference;
        /// <summary> Родитель к которому должен быть прикреплён эффект </summary>
        public Transform ParentTransform
        {
            get
            {
                Transform parentGameObject = null;
                _parentWeakReference?.TryGetTarget(out parentGameObject);
                return parentGameObject;
            }
        }


        private Vector3 _position;
        /// <summary> Позиция эффекта в мировых кординатах, которые высчитываются относительно родителя, если нужно </summary>
        public Vector3 Position
        {
            get
            {
                if (IsEffectHaseOwnParrent)
                    return ParentTransform.TransformPoint(_position);
                return _position;
            }
        }

        private Quaternion _rotation;
        /// <summary> Ориентация эффекта в мировых кординатах, которые высчитываются относительно родителя, если нужно </summary>
        public Quaternion Rotation
        {
            get
            {
                if (IsEffectHaseOwnParrent)
                    return ParentTransform.rotation * _rotation;
                return _rotation;
            }
        }


        public EffectRequestContainer(ParticleSystem effectPrefab, Vector3 position, Quaternion rotation)
        {
            EffectPrefab = effectPrefab;
            _position = position;
            _rotation = rotation;
        }

        public EffectRequestContainer(ParticleSystem effectPrefab, GameObject parent, Vector3 position, Quaternion rotation)
        {
            EffectPrefab = effectPrefab;
            _position = position;
            _rotation = rotation;
            _parentWeakReference = new WeakReference<Transform>(parent.transform);
        }
    }
}