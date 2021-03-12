namespace ForkJoint.Api.Components.Activities
{
    using System;
    using Contracts;


    public interface DressBeefBurgerArguments
    {
        Guid OrderId { get; }
        Guid BurgerId { get; }

        BeefPatty Patty { get; }

        bool Lettuce { get; }
        bool Pickle { get; }
        bool Onion { get; }
        bool Ketchup { get; }
        bool Mustard { get; }
        bool BarbecueSauce { get; }
        bool OnionRing { get; }

        Guid? OnionRingId { get; }
    }
}