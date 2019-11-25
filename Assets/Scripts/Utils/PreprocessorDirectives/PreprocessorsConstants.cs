using System;

namespace Utils.PreprocessorDirectives
{
    /// <summary>
    ///     Перечисление платформ, под которые может собираться игра
    /// </summary>
    public enum BuildTargetPlatform
    {
        UnityStandaloneOsx,
        UnityStandaloneWin,
        UnityStandaloneLinux,

        UnityIos,
        UnityAndroid,
        UnityWsa,
    }
}