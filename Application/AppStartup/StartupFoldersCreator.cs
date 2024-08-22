namespace Application.AppStartup
{
    public static class StartupFoldersCreator
    {
        public static void Generate()
        {
            if (!Directory.Exists(Settings.RootFilesPath)) Directory.CreateDirectory(Settings.RootFilesPath);

            if (!Directory.Exists(Settings.ImagesOriginFullPath)) Directory.CreateDirectory(Settings.ImagesOriginFullPath);

            if (!Directory.Exists(Settings.ImagesBigFullPath)) Directory.CreateDirectory(Settings.ImagesBigFullPath);

            if (!Directory.Exists(Settings.ImagesSmallFullPath)) Directory.CreateDirectory(Settings.ImagesSmallFullPath);
        }
    }
}
