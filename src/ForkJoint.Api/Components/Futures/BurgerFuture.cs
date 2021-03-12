namespace ForkJoint.Api.Components.Futures
{
    using Contracts;
    using MassTransit.Courier;
    using MassTransit.Futures;


    public class BurgerFuture<T, TCondiment> :
        Future<OrderBurger<T, TCondiment>, BurgerCompleted<T, TCondiment>> where TCondiment: Condiments
    {
        public BurgerFuture()
        {
            ConfigureCommand(x => x.CorrelateById(context => context.Message.OrderLineId));

            ExecuteRoutingSlip(x => x
                .OnRoutingSlipCompleted(r => r
                    .SetCompletedUsingInitializer(context =>
                    {
                        var burger = context.Message.GetVariable<Burger<T,TCondiment>>(nameof(BurgerCompleted<T,TCondiment>.Burger));

                        return new
                        {
                            Burger = burger,
                            Description = burger.ToString()
                        };
                    })));
        }
    }
}
