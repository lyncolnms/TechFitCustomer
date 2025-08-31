using CommunityToolkit.Mvvm.ComponentModel;

namespace TechFitCustomer.Features.Base;

public partial class BasePageViewModel : ObservableObject, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {

    }
}