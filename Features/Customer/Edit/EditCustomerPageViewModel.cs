using TechFitCustomer.Features.Base;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace TechFitCustomer.Features.Customer.Edit;

public partial class EditCustomerPageViewModel : BasePageViewModel
{
    public string Name { get; set; }
    public string Lastname { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }

    public ICommand SaveCommand { get; }

    public EditCustomerPageViewModel()
    {
        SaveCommand = new RelayCommand(Save);
    }

    private void Save()
    {
        // Implement save logic for editing customer here
    }
}