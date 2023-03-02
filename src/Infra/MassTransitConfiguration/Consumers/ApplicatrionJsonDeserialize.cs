using System.Net.Mime;
using MassTransit;
using MassTransit.Serialization;

namespace Infra.MassTransitConfiguration.Consumers;

internal class ApplicatrionJsonDeserialize : SystemTextJsonRawMessageSerializer, IMessageSerializer,
    IMessageDeserializer
{
    public ApplicatrionJsonDeserialize()
    {
    }

    public void Probe(ProbeContext context)
    {
        Task.Delay(1);
    }

    public ConsumeContext Deserialize(ReceiveContext receiveContext)
    {
        //_log 
        return base.Deserialize(receiveContext);
    }

    public SerializerContext Deserialize(MessageBody body, Headers headers, Uri? destinationAddress = null)
    {
        //_log
        return base.Deserialize(body, headers, destinationAddress);
    }

    public MessageBody GetMessageBody<T>(SendContext<T> context) where T : class
    {
        //_log
        return base.GetMessageBody(context);
    }
    public ContentType ContentType => new("application/json");
    
}