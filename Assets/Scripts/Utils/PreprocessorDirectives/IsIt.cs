namespace Utils.PreprocessorDirectives
{
    /// <summary>
    ///     Набор различных платформозависимых флагов
    /// </summary>
    public static class IsIt
    {
        /// <summary> Мы сейчас в редакторе Unity? </summary>
        public static bool Editor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary> Мы сейчас дебаге? </summary>
        public static bool Mobile
        {
            get
            {
               return BuildInfo.BuildTargetInfo == BuildTargetPlatform.UnityAndroid || 
                      BuildInfo.BuildTargetInfo == BuildTargetPlatform.UnityIos;
            }
        }
    }
}