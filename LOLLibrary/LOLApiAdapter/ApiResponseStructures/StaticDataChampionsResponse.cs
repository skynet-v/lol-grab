using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOLApiAdapter.CommonDefinitions.Interfaces;

namespace LOLApiAdapter.ApiResponseStructures
{
   public class StaticDataChampionsResponse : ILoLResponse
    {
        public Keys     keys    { get; set; }
        public string   type    { get; set; }
        public string   version { get; set; }
        public Data     data    { get; set; }
        public string   format  { get; set; }
    }

    public class Keys : Dictionary<int, string>
    {
    }

    public class Data : Dictionary<string, ChampionData>
    {
        
    }

    public class ChampionData
    {
        public Info             info            { get; set; }
        public string[]         enemytips       { get; set; }
        public ChampionStats    stats           { get; set; }
        public string           name            { get; set; }
        public string           title           { get; set; }
        public Image            image           { get; set; }
        public string[]         tags            { get; set; }
        public string           partype         { get; set; }
        public Skin[]           skins           { get; set; }
        public Passive          passive         { get; set; }
        public Recommended[]    recommended     { get; set; }
        public string[]         allytips        { get; set; }
        public string           key             { get; set; }
        public string           lore            { get; set; }
        public int              id              { get; set; }
        public string           blurb           { get; set; }
        public Spell[]          spells          { get; set; }
    }

    public class Info
    {
        public int difficulty { get; set; }
        public int attack { get; set; }
        public int defense { get; set; }
        public int magic { get; set; }
    }

    public class ChampionStats
    {
        public float armorperlevel { get; set; }
        public float attackdamage { get; set; }
        public float mpperlevel { get; set; }
        public float attackspeedoffset { get; set; }
        public float mp { get; set; }
        public float armor { get; set; }
        public float hp { get; set; }
        public float hpregenperlevel { get; set; }
        public float attackspeedperlevel { get; set; }
        public float attackrange { get; set; }
        public float movespeed { get; set; }
        public float attackdamageperlevel { get; set; }
        public float mpregenperlevel { get; set; }
        public float critperlevel { get; set; }
        public float spellblockperlevel { get; set; }
        public float crit { get; set; }
        public float mpregen { get; set; }
        public float spellblock { get; set; }
        public float hpregen { get; set; }
        public float hpperlevel { get; set; }
    }

    public class Image
    {
        public string full { get; set; }
        public string group { get; set; }
        public string sprite { get; set; }
        public int h { get; set; }
        public int w { get; set; }
        public int y { get; set; }
        public int x { get; set; }
    }

    public class Passive
    {
        public Image image { get; set; }
        public string sanitizedDescription { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

 
    public class Skin
    {
        public int num { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Recommended
    {
        public string map { get; set; }
        public Block[] blocks { get; set; }
        public string champion { get; set; }
        public string title { get; set; }
        public string mode { get; set; }
        public string type { get; set; }
    }

    public class Block
    {
        public Item[] items { get; set; }
        public bool recMath { get; set; }
        public string type { get; set; }
    }

    public class Item
    {
        public int count { get; set; }
        public int id { get; set; }
    }

    public class Spell
    {
        public string cooldownBurn { get; set; }
        public string key { get; set; }
        public string resource { get; set; }
        public Leveltip leveltip { get; set; }
        public Var[] vars { get; set; }
        public string costType { get; set; }
        public string description { get; set; }
        public string sanitizedDescription { get; set; }
        public string sanitizedTooltip { get; set; }
        public float[][] effect { get; set; }
        public string tooltip { get; set; }
        public int maxrank { get; set; }
        public string costBurn { get; set; }
        public string rangeBurn { get; set; }
        public float[] range { get; set; }
        public float[] cost { get; set; }
        public string[] effectBurn { get; set; }
        public Image image { get; set; }
        public float[] cooldown { get; set; }
        public string name { get; set; }
    }

    public class Leveltip
    {
        public string[] effect { get; set; }
        public string[] label { get; set; }
    }

    public class Var
    {
        public float[] coeff { get; set; }
        public string link { get; set; }
        public string key { get; set; }
    }
}
