using CommunityToolkit.Mvvm.ComponentModel;
using TechFitCustomer.Services;

namespace TechFitCustomer.Features.Base;

public partial class BasePageViewModel(INavigationService navigationService) : ObservableObject, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {

    }
}