namespace ForkJoint.Api.Components.Activities
{
    using System;


    public interface GrillBurgerArguments<T>
    {
        Guid OrderId { get; }
        Guid BurgerId { get; }

        T BurgerType {get;}
   }

   public interface GrillBeefBurgerArguments {
        decimal Weight { get; }
        bool Cheese { get; }
   }
}