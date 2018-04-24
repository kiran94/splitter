namespace Splitter.Framework
{
    using System.IO;

    /// <inheritdoc />
    public class FileIoService : IFileIoService
    {
        /// <inheritdoc />
        public Stream Open(string filePath, FileMode mode, FileAccess access, FileShare share)
        {
            return File.Open(filePath, mode, access, share);
        }

        /// <inheritdoc />
        public void Delete(string filePath)
        {
            File.Delete(filePath);
        }

         /// <inheritdoc />
        public void Move(string sourcePath, string targetPath)
        {
            File.Move(sourcePath, targetPath);
        }

        /// <inheritdoc />
        public Stream Open(string path, FileMode mode)
        {
            return new FileStream(path, mode);
        }

        /// <inheritdoc />
        public string GetDirectory(string path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <inheritdoc />
        public string GetFileWithoutExt(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <inheritdoc />
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        /// <inheritdoc />
        public void AddMeta(string path, string title, string album, string author, int trackNo, int trackCount)
        {
            var file = TagLib.File.Create(path);
            file.Tag.Title = title;
            file.Tag.Album = album;
            file.Tag.Performers = new[] { author };
            file.Tag.Track = (uint) trackNo;
            file.Tag.TrackCount = (uint) trackCount;

            // var image = new Picture(imagepath);
            // file.Tag.Pictures = new TagLib.IPicture[1] { image };

            file.Save();
        }
    }
}