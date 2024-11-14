using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using E_Commerce_Web.Service.VNPay;


namespace E_Commerce_Web.Utilities
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IVnPayService, VnPayService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}