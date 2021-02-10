# Purple.Sdk.MimeTyper
.NET MimeType Helper with realtime update from apache docs main git trunk

```csharp
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
            var ext = await downloader.GetMimeTypeFromExtension("jpeg");
            Assert.Equal("image/jpeg", ext);
        }
    }
}
```
