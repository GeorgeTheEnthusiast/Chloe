using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chloe.Client.Converters
{
    public static class FlightWebsiteConverter
    {
        public static ChloeDataSet.CarriersRow Convert(ChloeDataSet.FlightWebsitesRow websitesRow)
        {
            var carrierAdapter = new Chloe.Client.ChloeDataSetTableAdapters.CarriersTableAdapter();
            var flightDataSet = new Chloe.Client.ChloeDataSet();
            carrierAdapter.Fill(flightDataSet.Carriers);

            switch (websitesRow.Id)
            {
                case 1:
                    return flightDataSet.Carriers.Where(x => x.Id == 1).FirstOrDefault();
                case 2:
                    return flightDataSet.Carriers.Where(x => x.Id == 2).FirstOrDefault();
                case 3:
                    return flightDataSet.Carriers.Where(x => x.Id == 106).FirstOrDefault();
                default:
                    throw new NotSupportedException(string.Format("This flight website [{0}] is not supported!", websitesRow.Id));
            }
        }
    }
}
