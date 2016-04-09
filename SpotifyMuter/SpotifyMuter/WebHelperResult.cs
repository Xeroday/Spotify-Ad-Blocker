namespace SpotifyMuter
{
    class WebHelperResult
    {
        public bool isRunning = true;
        public bool isPlaying = false;
        public bool isAd = false;
        public bool isPrivateSession = false;

        public float position = 0;
        public int length = 1234;

        public string artistName = "N/A";
    }
}
