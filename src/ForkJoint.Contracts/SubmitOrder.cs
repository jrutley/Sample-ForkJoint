namespace ForkJoint.Contracts
{
    using System;


    public interface SubmitOrder
    {
        Guid OrderId { get; }

        Burger<BeefPatty, BeefCondiments>[] Burgers { get; }
        // Burger<ChickenPatty, ChickenCondiments>[] Burgers { get; }
        // Burger<FishPatty, FishCondiments>[] Burgers { get; }
        Fry[] Fries { get; }
        Shake[] Shakes { get; }
        FryShake[] FryShakes { get; }
    }
}