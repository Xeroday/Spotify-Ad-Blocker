namespace SpotifyMuter.Json
{
    class SpotifyStatus
    {
        public Error error;
        public bool running;
        public bool playing;
        public bool next_enabled;
        public OpenGraphState open_graph_state;
        public float playing_position;
        public Track track;
    }
}