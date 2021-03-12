namespace ForkJoint.Api.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Microsoft.Extensions.Logging;


    public class Grill :
        IGrill
    {
        readonly ILogger<Grill> _logger;
        readonly HashSet<BurgerPatty> _patties;

        public Grill(ILogger<Grill> logger)
        {
            _logger = logger;
            _patties = new HashSet<BurgerPatty>();
        }

        public async Task<BurgerPatty> CookOrUseExistingPatty(BurgerPatty patty) //T>(T patty)//decimal weight, bool cheese)
        {
            //var existing = _patties.FirstOrDefault(x => x.Cheese == cheese && x.Weight == weight);
            var existing = _patties.Contains(patty); // .FirstOrDefault(x => x == patty);
            //if (existing != null)
            if (existing)
            {
                _logger.LogDebug($"Using existing patty {patty}"); //{Weight} {Cheese}", existing.Weight, existing.Cheese);

                _patties.Remove(patty);
                return patty;
            }

            //_logger.LogDebug("Grilling patty {Weight} {Cheese}", weight, cheese);
            _logger.LogDebug("Grilling patty {patty}", patty);

            await Task.Delay((int)patty.CookTime()); //5000 + (int)(1000.0m * weight));

            // We should probably be making a new object and passing in properties from this method
            // var newPatty = new BurgerPatty
            // {
            //     Weight = weight,
            //     Cheese = cheese
            // };

            //return newPatty;
            return patty;
        }

        public void Add(BurgerPatty patty)
        {
            _patties.Add(patty);
        }
    }
}