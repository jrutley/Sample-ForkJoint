namespace ForkJoint.Api.Services
{
    using System.Threading.Tasks;
    using Contracts;


    public interface IGrill
    {
        Task<BurgerPatty> CookOrUseExistingPatty(BurgerPatty props);//decimal weight, bool cheese);
        void Add(BurgerPatty patty);
    }
}