using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TechFitCustomer.Features.Base;
using TechFitCustomer.Models;
using TechFitCustomer.Services;

namespace TechFitCustomer.Features.Customer.Add;

public partial class AddCustomerPageViewModel(
    IPreferenceService preferenceService,
    INavigationService navigationService) : BasePageViewModel(navigationService)
{
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _lastname = string.Empty;
    [ObservableProperty] private string _age = string.Empty;
    [ObservableProperty] private string _address = string.Empty;

    [RelayCommand]
    private async Task Save()
    {
        Window? currentWindow = Application.Current?.Windows.FirstOrDefault();
        Page? currentPage = currentWindow?.Page;

        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Lastname))
        {
            if (currentPage is not null)
                await currentPage.DisplayAlert("Erro", "Nome e Sobrenome são obrigatórios", "OK");

            return;
        }

        if (!int.TryParse(Age, out int ageValue) || ageValue <= 0)
        {
            if (currentPage is not null)
                await currentPage.DisplayAlert("Erro", "Idade deve ser um número válido maior que zero", "OK");

            return;
        }

        CustomerModel newCustomer = new CustomerModel
        {
            Name = Name.Trim(),
            Lastname = Lastname.Trim(),
            Age = ageValue,
            Address = Address.Trim()
        };

        List<CustomerModel> existingCustomers = preferenceService.Get<List<CustomerModel>>("customers") ?? [];

        existingCustomers.Add(newCustomer);

        preferenceService.Set("customers", existingCustomers);

        WeakReferenceMessenger.Default.Send(new CustomerChangedMessage());

        if (currentPage is not null) await currentPage.DisplayAlert("Sucesso", "Cliente adicionado com sucesso!", "OK");

        IReadOnlyList<Window>? windows = Application.Current?.Windows;
        Window? window = windows?.FirstOrDefault(w => w.Page is AddCustomerPage);
        if (window is not null)
        {
            Application.Current?.CloseWindow(window);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        IReadOnlyList<Window>? windows = Application.Current?.Windows;
        Window? window = windows?.FirstOrDefault(w => w.Page is AddCustomerPage);
        if (window is not null)
        {
            Application.Current?.CloseWindow(window);
        }
    }
}