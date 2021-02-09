namespace Purple.Sdk.MimeTyper.Models
{
    public class MimeTyperItem
    {
        public override string ToString()
        {
            return $"{Extension} - {MimeType}";
        }

        public string Extension { get; set; }
        public string MimeType { get; set; }
    }
}