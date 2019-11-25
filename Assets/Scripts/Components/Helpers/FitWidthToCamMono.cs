using System;
using UnityEngine;

namespace Components.Helpers
{
    /// <summary>
    ///     Компонент изменяющий скейл камеры таким образом, чтобы всё игровое поле входило по ширине
    ///     для нас это важно, т.к. в игре участвует всё игровое поле, которое должно быть видно
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class FitWidthToCamMono : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _gameFieldWidth;
        private float _originalOrthographicSize;
        private float _lastCheckedResolution = float.NaN;

        private const float TOLERANCE = 0.01f;


        private void Start()
        {
            if(_mainCamera == null)
                _mainCamera = GetComponent<Camera>();

            if (_mainCamera == null)
                throw new Exception("Не удалось найти камеру");

            _originalOrthographicSize = _mainCamera.orthographicSize;
        }
        
        private void Update()
        {
            var currentResolution = 1f * _mainCamera.pixelHeight / _mainCamera.pixelWidth;
            if (float.IsNaN(_lastCheckedResolution) || Math.Abs(_lastCheckedResolution - currentResolution) > TOLERANCE)
            {
                var orthographicSizeByWidth = 0.5f * currentResolution * _gameFieldWidth;
                var desiredSize = Mathf.Max(_originalOrthographicSize, orthographicSizeByWidth);
                _mainCamera.orthographicSize = desiredSize;
            }
            _lastCheckedResolution = currentResolution;
        }

        
    }
}