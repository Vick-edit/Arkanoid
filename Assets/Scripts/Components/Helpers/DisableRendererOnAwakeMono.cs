using UnityEngine;

namespace Components.Helpers
{
    /// <summary>
    ///     Компонент отключающий отображение объектов при старте
    /// </summary>
    public class DisableRendererOnAwakeMono : MonoBehaviour
    {
        private void Awake()
        {
            var allRenderers = GetComponents<Renderer>();

            foreach (var renderer in allRenderers)
            {
                renderer.enabled = false;
            }
        }
    }
}