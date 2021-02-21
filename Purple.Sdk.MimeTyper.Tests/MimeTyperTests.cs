using Purple.Sdk.MimeTyper.Models;
using System.Threading.Tasks;
using Xunit;

namespace Purple.Sdk.MimeTyper.Tests
{
    public class MimeTyperTests
    {
        [Fact]
        public async Task GetExtensionFromMimeType()
        {
            var downloader = new MimeTyper();
            var ext = await downloader.GetExtensionFromMimeType("image/jpeg");
            Assert.Equal("jpeg", ext);
        }

        [Fact]
        public async Task GetMimeTypeFromExtension()
        {
            var downloader = new MimeTyper();
            var ext = await downloader.GetMimeTypeFromExtension("logo-finaleaaa.svg");
            Assert.Equal("image/svg+xml", ext);
        }
        [Fact]
        public async Task GetMimeTypeFromCommonExtension()
        {
            var downloader = new MimeTyper();
            var ext = await downloader.GetMimeTypeFromExtension("logo-finaleaaa.docx");
            Assert.Equal("application/vnd.openxmlformats-officedocument.wordprocessingml.document", ext);
        }
        [Fact]
        public async Task GetMimeTypeFromCustomExtension()
        {
            var downloader = new MimeTyper();
            await downloader.AddCustomMimeType(new MimeTyperItem { Extension = "purple", MimeType = "application/purplesoft-io" });
            var ext = await downloader.GetMimeTypeFromExtension("logo-finaleaaa.purple");
            Assert.Equal("application/purplesoft-io", ext);
        }
        [Fact]
        public async Task CustomExtensionFromMimeType()
        {
            var downloader = new MimeTyper();
            await downloader.AddCustomMimeType(new MimeTyperItem { Extension = "purple", MimeType = "application/purplesoft-io" });
            var ext = await downloader.GetExtensionFromMimeType("application/purplesoft-io");
            Assert.Equal("purple", ext);
        }

        [Fact]
        public async Task FileStorageGetExtensionFromMimeType()
        {
            var downloader = new MimeTyper(true, MimeTyperLoadMode.File);
            var ext = await downloader.GetExtensionFromMimeType("image/jpeg");
            Assert.Equal("jpeg", ext);
        }

        [Fact]
        public async Task FileStorageGetMimeTypeFromExtension()
        {
            var downloader = new MimeTyper(true, MimeTyperLoadMode.File);
            var ext = await downloader.GetMimeTypeFromExtension("logo-finaleaaa.svg");
            Assert.Equal("image/svg+xml", ext);
        }
        [Fact]
        public async Task FileStorageGetMimeTypeFromCommonExtension()
        {
            var downloader = new MimeTyper(true, MimeTyperLoadMode.File);
            var ext = await downloader.GetMimeTypeFromExtension("logo-finaleaaa.docx");
            Assert.Equal("application/vnd.openxmlformats-officedocument.wordprocessingml.document", ext);
        }
        [Fact]
        public async Task FileStorageGetMimeTypeFromCustomExtension()
        {
            var downloader = new MimeTyper(true, MimeTyperLoadMode.File);
            await downloader.AddCustomMimeType(new MimeTyperItem { Extension = "purple", MimeType = "application/purplesoft-io" });
            var ext = await downloader.GetMimeTypeFromExtension("logo-finaleaaa.purple");
            Assert.Equal("application/purplesoft-io", ext);
        }
        [Fact]
        public async Task FileStorageGetCustomExtensionFromMimeType()
        {
            var downloader = new MimeTyper(true, MimeTyperLoadMode.File);
            await downloader.AddCustomMimeType(new MimeTyperItem { Extension = "purple", MimeType = "application/purplesoft-io" });
            var ext = await downloader.GetExtensionFromMimeType("application/purplesoft-io");
            Assert.Equal("purple", ext);
        }
    }
}
