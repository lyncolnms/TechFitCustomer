using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TechFitCustomer.Features.Base;
using TechFitCustomer.Features.Customer.Add;
using TechFitCustomer.Models;

namespace TechFitCustomer.Features.Main;

public partial class MainPageViewModel : BasePageViewModel
{
    [ObservableProperty] private List<CustomerModel> _customers = [];

    public MainPageViewModel()
    {
        Customers =
        [
            new CustomerModel { Name = "Juan", Lastname = "Perez", Age = 25, Address = "Calle 123" },
            new CustomerModel { Name = "Maria", Lastname = "Gomez", Age = 30, Address = "Avenida 456" },
            new CustomerModel { Name = "Carlos", Lastname = "Lopez", Age = 28, Address = "Boulevard 789" },
            new CustomerModel { Name = "Jose", Lastname = "Gonzalez", Age = 29, Address = "Calle 123" },
            new CustomerModel { Name = "Pedro", Lastname = "Garcia", Age = 30, Address = "Avenida 456" },
            new CustomerModel { Name = "Ana", Lastname = "Gonzalez", Age = 28, Address = "Boulevard 789" }
        ];
    }

    [RelayCommand]
    private void Add()
    {
        Application.Current?.OpenWindow(new Window(new AddCustomerPage()));
    }

    [RelayCommand]
    private async Task Edit(CustomerModel customer)
    {
        await Application.Current?.MainPage.DisplayAlert("Edit", $"Edit customer {customer.Name}", "OK");
    }

    [RelayCommand]
    private async Task Delete(CustomerModel customer)
    {
        await Application.Current?.MainPage.DisplayAlert("Delete", $"Delete customer {customer.Name}", "OK");
    }
}