using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TechFitCustomer.Features.Base;
using TechFitCustomer.Features.Customer.Add;
using TechFitCustomer.Features.Customer.Edit;
using TechFitCustomer.Models;
using TechFitCustomer.Services;

namespace TechFitCustomer.Features.Main;

public partial class MainPageViewModel : BasePageViewModel, IRecipient<CustomerChangedMessage>
{
    private readonly IPreferenceService _preferenceService;
    
    [ObservableProperty] private List<CustomerModel> _customers = [];

    public MainPageViewModel(IPreferenceService preferenceService)
    {
        _preferenceService = preferenceService;
        LoadCustomers();
        
        // Registrar para receber mensagens de mudança de cliente
        WeakReferenceMessenger.Default.Register(this);
    }

    private void LoadCustomers()
    {
        List<CustomerModel>? savedCustomers = _preferenceService.Get<List<CustomerModel>>("customers");
        
        if (savedCustomers == null || savedCustomers.Count == 0)
        {
            Customers = [];
        }
        else
        {
            Customers = savedCustomers;
        }
    }

    public void RefreshCustomers()
    {
        LoadCustomers();
    }

    [RelayCommand]
    private void Add()
    {
        IServiceProvider? serviceProvider = Application.Current?.Handler?.MauiContext?.Services;
        AddCustomerPage? addPage = serviceProvider?.GetService<AddCustomerPage>();
        
        if (addPage is not null)
        {
            Window window = new Window(addPage)
            {
                Title = "Adicionar Cliente",
                Width = 500,
                Height = 400
            };

#if WINDOWS
            CenterWindow(window);
#endif
            
            window.Stopped += (_, _) => RefreshCustomers();
            
            Application.Current?.OpenWindow(window);
        }
    }

    [RelayCommand]
    private void Edit(CustomerModel customer)
    {
        IServiceProvider? serviceProvider = Application.Current?.Handler?.MauiContext?.Services;
        EditCustomerPage? editPage = serviceProvider?.GetService<EditCustomerPage>();
        
        if (editPage is not null)
        {
            editPage.ViewModel.LoadCustomer(customer);
            Window window = new Window(editPage)
            {
                Title = "Editar Cliente",
                Width = 500,
                Height = 400
            };

#if WINDOWS
            CenterWindow(window);
#endif
            
            window.Stopped += (_, _) => RefreshCustomers();
            
            Application.Current?.OpenWindow(window);
        }
    }

#if WINDOWS
    private static void CenterWindow(Window window)
    {
        window.Created += (_, _) =>
        {
            if (window.Handler?.PlatformView is Microsoft.UI.Xaml.Window nativeWindow)
            {
                IntPtr handle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                Microsoft.UI.Windowing.AppWindow? appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                
                if (appWindow != null)
                {
                    Microsoft.UI.Windowing.DisplayArea? displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Primary);
                    if (displayArea != null)
                    {
                        Windows.Graphics.RectInt32 workArea = displayArea.WorkArea;
                        double centerX = (workArea.Width - window.Width) / 2 + workArea.X;
                        double centerY = (workArea.Height - window.Height) / 2 + workArea.Y;
                        
                        appWindow.MoveAndResize(new Windows.Graphics.RectInt32(
                            (int)centerX,
                            (int)centerY,
                            (int)window.Width,
                            (int)window.Height));
                    }
                }
            }
        };
    }
#endif

    [RelayCommand]
    private async Task Delete(CustomerModel customer)
    {
        Window? currentWindow = Application.Current?.Windows.FirstOrDefault();
        Page? currentPage = currentWindow?.Page;
        
        if (currentPage != null)
        {
            bool result = await currentPage.DisplayAlert("Confirmar", $"Deseja realmente excluir o cliente {customer.Name} {customer.Lastname}?", "Sim", "Não");

            if (result)
            {
                List<CustomerModel> updatedCustomers = Customers.Where(c => c.Id != customer.Id).ToList();
                Customers = updatedCustomers;

                _preferenceService.Set("customers", Customers);

                await currentPage.DisplayAlert("Sucesso", "Cliente excluído com sucesso!", "OK");
            }
        }
    }

    public void Receive(CustomerChangedMessage message)
    {
        RefreshCustomers();
    }
}