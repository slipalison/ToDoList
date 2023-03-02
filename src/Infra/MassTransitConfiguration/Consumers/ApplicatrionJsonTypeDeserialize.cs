using System.Net.Mime;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infra.MassTransitConfiguration.Consumers;

internal class ApplicatrionJsonTypeDeserialize : ISerializerFactory
{
    private readonly ILogger _logger;

    public ApplicatrionJsonTypeDeserialize(ILogger logger)
    {
        _logger = logger;
    }
    public IMessageSerializer CreateSerializer()
    {
        return new ApplicatrionJsonDeserialize(_logger);
    }

    public IMessageDeserializer CreateDeserializer()
    {
        return new ApplicatrionJsonDeserialize(_logger);
    }

    public ContentType ContentType => new("application/json");
}