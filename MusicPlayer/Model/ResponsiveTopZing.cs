namespace MusicPlayer
{
    public class ResponsiveTopZing
    {
        public class Rootobject
        {
            public int err { get; set; }
            public string msg { get; set; }
            public Data data { get; set; }
            public long timestamp { get; set; }
        }

        public class Data
        {
            public Song[] song { get; set; }
            public Customied[] customied { get; set; }
            public int peak_score { get; set; }
            public Songhis songHis { get; set; }
        }

        public class Songhis
        {
            public int min_score { get; set; }
            public float max_score { get; set; }
            public long from { get; set; }
            public int interval { get; set; }
            public int total_score { get; set; }
        }

       


        public class Song
        {
            public string id { get; set; }
            public string name { get; set; }
            public string title { get; set; }
            public string code { get; set; }
            public int content_owner { get; set; }
            public bool isoffical { get; set; }
            public bool isWorldWide { get; set; }
            public string playlist_id { get; set; }
            public Artist2[] artists { get; set; }
            public string artists_names { get; set; }
            public string performer { get; set; }
            public string type { get; set; }
            public string link { get; set; }
            public string lyric { get; set; }
            public string thumbnail { get; set; }
            public int duration { get; set; }
            public int total { get; set; }
            public object rank_num { get; set; }
            public string rank_status { get; set; }
            public Artist artist { get; set; }
            public int position { get; set; }
            public object order { get; set; }
            public Album album { get; set; }
            public string mv_link { get; set; }
        }

        public class Artist
        {
            public string id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public string cover { get; set; }
            public string thumbnail { get; set; }
        }

        public class Album
        {
            public string id { get; set; }
            public string link { get; set; }
            public string title { get; set; }
            public string name { get; set; }
            public bool isoffical { get; set; }
            public string artists_names { get; set; }
            public Artist1[] artists { get; set; }
            public string thumbnail { get; set; }
            public string thumbnail_medium { get; set; }
        }

        public class Artist1
        {
            public string name { get; set; }
            public string link { get; set; }
        }

        public class Artist2
        {
            public string name { get; set; }
            public string link { get; set; }
        }

        public class Customied
        {
            public string id { get; set; }
            public string name { get; set; }
            public string title { get; set; }
            public string code { get; set; }
            public int content_owner { get; set; }
            public bool isoffical { get; set; }
            public bool isWorldWide { get; set; }
            public string playlist_id { get; set; }
            public Artist3[] artists { get; set; }
            public string artists_names { get; set; }
            public string performer { get; set; }
            public string type { get; set; }
            public string link { get; set; }
            public string lyric { get; set; }
            public string thumbnail { get; set; }
            public int duration { get; set; }
        }

        public class Artist3
        {
            public string name { get; set; }
            public string link { get; set; }
        }
    }
}
