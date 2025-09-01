using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TechFitCustomer.Features.Base;
using TechFitCustomer.Models;
using TechFitCustomer.Services;

namespace TechFitCustomer.Features.Customer.Edit;

public partial class EditCustomerPageViewModel(IPreferenceService preferenceService) : BasePageViewModel
{
    private CustomerModel? _originalCustomer;
    
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _lastname = string.Empty;
    [ObservableProperty] private string _age = string.Empty;
    [ObservableProperty] private string _address = string.Empty;

    public void LoadCustomer(CustomerModel customer)
    {
        _originalCustomer = customer;
        Name = customer.Name;
        Lastname = customer.Lastname;
        Age = customer.Age.ToString();
        Address = customer.Address;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (_originalCustomer == null) return;

        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Lastname))
        {
            await ShowAlert("Erro", "Nome e Sobrenome são obrigatórios");
            return;
        }

        if (!int.TryParse(Age, out int ageValue) || ageValue <= 0)
        {
            await ShowAlert("Erro", "Idade deve ser um número válido maior que zero");
            return;
        }

        List<CustomerModel> customers = preferenceService.Get<List<CustomerModel>>("customers") ?? [];
        
        int customerIndex = customers.FindIndex(c => c.Id == _originalCustomer.Id);
        
        if (customerIndex >= 0)
        {
            customers[customerIndex] = new CustomerModel
            {
                Id = _originalCustomer.Id,
                Name = Name.Trim(),
                Lastname = Lastname.Trim(),
                Age = ageValue,
                Address = Address.Trim()
            };
            
            preferenceService.Set("customers", customers);

            WeakReferenceMessenger.Default.Send(new CustomerChangedMessage());

            await ShowAlert("Sucesso", "Cliente atualizado com sucesso!");
            CloseWindow();
        }
        else
        {
            await ShowAlert("Erro", "Cliente não encontrado");
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseWindow();
    }

    private async Task ShowAlert(string title, string message)
    {
        Page? currentPage = Application.Current?.Windows?.FirstOrDefault()?.Page;
        if (currentPage is not null) await currentPage.DisplayAlert(title, message, "OK");
    }

    private void CloseWindow()
    {
        Window? window = Application.Current?.Windows?.FirstOrDefault(w => w.Page is EditCustomerPage);
        if (window is not null) Application.Current?.CloseWindow(window);
    }
}