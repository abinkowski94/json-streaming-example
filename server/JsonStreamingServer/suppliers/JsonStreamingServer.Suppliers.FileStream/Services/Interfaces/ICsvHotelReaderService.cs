using JsonStreamingServer.Core.Models.Domain;

namespace JsonStreamingServer.Suppliers.FileStream.Services.Interfaces
{
    public interface ICsvHotelReaderService
    {
        HotelOffer GetNext();
    }
}
