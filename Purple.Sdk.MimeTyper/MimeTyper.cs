using Newtonsoft.Json;
using Purple.Sdk.MimeTyper.Exceptions;
using Purple.Sdk.MimeTyper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Purple.Sdk.MimeTyper
{
    public class MimeTyper
    {
        private readonly string _mimeTypesUrl;
        private readonly bool _includeMozillaCommonMimeTypes;
        private readonly MimeTyperLoadMode _mode;
        private MimeTyperItem[] _mimeCache;
        private readonly List<MimeTyperItem> _customMimeCache = new List<MimeTyperItem>();


        public MimeTyper(bool includeMozillaCommonMimeTypes = true, MimeTyperLoadMode mode = MimeTyperLoadMode.ApacheGithub, string mimeTypesUrl = "https://raw.githubusercontent.com/apache/httpd/trunk/docs/conf/mime.types")
        {
            if (!Enum.IsDefined(typeof(MimeTyperLoadMode), mode))
                throw new InvalidEnumArgumentException(nameof(mode), (int)mode, typeof(MimeTyperLoadMode));
            this._mimeTypesUrl = string.IsNullOrWhiteSpace(mimeTypesUrl) ? "https://raw.githubusercontent.com/apache/httpd/trunk/docs/conf/mime.types" : mimeTypesUrl;
            _includeMozillaCommonMimeTypes = includeMozillaCommonMimeTypes;
            _mode = mode;
        }

        public async Task AddCustomMimeType(MimeTyperItem customMimeType)
        {
            if (customMimeType == null) throw new ArgumentNullException(nameof(customMimeType));
            if (string.IsNullOrWhiteSpace(customMimeType.Extension)) throw new ArgumentNullException(nameof(customMimeType.Extension));
            if (string.IsNullOrWhiteSpace(customMimeType.MimeType)) throw new ArgumentNullException(nameof(customMimeType.MimeType));
            await RefreshCache();
            if (_customMimeCache.All(a => customMimeType.Extension != a.Extension) &&
                _mimeCache.All(a => customMimeType.Extension != a.Extension))
            {
                _customMimeCache.Add(customMimeType);
            }
        }

        public async Task ClearCustomMimeTypes(bool skipCommonMimeTypes = true)
        {
            _customMimeCache.Clear();
            if (skipCommonMimeTypes)
            {
                await AddCommonMimeTypes();
            }
        }
        private async Task AddCommonMimeTypes()
        {
            await AddCustomMimeType(new MimeTyperItem { Extension = "aac", MimeType = "audio/aac" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "abw", MimeType = "application/x-abiword" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "arc", MimeType = "application/x-freearc" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "avi", MimeType = "video/x-msvideo" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "azw", MimeType = "application/vnd.amazon.ebook" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "bin", MimeType = "application/octet-stream" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "bmp", MimeType = "image/bmp" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "bz", MimeType = "application/x-bzip" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "bz2", MimeType = "application/x-bzip2" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "csh", MimeType = "application/x-csh" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "css", MimeType = "text/css" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "csv", MimeType = "text/csv" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "doc", MimeType = "application/msword" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "docx", MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "eot", MimeType = "application/vnd.ms-fontobject" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "epub", MimeType = "application/epub+zip" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "gz", MimeType = "application/gzip" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "gif", MimeType = "image/gif" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "html", MimeType = "text/html" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "htm", MimeType = "text/html" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "ico", MimeType = "image/vnd.microsoft.icon" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "ics", MimeType = "text/calendar" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "jar", MimeType = "application/java-archive" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "jpg", MimeType = "image/jpeg" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "jpeg", MimeType = "image/jpeg" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "js", MimeType = "[List]" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "json", MimeType = "application/json" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "jsonld", MimeType = "application/ld+json" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "mid", MimeType = "audio/midi audio/x-midi" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "midi", MimeType = "audio/midi audio/x-midi" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "mjs", MimeType = "text/javascript" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "mp3", MimeType = "audio/mpeg" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "mpeg", MimeType = "video/mpeg" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "mpkg", MimeType = "application/vnd.apple.installer+xml" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "odp", MimeType = "application/vnd.oasis.opendocument.presentation" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "ods", MimeType = "application/vnd.oasis.opendocument.spreadsheet" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "odt", MimeType = "application/vnd.oasis.opendocument.text" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "oga", MimeType = "audio/ogg" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "ogv", MimeType = "video/ogg" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "ogx", MimeType = "application/ogg" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "opus", MimeType = "audio/opus" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "otf", MimeType = "font/otf" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "png", MimeType = "image/png" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "pdf", MimeType = "application/pdf" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "php", MimeType = "application/x-httpd-php" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "ppt", MimeType = "application/vnd.ms-powerpoint" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "pptx", MimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "rar", MimeType = "application/vnd.rar" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "rtf", MimeType = "application/rtf" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "sh", MimeType = "application/x-sh" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "svg", MimeType = "image/svg+xml" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "swf", MimeType = "application/x-shockwave-flash" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "tar", MimeType = "application/x-tar" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "tif", MimeType = "image/tiff" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "tiff", MimeType = "image/tiff" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "ts", MimeType = "video/mp2t" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "ttf", MimeType = "font/ttf" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "txt", MimeType = "text/plain" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "vsd", MimeType = "application/vnd.visio" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "wav", MimeType = "audio/wav" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "weba", MimeType = "audio/webm" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "webm", MimeType = "video/webm" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "webp", MimeType = "image/webp" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "woff", MimeType = "font/woff" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "woff2", MimeType = "font/woff2" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "xhtml", MimeType = "application/xhtml+xml" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "xls", MimeType = "application/vnd.ms-excel" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "xlsx", MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "xml", MimeType = "application/xml" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "xul", MimeType = "application/vnd.mozilla.xul+xml" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "zip", MimeType = "application/zip" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "3gp", MimeType = "video/3gpp" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "3g2", MimeType = "video/3gpp2" });
            await AddCustomMimeType(new MimeTyperItem { Extension = "7z", MimeType = "application/x-7z-compressed" });

        }
        private async Task<MimeTyperItem[]> Fetch()
        {
            try
            {
                MimeTyperItem[] lines;
                using (var cl = new HttpClient())
                {
                    var mimetypes = await cl.GetStringAsync(_mimeTypesUrl);
                    lines = mimetypes.Split('\r', '\n').Where(w => !string.IsNullOrWhiteSpace(w) && !w.StartsWith("#")).SelectMany(CastAsMimeTyperItem).ToArray();
                }

                return lines;
            }
            catch (Exception e)
            {
                throw new MimeTyperFetchException($"Error fetching data from provided uri: {_mimeTypesUrl}", e);
            }

        }

        public async Task<string> GetMimeTypeFromExtension(string fileNameOrExtension)
        {
            if (string.IsNullOrWhiteSpace(fileNameOrExtension))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(fileNameOrExtension));

            await RefreshCache();

            return _customMimeCache
                       .OrderByDescending(o => o.Extension.Length)
                       .FirstOrDefault(f => fileNameOrExtension.EndsWith(f.Extension, StringComparison.InvariantCultureIgnoreCase))
                       ?.MimeType ??
                   _mimeCache
                       .FirstOrDefault(f => fileNameOrExtension.EndsWith(f.Extension, StringComparison.InvariantCultureIgnoreCase))
                       ?.MimeType ??
                   "application/octet-stream";

        }

        private async Task RefreshCache(bool force = false)
        {

            switch (_mode)
            {
                case MimeTyperLoadMode.ApacheGithub:
                    if (_mimeCache == null || _mimeCache.Length == 0 || force)
                    {
                        _mimeCache = (await Fetch()).OrderByDescending(o => o.Extension.Length).ToArray();
                        _customMimeCache.Clear();
                        if (_includeMozillaCommonMimeTypes)
                        {
                            await AddCommonMimeTypes();
                        }
                    }
                    break;
                case MimeTyperLoadMode.File:
                    if (_mimeCache == null || _mimeCache.Length == 0 || force)
                    {
                        var cacheFile =
                            Path.Combine(Path.GetDirectoryName(typeof(MimeTyper).Assembly.Location) ?? string.Empty,
                                "mimetyper.cache.json");
                        if (!File.Exists(cacheFile))
                        {
                            _mimeCache = (await Fetch()).OrderByDescending(o => o.Extension.Length).ToArray();
                            File.WriteAllText(cacheFile, JsonConvert.SerializeObject(_mimeCache));
                        }
                        else
                        {
                            _mimeCache = JsonConvert.DeserializeObject<MimeTyperItem[]>(File.ReadAllText(cacheFile));
                            if (_mimeCache == null || _mimeCache.Length == 0 || force)
                            {
                                _mimeCache = (await Fetch()).OrderByDescending(o => o.Extension.Length).ToArray();
                                File.WriteAllText(cacheFile, JsonConvert.SerializeObject(_mimeCache));
                            }
                        }
                        _customMimeCache.Clear();
                        if (_includeMozillaCommonMimeTypes)
                        {
                            await AddCommonMimeTypes();
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }



        }

        public async Task<string> GetExtensionFromMimeType(string mimeType)
        {
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(mimeType));

            await RefreshCache();

            mimeType = mimeType.ToLower();
            return _customMimeCache.OrderByDescending(o => o.Extension.Length).FirstOrDefault(f => f.MimeType == mimeType)?.Extension ??
                   _mimeCache.FirstOrDefault(f => f.MimeType == mimeType)?.Extension ??
                   "bin";

        }

        private static IEnumerable<MimeTyperItem> CastAsMimeTyperItem(string s)
        {
            const string doubleTab = "\t\t";

            while (s.Contains(doubleTab))
            {
                s = s.Replace(doubleTab, "\t");
            }

            var spl = s.Split('\t');
            var mime = spl.First();
            var extensions = spl.Last();

            return extensions.Split(' ').Select(s1 => new MimeTyperItem
            {
                MimeType = mime.Trim().ToLower(),
                Extension = s1.Trim().ToLower()
            });
        }
    }

    public enum MimeTyperLoadMode
    {
        ApacheGithub,
        File
    }

}
