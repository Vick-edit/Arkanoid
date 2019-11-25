
using UnityEngine;

namespace Extensions
{
    /// <summary>
    ///     Набор расширяющих методов для типа данных <see cref="Vector2"/>
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary> Повернуть вектор на рандомный угол </summary>
        /// <param name="source">Исходный вектор</param>
        /// <param name="angelRange">Дипозон разброса рандомных значений</param>
        public static Vector2 RotateRandomByAngle(this Vector2 source, float angelRange)
        {
            var halfRange = Mathf.Abs(angelRange/2);
            var rotationValue = Random.Range(-halfRange, halfRange);

            var newDirection = Quaternion.AngleAxis(rotationValue, Vector3.forward) * source;
            return newDirection;
        }

        /// <summary> Повернуть вектор на заданный угол </summary>
        /// <param name="source">Исходный вектор</param>
        public static Vector2 RotateByAngle(this Vector2 source, float turnAngel)
        {
            var newDirection = Quaternion.AngleAxis(turnAngel, Vector3.forward) * source;
            return newDirection;
        }
    }
}