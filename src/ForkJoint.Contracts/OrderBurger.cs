namespace ForkJoint.Contracts
{
    public interface OrderBurger<T, TCondiments> :
        OrderLine where TCondiments: Condiments
    {
        Burger<T, TCondiments> Burger { get; }
    }
}