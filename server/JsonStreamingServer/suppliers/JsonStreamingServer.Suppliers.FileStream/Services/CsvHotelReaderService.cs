using JsonStreamingServer.Suppliers.FileStream.Services.Interfaces;
using JsonStreamingServer.Core.Models.Domain;
using CsvHelper;
using Serilog;
using System.Globalization;
using System.Reflection;

namespace JsonStreamingServer.Suppliers.FileStream.Services
{
    public class CsvHotelReaderService : ICsvHotelReaderService
    {
        private readonly ILogger _logger;
        private int _index = 0;
        private readonly List<HotelOffer> HotelOffersData = new();

        public CsvHotelReaderService(ILogger logger)
        {
            _logger = logger;
            HotelOffersData = GetCsvContents("HotelOffers");
        }

        public HotelOffer GetNext()
        {
            var nextData = HotelOffersData.ElementAt(_index);

            _index = (_index + 1) % HotelOffersData.Count;

            return nextData;
        }

        private List<HotelOffer> GetCsvContents(string CsvFileName)
        {
            var ReturnContents = new List<HotelOffer>();

            try
            {
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @$"Data\{CsvFileName}.csv");

                using (var ReadCsv = new CsvReader(new StreamReader(path), CultureInfo.InvariantCulture))
                {
                    ReturnContents.AddRange(ReadCsv.GetRecords<HotelOffer>());
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Error: {e.Message}");
            }

            return ReturnContents;
        }
    }
}
