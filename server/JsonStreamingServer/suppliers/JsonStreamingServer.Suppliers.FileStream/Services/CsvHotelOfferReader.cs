using JsonStreamingServer.Core.Models.Domain;

namespace JsonStreamingServer.Suppliers.FileStream.Services
{
    public class CsvHotelOfferReader : IDisposable
    {
        private readonly string _csvFilePath;
        private readonly StreamReader _streamReader;

        public CsvHotelOfferReader(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
            _streamReader = new StreamReader(_csvFilePath);
        }

        public async Task<HotelOffer?> GetNextOfferAsync(CancellationToken cancellationToken)
        {
            var line = await _streamReader.ReadLineAsync(cancellationToken);

            if (line == null)
            {
                return null;
            }

            return MapToHotelOffer(line);
        }

        private HotelOffer MapToHotelOffer(string line)
        {
            var fields = line.Split(',');

            return new HotelOffer
            {
                Id = Guid.Empty,
                Avaliability = new DateRange
                {
                    From = DateOnly.MinValue,
                    To = DateOnly.MaxValue
                },
                Supplier = nameof(FileStream),
                TotalPrice = new Price
                {
                    Currency = "USD",
                    Value = 100
                }
            };
        }

        public void Dispose()
        {
            _streamReader.Dispose();
        }
    }
}
