using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOLApiAdapter.CommonDefinitions;
using LOLApiAdapter.CommonDefinitions.Interfaces;

namespace LOLApiAdapter.ApiResponseStructures
{
    public class SummonerResponse : ILoLResponse
    {
        public long     id;
        public string   name;
        public int      profileIconId;
        public long     revisionDate;
        public long     summonerLevel;
    }
}
