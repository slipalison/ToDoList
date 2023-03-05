using Domain.Contracts.Services;
using MassTransit;
using NSubstitute;
using WebApi;

namespace UnitTest.IntegratedTests;

public abstract class ConsumerBaseTest<TMessage> : AbstractIntegratedTest where TMessage : class, IEventBase
{
    protected readonly ConsumeContext<TMessage> Context = Substitute.For<ConsumeContext<TMessage>>();
    protected IConsumer<TMessage> Consumer;

    public abstract void MockMessage();

    protected abstract IConsumer<TMessage> BuildConsumer();

    [Fact]
    public async Task ComsumeTest()
    {
        Consumer = BuildConsumer();
        
        await Consumer.Consume(Context);
        await Context.Received()
            .NotifyConsumed(Arg.Any<ConsumeContext<TMessage>>(),
                Arg.Any<TimeSpan>(),
                Arg.Any<string>());
    }

    protected ConsumerBaseTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        MockMessage();
    }


    // protected void MockPuplish(string routingKey)
    // {
    //     BusCustom.WhenForAnyArgs(async publish =>
    //             await publish.Publish(Arg.Any<TMessage>(), x =>
    //             {
    //                 x.SetRoutingKey(routingKey);
    //                 x.CorrelationId = CorrelationService.GetCorrelationId();
    //             }))
    //         .Do(info =>
    //         {
    //             var value = info.Arg<TMessage>();
    //             Context.Message.Returns(value);
    //             Consumer.Consume(Context).GetAwaiter().GetResult();
    //         });
    // }
}