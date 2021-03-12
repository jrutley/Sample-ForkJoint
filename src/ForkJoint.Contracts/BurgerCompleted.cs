namespace ForkJoint.Contracts
{
    public interface BurgerCompleted<T, TCondiments> :
        OrderLineCompleted where TCondiments: Condiments
    {
        Burger<T, TCondiments> Burger { get; }
    }
}