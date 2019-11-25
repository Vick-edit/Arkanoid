
using UnityEngine;

namespace Extensions
{
    /// <summary>
    ///     Набор расширяющих методов для типа данных <see cref="Vector3"/>
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary> Повернуть вектор на рандомный угол </summary>
        /// <param name="source">Исходный вектор</param>
        /// <param name="normal">Нормаль к поверхности в которой должен быть повёрнут угол</param>
        /// <param name="angelRange">Дипозон разброса рандомных значений</param>
        public static Vector3 RotateRandomByAngle(this Vector3 source, Vector3 normal, float angelRange)
        {
            var halfRange = Mathf.Abs(angelRange/2);
            var rotationValue = Random.Range(-halfRange, halfRange);

            var newDirection = Quaternion.AngleAxis(rotationValue, normal) * source;
            return newDirection;
        }

        /// <summary> Повернуть вектор на рандомный угол </summary>
        /// <param name="source">Исходный вектор</param>
        /// <param name="angelRange">Дипозон разброса рандомных значений</param>
        public static Vector3 RotateRandomByAngle(this Vector3 source, float angelRange)
        {
            var randomAngel = Random.Range(0, 360);
            var randomNormal = Quaternion.AngleAxis(randomAngel, source) * Vector3.one;

            return source.RotateRandomByAngle(randomNormal, angelRange);
        }
    }
}