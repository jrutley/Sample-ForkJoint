namespace ForkJoint.Api.Components.StateMachines
{
    using System;
    using Automatonymous.Contracts;
    using Automatonymous.Requests;
    using GreenPipes;
    using GreenPipes.Partitioning;
    using MassTransit;
    using MassTransit.Definition;


    public class RequestSagaDefinition :
        SagaDefinition<RequestState>
    {
        public RequestSagaDefinition()
        {
            var partitionCount = 64;

            ConcurrentMessageLimit = partitionCount;
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<RequestState> sagaConfigurator)
        {
            var partitionCount = ConcurrentMessageLimit ?? Environment.ProcessorCount * 4;

            IPartitioner partitioner = new Partitioner(partitionCount, new Murmur3UnsafeHashGenerator());

            endpointConfigurator.UsePartitioner<RequestStarted>(partitioner, x => x.Message.CorrelationId);
            endpointConfigurator.UsePartitioner<RequestCompleted>(partitioner, x => x.Message.CorrelationId);
            endpointConfigurator.UsePartitioner<RequestFaulted>(partitioner, x => x.Message.CorrelationId);

            endpointConfigurator.UseScheduledRedelivery(r => r.Intervals(1000));
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100));
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}