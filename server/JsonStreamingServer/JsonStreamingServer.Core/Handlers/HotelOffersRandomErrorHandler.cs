﻿using JsonStreamingServer.Core.Abstractions.Handlers;
using JsonStreamingServer.Core.Models.Domain;
using JsonStreamingServer.Core.Models.Requests;
using JsonStreamingServer.Core.Models.Results;
using System.Runtime.CompilerServices;

namespace JsonStreamingServer.Core.Handlers
{
    public class HotelOffersRandomErrorHandler : AbstractHotelOfferStreamHandler
    {
        private readonly Random _random;

        public HotelOffersRandomErrorHandler(IHotelOfferStreamHandler nextHanlder)
            : base(nextHanlder)
        {
            _random = new Random();
        }

        public async override IAsyncEnumerable<Result<HotelOffer>> GetHotelOffersAsync(
            GetHotelOffersRequest request,
            IAsyncEnumerable<Result<HotelOffer>> inputStream,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var response in inputStream)
            {
                var value = _random.NextDouble();

                if (request.ErrorChance.HasValue && value < request.ErrorChance)
                {
                    yield return new Exception("Oops, there was an error!");
                }
                else
                {
                    yield return response;
                }
            }
        }
    }
}
