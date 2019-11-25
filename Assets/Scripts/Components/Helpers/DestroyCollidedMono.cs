using System;
using System.Linq;
using UnityEngine;

namespace Components.Helpers
{
    /// <summary>
    ///     Компонент, уничтожающий объект, попавшие в его зону
    /// </summary>
    public class DestroyCollidedMono : MonoBehaviour
    {
        [SerializeField] private LayerMask _collisionMask;
        [SerializeField] private SpriteRenderer _boxSprite;

        private readonly Collider2D[] _collisionContainer = new Collider2D[100];
        private Vector2 _objectCenter;
        private Vector2 _objectSize;

        private void Awake()
        {
            _objectCenter = transform.position;
            _objectSize = _boxSprite.size; 
        }

        private void FixedUpdate()
        {
            var collisions = Physics2D.OverlapBoxNonAlloc(_objectCenter, _objectSize, 0, _collisionContainer, _collisionMask);
            if(collisions == 0)
                return;
            if(collisions > _collisionContainer.Length)
                throw new Exception("Размер контенера коллизий слишком мал");

            var collidedObjects = _collisionContainer
                .Where(x => x != null && x.gameObject != null)
                .Select(x => x.gameObject);
            foreach (var collidedObject in collidedObjects)
            {
                Destroy(collidedObject);
            }
        }
    }
}