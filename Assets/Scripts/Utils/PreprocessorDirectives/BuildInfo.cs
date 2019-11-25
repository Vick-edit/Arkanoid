namespace Utils.PreprocessorDirectives
{
    /// <summary>
    ///     Класс, определяющий параметры сборки через препроцесорные директивы, чтобы не определять их в классе,
    ///     что даёт нам возможность переименовывать нормально объекты и прочее, потому что решарпер не забирается под
    ///     отключенный препроцессорными директивами код
    /// </summary>
    public static class BuildInfo
    {
        /// <summary> Получить текущую выбранную платформу для билда </summary>
        public static BuildTargetPlatform BuildTargetInfo
        {
            get
            {
#if UNITY_STANDALONE_WIN
                return BuildTargetPlatform.UnityStandaloneWin;
#elif UNITY_STANDALONE_OSX
                return BuildTargetPlatform.UnityStandaloneOsx;
#elif UNITY_STANDALONE_LINUX
                return BuildTargetPlatform.UnityStandaloneLinux;
#elif UNITY_IOS
                return BuildTargetPlatform.UnityIos;
#elif UNITY_ANDROID
                return BuildTargetPlatform.UnityAndroid;
#elif UNITY_WSA
                return BuildTargetPlatform.UnityWsa;
#else
                throw new InvalidDataException("Сборка под эту платформу не поддерживается");
#endif
            }
        }

        /// <summary> Получить название текущей выбранной платформы </summary>
        public static string GetBuildTargetName()
        {
            return BuildTargetInfo.ToString().Replace("Unity", "");
        }
    }
}