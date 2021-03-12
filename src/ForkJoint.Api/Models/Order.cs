namespace ForkJoint.Api.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Contracts;


    public class Order
    {
        [Required]
        public Guid OrderId { get; init; }

        public Burger<BeefPatty,BeefCondiments>[] Burgers { get; init; }
        // Chicken
        // Fish
        public Fry[] Fries { get; init; }
        public Shake[] Shakes { get; init; }
        public FryShake[] FryShakes { get; init; }
    }
}