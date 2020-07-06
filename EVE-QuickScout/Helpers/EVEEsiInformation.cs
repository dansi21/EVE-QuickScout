using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;
using ESI.NET;
using ESI.NET.Enumerations;
using ESI.NET.Models;
using ESI.NET.Logic;

namespace EVE_QuickScout
{
    class EVEEsiInformation
    {
        
        public EsiClient client;

        public EVEEsiInformation() {

            var currentDir = Directory.GetCurrentDirectory();

            string path = currentDir + "\\credentials.txt";
            IOptions<EsiConfig> config;

            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                Console.WriteLine("no file");
                throw new Exception("No Credentials Present");
            }
            else {
                string[] lines = System.IO.File.ReadAllLines(path);
                config = Options.Create(new EsiConfig()
                {
                    EsiUrl = "https://esi.evetech.net/",
                    DataSource = DataSource.Tranquility,
                    ClientId = lines[0],
                    SecretKey = lines[1],
                    CallbackUrl = lines[2],
                    UserAgent = lines[3]
                });
            }

            client = new EsiClient(config);
        }

        public async Task<SystemInfo> SearchSystemName(string searchSystem) {


            Console.WriteLine("Test");
            EsiResponse<ESI.NET.Models.Universe.IDLookup> searchResults = await client.Universe.IDs(new List<string>() { searchSystem });
            if (searchResults.Data.Systems.Count == 0)
                return null;
            EsiResponse<ESI.NET.Models.Universe.SolarSystem> SystemData = await client.Universe.System(searchResults.Data.Systems[0].Id);

            SystemInfo toReturn = new SystemInfo();

            int moonCount = 0;
            int beltCount = 0;
            foreach(ESI.NET.Models.Universe.Planet temp in SystemData.Data.Planets){
                if(temp.AsteroidBelts != null)
                    beltCount += temp.AsteroidBelts.Count();
                if(temp.Moons != null)
                    moonCount += temp.Moons.Count();
            }
            toReturn.SystemName = SystemData.Data.Name;
            toReturn.Planets = SystemData.Data.Planets.Count();
            toReturn.Moons = moonCount;
            toReturn.Belts = beltCount;
            

            //toReturn.Moons

            //searchResults.Data.Systems[0].Name
            return toReturn;
        }

    }
}
