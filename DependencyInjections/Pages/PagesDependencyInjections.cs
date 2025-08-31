using CommunityToolkit.Maui;
using TechFitCustomer.Features.Customer.Add;
using TechFitCustomer.Features.Customer.Edit;
using TechFitCustomer.Features.Main;

namespace TechFitCustomer.DependencyInjections.Pages;

public static class PagesDependencyInjections
{
    public static void AddPagesDependencyInjections(this IServiceCollection services)
    {
        services.AddTransient<MainPage, MainPageViewModel>();
        services.AddTransient<AddCustomerPage, AddCustomerPageViewModel>();
        services.AddTransient<EditCustomerPage, EditCustomerPageViewModel>();
    }
}