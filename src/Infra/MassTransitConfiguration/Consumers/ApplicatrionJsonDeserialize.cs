using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using MassTransit;
using MassTransit.Serialization;
using Microsoft.Extensions.Logging;

namespace Infra.MassTransitConfiguration.Consumers;

[ExcludeFromCodeCoverage]
internal class ApplicatrionJsonDeserialize :  SystemTextJsonRawMessageSerializer, IMessageSerializer,
    IMessageDeserializer
{
    private readonly ILogger _logger;

    public ApplicatrionJsonDeserialize(ILogger logger)
    {
        _logger = logger;
    }

    public void Probe(ProbeContext context)
    {
        Task.Delay(1);
    }

    public ConsumeContext Deserialize(ReceiveContext receiveContext)
    {
        _logger.LogInformation("Desetialize message: {receiveContext} | CorrelationId {Correlation}", receiveContext.Body.GetString(), receiveContext.GetCorrelationId());
        
        var msg = base.Deserialize(receiveContext.Body, receiveContext.TransportHeaders, receiveContext.InputAddress);
        var cxt = new BodyConsumeContext(receiveContext,msg);
        return cxt;
    }

    public SerializerContext Deserialize(MessageBody body, Headers headers, Uri? destinationAddress = null)
    {
        _logger.LogInformation("Desetialize message: {receiveContext}", body.GetString());
        return base.Deserialize(body, headers, destinationAddress);
    }

    public MessageBody GetMessageBody<T>(SendContext<T> context) where T : class
    {
        _logger.LogInformation("Desetialize message: {@Message}", context.Message);
        return base.GetMessageBody(context);
    }
    public ContentType ContentType => new("application/json");
    
}