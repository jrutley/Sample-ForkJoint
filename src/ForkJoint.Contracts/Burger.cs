namespace ForkJoint.Contracts
{
    using System;
    using System.Text;
    using MassTransit.Topology;

    public record Fish
    {
        public bool Mayo { get; init; }
    }

    public record Chicken
    {
        public bool Mayo { get; init; }
        public bool Breaded { get; init; }
    }

    [ExcludeFromTopology]
    public interface Condiments { }

    public record BeefCondiments : Condiments
    {
        public bool Ketchup { get; init; }
        public bool Lettuce { get; init; }
        //Cheese = patty.Cheese,
        public bool Onion { get; init; }
        public bool Pickle { get; init; }
        public bool Mustard { get; init; }
        public bool BarbecueSauce { get; init; }
        public bool OnionRing { get; init; }

        public override string ToString()
        {
            StringBuilder sb = new();
            if (Lettuce)
                sb.Append(" Lettuce");
            if (Pickle)
                sb.Append(" Pickle");
            if (Onion)
                sb.Append(" Onion");
            if (Ketchup)
                sb.Append(" Ketchup");
            if (Mustard)
                sb.Append(" Mustard");
            if (BarbecueSauce)
                sb.Append(" BBQ");
            if (OnionRing)
                sb.Append(" OnionRing");
            return sb.ToString();
        }
    }

    public record Burger<TPatty, TCondiments> where TCondiments : Condiments
    {
        //public bool Mayo { get; init; }
        //public bool Breaded { get; init; }
        //public bool Ketchup { get; init; }
        public TPatty Patty { get; init; }
        public TCondiments Condiments {get; init; }
        public Guid BurgerId { get; init; }
        //public decimal Weight { get; init; } = 0.5m;
        //public bool Lettuce { get; init; }
        //public bool Cheese { get; init; }
        // public bool Pickle { get; init; } = true;
        // public bool Onion { get; init; } = true;
        // public bool Mustard { get; init; } = true;
        // public bool BarbecueSauce { get; init; }
        // public bool OnionRing { get; init; }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.AppendFormat($"Burger: {Patty}");

            // if (Cheese)
            //     sb.Append(" Cheese");

            sb.Append(Condiments.ToString());

            return sb.ToString();
        }
    }
}