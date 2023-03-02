using System.Net.Mime;
using MassTransit;

namespace Infra.MassTransitConfiguration.Consumers;

internal class ApplicatrionJsonTypeDeserialize : ISerializerFactory
{
    public IMessageSerializer CreateSerializer()
    {
        return new ApplicatrionJsonDeserialize();
    }

    public IMessageDeserializer CreateDeserializer()
    {
        return new ApplicatrionJsonDeserialize();
    }

    public ContentType ContentType => new("application/json");
}