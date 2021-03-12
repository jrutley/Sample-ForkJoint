namespace ForkJoint.Api.Components.Activities
{
    using System.Threading.Tasks;
    using ForkJoint.Contracts;
    using MassTransit.Courier;
    using Microsoft.Extensions.Logging;
    using Services;


    public class GrillBurgerActivity<T> :
        IActivity<GrillBurgerArguments<T>, GrillBurgerLog<T>> where T: BurgerPatty
    {
        readonly IGrill _grill;
        readonly ILogger<GrillBurgerActivity<T>> _logger;

        public GrillBurgerActivity(ILogger<GrillBurgerActivity<T>> logger, IGrill grill)
        {
            _logger = logger;
            _grill = grill;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<GrillBurgerArguments<T>> context)
        {
            var patty = await _grill.CookOrUseExistingPatty(context.Arguments.BurgerType);//.Weight, context.Arguments.Cheese);

            return context.CompletedWithVariables<GrillBurgerLog<T>>(new {patty}, new {patty});
        }

        public Task<CompensationResult> Compensate(CompensateContext<GrillBurgerLog<T>> context)
        {
            var patty = context.Log.Patty;

            _logger.LogDebug("Putting Burger back in inventory: {patty}", patty);

            _grill.Add(patty);

            return Task.FromResult(context.Compensated());
        }
    }
}