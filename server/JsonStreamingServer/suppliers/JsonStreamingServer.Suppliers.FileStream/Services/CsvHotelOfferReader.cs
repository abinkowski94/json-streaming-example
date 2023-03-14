using JsonStreamingServer.Core.Models.Domain;

namespace JsonStreamingServer.Suppliers.FileStream.Services
{
    public class CsvHotelOfferReader : IDisposable
    {
        private readonly string _csvFilePath;
        private string _csvHeader;
        private readonly StreamReader _streamReader;

        public CsvHotelOfferReader(string csvFilePath)
        {
            _csvFilePath = csvFilePath;
            _csvHeader = string.Empty;
            _streamReader = new StreamReader(_csvFilePath);
        }

        public async Task<HotelOffer?> GetNextOfferAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_csvHeader))
            {
                _csvHeader = await _streamReader.ReadLineAsync(cancellationToken);
            }

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

            HotelOffer result = null;

            try
            {
                result = new HotelOffer
                {
                    Id = Guid.TryParse(fields[0], out Guid id) ? id : Guid.Empty,
                    Avaliability = new DateRange
                    {
                        From = DateOnly.TryParse(fields[1], out DateOnly from) ? from : DateOnly.MaxValue,
                        To = DateOnly.TryParse(fields[2], out DateOnly to) ? to : DateOnly.MinValue,
                    },
                    Supplier = nameof(FileStream),
                    TotalPrice = new Price
                    {
                        Value = int.TryParse(fields[3], out int value) ? value : 0,
                        Currency = fields[4]
                    },
                    ExternalId = fields[5],
                    Name = fields[6],
                    Content = new Content
                    {
                        Description = fields[7],
                        ShortDescription = fields[8],
                        Images = null
                    },
                    PriceBreakdown = null
                };
            }
            catch (Exception)
            {
                //We should log this
            }

            return result;
        }

        public void Dispose()
        {
            _streamReader.Dispose();
        }
    }
}
