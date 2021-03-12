namespace ForkJoint.Api.Components.Activities
{
    using Contracts;


    public interface GrillBurgerLog<T> where T: BurgerPatty
    {
        T Patty { get; }
    }
}