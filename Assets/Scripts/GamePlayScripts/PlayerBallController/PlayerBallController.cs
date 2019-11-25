using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Components.GamePlayComponents;
using GamePlayScripts.PlayerBallController.DataModels;
using UnityEditor;
using UnityEngine;
using Utils.Dispatcher;

namespace GamePlayScripts.PlayerBallController
{
    public class PlayerBallController : IPlayerBallController
    {
        private const float MINIMUM_MOVEMENT_STEP = 0.01f;      //Движения меньше этой величины не считаются

        private readonly Collider2D[] _collidedObjects = new Collider2D[100];
        private readonly PlayerBallParameters _monoBehaviourParameters;

        /// <inheritdoc />
        public Transform BallTransform => _monoBehaviourParameters.BallTransform;


        /// <inheritdoc />
        public PlayerBallController(PlayerBallParameters monoBehaviourParameters)
        {
            _monoBehaviourParameters = monoBehaviourParameters;
        }

        /// <inheritdoc />
        public List<GameObject> CheckCollision()
        {
            var ballRadius = _monoBehaviourParameters.BallRadius;
            var collisionMask = _monoBehaviourParameters.CollisionMask;
            var collisionCount = Physics2D.OverlapCircleNonAlloc(BallTransform.position, ballRadius, _collidedObjects, collisionMask);
            if(collisionCount == 0)
                return new List<GameObject>();

            if (collisionCount > _collidedObjects.Length)
                throw new Exception("Произшла потеря данных, т.к. массив для подсчёта объектов с которыми столкнулся меньше числа объектов с которыми реально столкнулся мячик");

            var collidedObjects = _collidedObjects
                .Where(x => x != null)
                .Select(x => x.gameObject)
                .ToList();

            return collidedObjects;
        }

        /// <inheritdoc />
        public bool MoveBallForward(float movementDurationInSeconds)
        {
            //Если вектор движения не задан, то просто проверяем статически коллизии
            if (_monoBehaviourParameters.NormalizedMovementVector == Vector2.zero)
                return CheckCollisionsWithoutMovement();

            //Если движение было слишком маленьким, то считаем, что его и не было
            var movementDistance = movementDurationInSeconds * _monoBehaviourParameters.BallSpeed;
            if(movementDistance < MINIMUM_MOVEMENT_STEP)
                return CheckCollisionsWithoutMovement();

            //Иначе двигаем объект с учётом коллизий
            return MoveWithCollisionCorrection(movementDistance);
        }

        /// <inheritdoc />
        public void ChangeSpeed(float speedModificator)
        {
            var newSpeed = _monoBehaviourParameters.BallSpeed + speedModificator;
            newSpeed = Mathf.Clamp(newSpeed, _monoBehaviourParameters.MinBallSpeed, _monoBehaviourParameters.MaxBallSpeed);
            _monoBehaviourParameters.BallSpeed = newSpeed;
        }

        private bool MoveWithCollisionCorrection(float movementDistance)
        {
            //Просчитываем потенциальное перемещение шарика
            var startPosition = _monoBehaviourParameters.BallTransform.position;
            var movementVector = movementDistance * _monoBehaviourParameters.NormalizedMovementVector;

            //Проверяем нет ли препятствий на пути, для осуществления такого преемещения
            var ballRadius = _monoBehaviourParameters.BallRadius;
            var raycastDirection = _monoBehaviourParameters.NormalizedMovementVector;
            var collisionMask = _monoBehaviourParameters.CollisionMask;
            var circleCastHit = Physics2D.CircleCast(startPosition, ballRadius, raycastDirection, movementDistance, collisionMask);
            var hitCollider = circleCastHit.collider;
            if (hitCollider == null)
            {
                //если припятствий не было, просто двигаем шарик
                _monoBehaviourParameters.BallTransform.Translate(movementVector);
                return false;
            }
            else
            {
                //если они всё же были, то просчитываем отражение от препятствия
                SentCollisionMessage(hitCollider.gameObject);

                //вычисляем в каком месте должен находится шарик, чтобы произошло такое столкновение, но чтобы не провалиться в коллайдер
                var collisionPointNormal = circleCastHit.normal;
                var collisionBallPosition = circleCastHit.point + collisionPointNormal * ballRadius;
                var reflectionVector = Vector2.Reflect(raycastDirection, collisionPointNormal);

                //если ударились о площадку, то делаем поправку на сдвиг, который даёт площадка
                if (hitCollider.CompareTag(_monoBehaviourParameters.PlayerControllerTag))
                    reflectionVector = AddPadReflectionCorrection(circleCastHit, hitCollider, reflectionVector);

                //перемещаем шарик к месту столкновения
                _monoBehaviourParameters.BallTransform.position = collisionBallPosition;
                _monoBehaviourParameters.NormalizedMovementVector = reflectionVector.normalized;
                //расчитываем параметры отскока
                var collisionDistance = Vector3.Distance(collisionBallPosition, startPosition);
                var reflectionDistance = movementDistance - collisionDistance;
                if (reflectionDistance > MINIMUM_MOVEMENT_STEP)
                    MoveWithCollisionCorrection(reflectionDistance);
                return true;
            }
        }

        /// <summary> Добавить смещение при ударе по площадку, чем дальше от центра тем сильнее смещение </summary>
        private Vector2 AddPadReflectionCorrection(RaycastHit2D rayHit, Collider2D playerPadCollider, Vector2 reflectionVector)
        {
            var userTransform = playerPadCollider.transform;
            var localHitPosition = userTransform.InverseTransformPoint(rayHit.point);
            var localHitPositionProjection = Vector3.Project(localHitPosition, Vector3.right);
            var offsetDirection = localHitPositionProjection.x < 0 ? Vector2.left : Vector2.right;

            var hitDistance = localHitPositionProjection.magnitude;
            float offsetPower = 0f;
            if (hitDistance > _monoBehaviourParameters.OnPlayerHitDeadZoneInCenter)
            {
                //расчёты этих величин не слишком точные, но учитывая размеры бъекта и назначение этих величин, я посчитал, что такая точность меня устраивает
                var colliderBounds = playerPadCollider.bounds;
                var extendSize = colliderBounds.extents.magnitude;
                offsetPower = Mathf.Clamp(Mathf.Abs(hitDistance / extendSize), 0, 1);
            }

            var offset = _monoBehaviourParameters.OnPlayerHitMaxOffset * offsetPower * offsetDirection;
            var reflectionWithOffset = reflectionVector + offset;
            return reflectionWithOffset;
        }

        private bool CheckCollisionsWithoutMovement()
        {
            var collisions = CheckCollision();
            foreach (var collidedObject in collisions)
                SentCollisionMessage(collidedObject);
            return collisions.Any();
        }

        private void SentCollisionMessage(GameObject collidedObject)
        {
            /*
             * Да, использовать GetComponent не в Awake плохо, но другие способы проверить коллизию с объектом 
             * а потом сообщить ему, что произошла коллизия с шариком без RigidBody (SendMessage, пообъектная проверка и т.д.)
             * ещё дороже, пусть раз шарик летает, то он и сообщает всем о том, что произошла коллизия
             */
            var ballCollisionDetector = collidedObject.GetComponent<OnBallCollisionMono>();
            if (ballCollisionDetector == null) return;
            ballCollisionDetector.OnBallCollision.Invoke(this);
        }
    }
}