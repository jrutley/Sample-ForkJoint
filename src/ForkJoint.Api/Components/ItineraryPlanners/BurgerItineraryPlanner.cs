namespace ForkJoint.Api.Components.ItineraryPlanners
{
    using System;
    using System.Threading.Tasks;
    using Activities;
    using Contracts;
    using MassTransit;
    using MassTransit.Courier;
    using MassTransit.Futures;


    public class BeefBurgerItineraryPlanner :
        IItineraryPlanner<OrderBurger<BeefPatty,BeefCondiments>>
    {
        readonly Uri _dressAddress;
        readonly Uri _grillAddress;

        public BeefBurgerItineraryPlanner(IEndpointNameFormatter formatter)
        {
            _grillAddress = new Uri($"exchange:{formatter.ExecuteActivity<GrillBurgerActivity<BeefPatty>, GrillBurgerArguments<BeefPatty>>()}");
            _dressAddress = new Uri($"exchange:{formatter.ExecuteActivity<DressBeefBurgerActivity, DressBeefBurgerArguments>()}");
        }

        public async Task PlanItinerary(FutureConsumeContext<OrderBurger<BeefPatty,BeefCondiments>> context, ItineraryBuilder builder)
        {
            var orderBurger = context.Message;

            builder.AddVariable(nameof(OrderBurger<BeefPatty,BeefCondiments>.OrderId), orderBurger.OrderId);
            builder.AddVariable(nameof(OrderBurger<BeefPatty,BeefCondiments>.OrderLineId), orderBurger.OrderLineId);

            var burger = orderBurger.Burger;

            builder.AddActivity(nameof(GrillBurgerActivity<BeefPatty>), _grillAddress, new
            {
                burger.Patty.Weight,
                burger.Patty.Cheese,
            });

            Guid? onionRingId = default;
            if (burger.Condiments.OnionRing)
            {
                onionRingId = NewId.NextGuid();

                // TODO create a future with address/id
                await context.Publish<OrderOnionRings>(new
                {
                    orderBurger.OrderId,
                    OrderLineId = onionRingId,
                    Quantity = 1
                });
            }

            builder.AddActivity(nameof(DressBeefBurgerActivity), _dressAddress, new
            {
                burger.Condiments.Lettuce,
                burger.Condiments.Pickle,
                burger.Condiments.Onion,
                burger.Condiments.Ketchup,
                burger.Condiments.Mustard,
                burger.Condiments.BarbecueSauce,
                burger.Condiments.OnionRing,
                onionRingId
            });
        }
    }
}