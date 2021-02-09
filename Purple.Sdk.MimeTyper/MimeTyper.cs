using Purple.Sdk.MimeTyper.Exceptions;
using Purple.Sdk.MimeTyper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Purple.Sdk.MimeTyper
{
    public class MimeTyper
    {
        private readonly string _mimeTypesUrl;
        private MimeTyperItem[] _mimeCache;

        public MimeTyper(string mimeTypesUrl = "https://raw.githubusercontent.com/apache/httpd/trunk/docs/conf/mime.types")
        {
            if (string.IsNullOrWhiteSpace(mimeTypesUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(mimeTypesUrl));
            this._mimeTypesUrl = mimeTypesUrl;
        }

        public async Task<MimeTyperItem[]> Fetch()
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

            if (_mimeCache == null || _mimeCache.Length == 0)
            {
                _mimeCache = (await Fetch()).OrderByDescending(o => o.Extension.Length).ToArray();
            }

            return _mimeCache.FirstOrDefault(f => fileNameOrExtension.EndsWith(f.Extension, StringComparison.InvariantCultureIgnoreCase))?.MimeType ?? "application/octet-stream";

        }
        public async Task<string> GetExtensionFromMimeType(string mimeType)
        {
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(mimeType));

            if (_mimeCache == null || _mimeCache.Length == 0)
            {
                _mimeCache = (await Fetch()).OrderByDescending(o => o.Extension.Length).ToArray();
            }

            mimeType = mimeType.ToLower();
            return _mimeCache.FirstOrDefault(f => f.MimeType == mimeType)?.Extension ?? "bin";

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
}
