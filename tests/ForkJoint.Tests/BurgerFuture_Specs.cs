namespace ForkJoint.Tests
{
    using System.Threading.Tasks;
    using Api.Components.Activities;
    using Api.Components.Futures;
    using Api.Components.ItineraryPlanners;
    using Api.Services;
    using Contracts;
    using MassTransit;
    using MassTransit.ExtensionsDependencyInjectionIntegration;
    using MassTransit.Futures;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;


    [TestFixture]
    public class BurgerFuture_Specs :
        FutureTestFixture
    {
        [Test]
        public async Task Should_complete()
        {
            var orderId = NewId.NextGuid();
            var orderLineId = NewId.NextGuid();

            var scope = Provider.CreateScope();

            var client = scope.ServiceProvider.GetRequiredService<IRequestClient<OrderBurger<BeefPatty, BeefCondiments>>>();

            Response<BurgerCompleted<BeefPatty,BeefCondiments>> response = await client.GetResponse<BurgerCompleted<BeefPatty,BeefCondiments>>(new
            {
                OrderId = orderId,
                OrderLineId = orderLineId,
                Burger = new Burger<BeefPatty,BeefCondiments>
                {
                    BurgerId = orderLineId,
                    Patty = new BeefPatty{
                    Weight = 1.0m,
                    Cheese = true},
                    Condiments = new BeefCondiments{
                        Ketchup = true
                    }
                }
            });

            Assert.That(response.Message.OrderId, Is.EqualTo(orderId));
            Assert.That(response.Message.OrderLineId, Is.EqualTo(orderLineId));
            Assert.That(response.Message.Burger.Patty.Cheese, Is.True);
            Assert.That(response.Message.Burger.Patty.Weight, Is.EqualTo(1.0m));
        }

        [Test]
        public async Task Should_fault()
        {
            var orderId = NewId.NextGuid();
            var orderLineId = NewId.NextGuid();

            var scope = Provider.CreateScope();

            var client = scope.ServiceProvider.GetRequiredService<IRequestClient<OrderBurger<BeefPatty,BeefCondiments>>>();

            try
            {
                await client.GetResponse<BurgerCompleted<BeefPatty,BeefCondiments>>(new
                {
                    OrderId = orderId,
                    OrderLineId = orderLineId,
                    Burger = new Burger<BeefPatty,BeefCondiments>
                    {
                        BurgerId = orderLineId,
                        Patty = new BeefPatty {
                        Weight = 1.0m,
                        Cheese = true},
                        Condiments = new BeefCondiments {
                        Lettuce = true
                        }
                    }
                });

                Assert.Fail("Should have thrown");
            }
            catch (RequestFaultException exception)
            {
                Assert.That(exception.Fault.Host, Is.Not.Null);
                Assert.That(exception.Message, Contains.Substring("lettuce"));
            }
        }

        protected override void ConfigureServices(IServiceCollection collection)
        {
            collection.AddSingleton<IGrill, Grill>();
            collection.AddScoped<IItineraryPlanner<OrderBurger<BeefPatty,BeefCondiments>>, BeefBurgerItineraryPlanner>();
        }

        protected override void ConfigureMassTransit(IServiceCollectionBusConfigurator configurator)
        {
            configurator.AddActivitiesFromNamespaceContaining<GrillBurgerActivity<BeefPatty>>();

            configurator.AddFuture<BurgerFuture<BeefPatty, BeefCondiments>>();
            configurator.AddFuture<OnionRingsFuture>();
        }
    }
}