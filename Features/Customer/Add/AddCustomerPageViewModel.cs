using CommunityToolkit.Mvvm.Input;
using TechFitCustomer.Features.Base;

namespace TechFitCustomer.Features.Customer.Add;

public partial class AddCustomerPageViewModel : BasePageViewModel
{
    [RelayCommand]
    private async Task Save()
    {
        //save customer in Preferences
    }
}