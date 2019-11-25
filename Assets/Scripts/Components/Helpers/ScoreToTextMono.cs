using System.Globalization;
using TMPro;
using UnityEngine;

namespace Components.Helpers
{
    /// <summary>
    ///     Компонент, который занимается форматированием числа очков в UI текст
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreToTextMono : MonoBehaviour
    {
        [SerializeField] private string _numberFormating;
        [SerializeField] private TextMeshProUGUI _scoreText;
        private readonly NumberFormatInfo _numberFormat = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();

        public void SetScore(int score)
        {
            _scoreText.text = score.ToString(_numberFormating, _numberFormat);
        }
    }
}