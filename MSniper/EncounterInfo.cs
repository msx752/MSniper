using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSniper.Enums;

namespace MSniper
{
    public class EncounterInfo : IDisposable
    {
        public short PokemonId { get; set; }
        public long EncounterId { get; set; }
        public string SpawnPointId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Iv { get; set; }
        //public double Iv { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
