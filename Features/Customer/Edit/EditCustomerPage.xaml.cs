using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechFitCustomer.Features.Customer.Edit;

public partial class EditCustomerPage : ContentPage
{
    public EditCustomerPage(EditCustomerPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}