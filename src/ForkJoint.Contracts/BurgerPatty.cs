namespace ForkJoint.Contracts
{
    public interface BurgerPatty
    {
        string ToString();
        decimal CookTime();
    }

    public class BeefPatty : BurgerPatty {
        public decimal Weight { get; init; }
        public bool Cheese { get; init; }

        public override string ToString()
        {
            return $"{Weight}, {Cheese}";
        }
        public decimal CookTime(){
            return 5000 + 1000 * Weight;
        }
    }
}