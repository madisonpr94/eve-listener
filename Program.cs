using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EveListener.Model;
using EveListener.Service;

namespace EveListener
{
    class Program
    {

        static async Task Main(string[] args)
        {
            string pathToDb = args.Count() > 0 ? args.First() : "";
            string pathToEve = args.Count() > 1 ? args[1] : "";

            // attempt to fetch data from Eve
            EveDataService eveSvc = new EveDataService(pathToEve);
            Measurement data = await eveSvc.GetMeasurement();

            // 
            DbService dbSvc = new DbService(pathToDb);
            dbSvc.AddMeasurement(data);
        }
    }
}
