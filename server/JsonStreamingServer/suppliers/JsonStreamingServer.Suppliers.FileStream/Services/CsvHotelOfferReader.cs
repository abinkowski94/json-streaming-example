using JsonStreamingServer.Core.Models.Domain;
using Microsoft.Extensions.Logging;

namespace JsonStreamingServer.Suppliers.FileStream.Services
{
    public class CsvHotelOfferReader : IDisposable
    {
        private readonly ILogger _logger;
        private readonly string _csvFilePath;
        private readonly int _minimumFieldsExpected = 9;
        private readonly bool _hasHeader;
        private readonly StreamReader _streamReader;

        private bool headerRead = false;

        public CsvHotelOfferReader(ILogger<CsvHotelOfferReader> logger, string csvFilePath, bool hasHeader = true)
        {
            _logger = logger;
            _csvFilePath = csvFilePath;
            _hasHeader = hasHeader;
            _streamReader = new StreamReader(_csvFilePath);
        }

        public async Task<HotelOffer?> GetNextOfferAsync(CancellationToken cancellationToken)
        {
            if (_streamReader is null)
            {
                return null;
            }

            if (_hasHeader && !headerRead)
            {
                await _streamReader.ReadLineAsync(cancellationToken);
                headerRead = true;
            }

            var line = await _streamReader.ReadLineAsync(cancellationToken);
            if (line is null)
            {
                return null;
            }

            return MapToHotelOffer(line);
        }

        private HotelOffer? MapToHotelOffer(string line)
        {
            HotelOffer? result = null;

            var fields = line.Split(',');

            if (fields is null || fields.Length < _minimumFieldsExpected)
            {
                return result;
            }

            try
            {
                result = MapToHotelOffer(fields);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while mapping csv data input");
            }

            return result;
        }

        private static HotelOffer MapToHotelOffer(string[] fields)
        {
            return new HotelOffer
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

        public void Dispose()
        {
            _streamReader.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
