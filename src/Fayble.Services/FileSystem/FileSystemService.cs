using System.Security.Cryptography;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;

namespace Fayble.Services.FileSystem
{
    public abstract class FileSystemService : IFileSystemService
    {

        private readonly IFileTypeRepository _fileTypeRepository;

        protected FileSystemService(IFileTypeRepository fileTypeRepository)
        {
            _fileTypeRepository = fileTypeRepository;
        }

        public async Task<IEnumerable<string>> GetFiles(string directory, MediaType mediaType)
        {
            var extensions = (await _fileTypeRepository.Get(x => x.MediaType == mediaType))
                .Select(x => x.FileExtension).ToList();

            return Directory.EnumerateFiles(directory, "*.*").Where(
                f => extensions.Contains(Path.GetExtension(f).Replace(".", "").ToLowerInvariant()));
        }
        public string GetHash(string filePath)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
