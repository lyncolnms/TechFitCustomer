namespace TechFitCustomer.Features.Customer.Add;

public partial class AddCustomerPage : ContentPage
{
    public AddCustomerPage()
    {
        InitializeComponent();
        BindingContext = new AddCustomerPageViewModel();
    }
}