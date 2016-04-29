namespace SpotifyMuter
{
    static class UrlBuilder
    {
        private const string Port = ":4380";

        public static string GetUrl(string path)
        {
            return "http://127.0.0.1" + Port + path;
            //return "http://" + hostname + ".spotilocal.com" + port + path;
        }
    }
}