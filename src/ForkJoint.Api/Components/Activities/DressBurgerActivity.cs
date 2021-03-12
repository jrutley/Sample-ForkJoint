namespace ForkJoint.Api.Components.Activities
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using MassTransit.Courier;
    using Microsoft.Extensions.Logging;


    public class DressBeefBurgerActivity :
        IExecuteActivity<DressBeefBurgerArguments>
    {
        readonly ILogger<DressBeefBurgerActivity> _logger;
        readonly IRequestClient<OrderOnionRings> _onionRingClient;

        public DressBeefBurgerActivity(ILogger<DressBeefBurgerActivity> logger, IRequestClient<OrderOnionRings> onionRingClient)
        {
            _logger = logger;
            _onionRingClient = onionRingClient;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<DressBeefBurgerArguments> context)
        {
            var arguments = context.Arguments;

            var patty = arguments.Patty;
            if (patty == null)
                throw new ArgumentNullException(nameof(arguments.Patty));

            _logger.LogDebug("Dressing Burger: {OrderId} {Ketchup} {Lettuce}", arguments.OrderId, arguments.Ketchup,
                arguments.Lettuce);

            if (arguments.Lettuce)
                throw new InvalidOperationException("No lettuce available");

            if (arguments.OnionRing)
            {
                Guid? onionRingId = arguments.OnionRingId ?? NewId.NextGuid();

                _logger.LogDebug("Ordering Onion Ring: {OrderId}", onionRingId);

                Response<OnionRingsCompleted> response = await _onionRingClient.GetResponse<OnionRingsCompleted>(new
                {
                    arguments.OrderId,
                    OrderLineId = onionRingId,
                    Quantity = 1
                }, context.CancellationToken);
            }

            var burger = new Burger<BeefPatty, BeefCondiments>
            {
                BurgerId = arguments.BurgerId,
                //Weight = patty.Weight,
                Patty = patty,
                Condiments = new BeefCondiments {
                //Cheese = patty.Cheese,
                Lettuce = arguments.Lettuce,
                Onion = arguments.Onion,
                Pickle = arguments.Pickle,
                //Ketchup = arguments.Ketchup,
                Mustard = arguments.Mustard,
                BarbecueSauce = arguments.BarbecueSauce,
                OnionRing = arguments.OnionRing
                }
            };

            return context.CompletedWithVariables(new {burger});
        }
    }
}