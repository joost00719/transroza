namespace TransRoza.FileHandling
{
    public static class FileSizeHelper
    {
        public static int BytesToMegaBytes(long bytes)
        {
            return (int)(bytes / 1024 / 1024);
        }
    }
}
