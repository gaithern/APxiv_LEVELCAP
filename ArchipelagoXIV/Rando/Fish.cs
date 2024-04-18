using System;
using System.Linq;

namespace ArchipelagoXIV.Rando
{
    internal class FishData
    {
        //public string Name;
        public int Id;
        public int Level;
        public Region[] Regions;
        public Dictionary<string, string[]> Baits;
    }

    internal class Fish : Location
    {
        public Fish(ApState apState, long id, string name) : base(apState, id, name)
        {
            Data = APData.FishData[name];
            region = Data.Regions[0];
        }

        public FishData Data { get; }


        public override bool IsAccessible()
        {
            if (Completed)
                return false;
            if (stale)
            {
                stale = false;

                var allMissingLocations = apState?.session?.Locations?.AllMissingLocations;
                if (allMissingLocations == null)
                    return Accessible = false;
                if (!allMissingLocations.Contains(ApId))
                    return Accessible = false;
                if (!Logic.Level(Data.Level, "FSH")(apState, false))
                    return Accessible = false;
                var region = Data.Regions.FirstOrDefault(r => RegionContainer.CanReach(apState, r) && apState.Items.Any(i => Data.Baits[r.Name].Contains(i)));
                if (region == null)
                    return Accessible = false;
                else
                    this.region = region;
                return Accessible = true;
            }
            return Accessible;
        }
    }
}
