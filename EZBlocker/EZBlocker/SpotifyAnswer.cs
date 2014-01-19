using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZBlocker
{
    public class Info
    {
        public int num_results;
        public int limit;
        public int offset;
        public string query;
        public string type;
        public int page;
    }

    public class Artist
    {
        public string href;
        public string name;
        public float popularity;
    }

    public class SpotifyAnswer
    {
        public Info info;
        public IList<Artist> artists;
    }
}
