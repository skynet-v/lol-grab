using LOLApiAdapter.CommonDefinitions.Interfaces;

namespace LOLApiAdapter.ApiResponseStructures
{
    public class ChampionsResponse : ILoLResponse
    {
        public ChampionDescription[] champions { get; set; }
    }

    public class ChampionDescription : ILoLResponse
    {
        public bool rankedPlayEnabled { get; set; }
        public bool botEnabled { get; set; }
        public bool botMmEnabled { get; set; }
        public bool active { get; set; }
        public bool freeToPlay { get; set; }
        public int id { get; set; }
    }
}
