using System.Windows.Input;

namespace TechFitCustomer.Features.Customer.Views;

public partial class CustomerFormView : ContentView
{
    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(CustomerFormView), string.Empty, BindingMode.TwoWay);

    public static readonly BindableProperty LastnameProperty =
        BindableProperty.Create(nameof(Lastname), typeof(string), typeof(CustomerFormView), string.Empty, BindingMode.TwoWay);

    public static readonly BindableProperty AgeProperty =
        BindableProperty.Create(nameof(Age), typeof(string), typeof(CustomerFormView), string.Empty, BindingMode.TwoWay);

    public static readonly BindableProperty AddressProperty =
        BindableProperty.Create(nameof(Address), typeof(string), typeof(CustomerFormView), string.Empty, BindingMode.TwoWay);

    public static readonly BindableProperty SaveCommandProperty =
        BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(CustomerFormView));

    public static readonly BindableProperty CancelCommandProperty =
        BindableProperty.Create(nameof(CancelCommand), typeof(ICommand), typeof(CustomerFormView));

    public static readonly BindableProperty FormTitleProperty =
        BindableProperty.Create(nameof(FormTitle), typeof(string), typeof(CustomerFormView), "Adicionar Cliente");

    public static readonly BindableProperty ButtonTextProperty =
        BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(CustomerFormView), "Salvar");

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public string Lastname
    {
        get => (string)GetValue(LastnameProperty);
        set => SetValue(LastnameProperty, value);
    }

    public string Age
    {
        get => (string)GetValue(AgeProperty);
        set => SetValue(AgeProperty, value);
    }

    public string Address
    {
        get => (string)GetValue(AddressProperty);
        set => SetValue(AddressProperty, value);
    }

    public ICommand SaveCommand
    {
        get => (ICommand)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    public ICommand CancelCommand
    {
        get => (ICommand)GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public string FormTitle
    {
        get => (string)GetValue(FormTitleProperty);
        set => SetValue(FormTitleProperty, value);
    }

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public CustomerFormView()
    {
        InitializeComponent();
    }
}
