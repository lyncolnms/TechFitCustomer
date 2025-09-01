namespace TechFitCustomer.Features.Customer.Add;

public partial class AddCustomerPage : ContentPage
{
    public AddCustomerPage(AddCustomerPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}