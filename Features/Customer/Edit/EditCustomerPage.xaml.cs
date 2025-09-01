namespace TechFitCustomer.Features.Customer.Edit;

public partial class EditCustomerPage : ContentPage
{
    public EditCustomerPageViewModel ViewModel { get; }
    
    public EditCustomerPage(EditCustomerPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = viewModel;
    }
}