using Application;
using Domain.BackendResponses;
using Domain.CoreEnums;
using Domain.Interfaces.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        public ImageService() { }

        public async Task<MethodResult<string>> SaveImageAsync(Enums.ImagePurpose imagePurpose, Stream fileStream)
        {
            string filePublicName = string.Empty;
            switch (imagePurpose)
            {
                case Enums.ImagePurpose.OriginImage:
                    filePublicName = await ResizeImageAsync(Settings.ImagesOriginPublicPath, fileStream, Settings.BIG_IMAGES_WIDTH_OR_HEIGHT, false);
                    break;

                case Enums.ImagePurpose.BigImage:
                    filePublicName = await ResizeImageAsync(Settings.ImagesBigPublicPath, fileStream,Settings.BIG_IMAGES_WIDTH_OR_HEIGHT, true);
                    break;

                case Enums.ImagePurpose.SmallImage:
                    filePublicName = await ResizeImageAsync(Settings.ImagesSmallPublicPath, fileStream, Settings.SMALL_IMAGES_WIDTH_OR_HEIGHT, true);
                    break;
            }

            return new MethodResult<string>(filePublicName, [], Enums.MethodResults.Ok);
        }

        public async Task<MethodResult> DeleteImageAsync(string imagePublicName)
        {
            if (!File.Exists($"{Settings.RootFolderForFiles}{imagePublicName}"))
            {
                return new MethodResult([], Enums.MethodResults.Ok);
            }

            bool isSucceeded = false;

            await Task.Run(() => 
            {
                for (int attempts = 0;  attempts < 3; attempts++) 
                {
                    try
                    {
                        File.Delete($"{Settings.RootFolderForFiles}{imagePublicName}");
                        isSucceeded = true;
                    }
                    catch { Thread.Sleep(1000); }
                }
            });

            if (isSucceeded)
            {
                return new MethodResult([], Enums.MethodResults.Ok);
            }
            else
            {
                return new MethodResult([], Enums.MethodResults.Conflict);
            }
        }

        private async Task<string> ResizeImageAsync(string specificSizeImagePublicFolderPath, Stream fileStream,int sizeLimit, bool isNeededResize = true)
        {
            var specificImageDirPath = $"{Settings.RootFolderForFiles}{specificSizeImagePublicFolderPath}";

            int dirsCount = Directory.EnumerateDirectories(specificImageDirPath).Count();

            if (dirsCount == 0) { Directory.CreateDirectory($"{specificImageDirPath}/1"); dirsCount++; }

            var lastDirFullPath = new DirectoryInfo(specificImageDirPath).GetDirectories().OrderByDescending(d => d.CreationTimeUtc).First().FullName;
            
            if (Directory.EnumerateFiles(lastDirFullPath).Count() >= 1000)
            {
                lastDirFullPath = $"{specificImageDirPath}/{dirsCount + 1}";
                Directory.CreateDirectory(lastDirFullPath);
            }

            fileStream.Position = 0;

            using var sourceImage = await Image.LoadAsync(fileStream);

            string fileName = Guid.NewGuid().ToString() + ".webp";

            string fileFullName = $"{lastDirFullPath}/{fileName}";

            while (File.Exists(fileFullName))
            {
                fileName = Guid.NewGuid().ToString() + ".webp";
                fileFullName = $"{lastDirFullPath}/{fileName}";
            }

            Image cloned;
            if (isNeededResize)
            {
                cloned = GetClonedImage(sourceImage, sizeLimit);
            }
            else
            {
                cloned = sourceImage;
            }

            await cloned.SaveAsWebpAsync(fileFullName);

            cloned.Dispose();

            var filePublicName = fileFullName.Substring(Settings.RootFolderForFiles.Length); // need to remove system file path and leave only public part

            return filePublicName;
        }

        private Image GetClonedImage(Image source, int sizeLimit)
        {
            if (source.Height >= sizeLimit || source.Width >= sizeLimit)
            {
                float scale;
                if (source.Height >= source.Width)
                {
                    scale = (float)source.Height / (float)sizeLimit;
                }
                else
                {
                    scale = (float)source.Width / (float)sizeLimit;
                }
                var cloned = source.Clone(i => i.Resize((int)(source.Width / scale), (int)(source.Height / scale)));

                return cloned;
            }

            else
            {
                return source;
            }
        }
    }
}
