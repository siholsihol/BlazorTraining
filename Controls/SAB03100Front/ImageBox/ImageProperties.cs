namespace SAB03100Front.ImageBox
{
    public class ImageProperties
    {
        public ImageProperties()
        {
        }

        public ImageProperties(string fullUrl, string name)
        {
            FullUrl = fullUrl;
            Name = name;
        }

        public ImageProperties(byte[] imageByte)
        {
            ImageByte = imageByte;
        }

        public string FullUrl { get; set; }
        public string Name { get; set; }
        public byte[] ImageByte { get; set; }
    }
}
