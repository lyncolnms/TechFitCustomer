using System.Text.Json;

namespace TechFitCustomer.Services;

public class PreferenceService : IPreferenceService
{
    public void Set<T>(string key, T value)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("A chave não pode ser nula ou vazia", nameof(key));

        if (value == null)
        {
            Remove(key);
            return;
        }

        try
        {
            if (IsPrimitiveType(typeof(T)))
            {
                Preferences.Set(key, value.ToString());
            }
            else
            {
                string json = JsonSerializer.Serialize(value);
                Preferences.Set(key, json);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao salvar a preferência com a chave '{key}'", ex);
        }
    }

    public T? Get<T>(string key, T? defaultValue = default)
    {
        if (string.IsNullOrWhiteSpace(key) || !ContainsKey(key)) return defaultValue;

        try
        {
            var storedValue = Preferences.Get(key, string.Empty);
            
            if (string.IsNullOrEmpty(storedValue)) return defaultValue;

            if (IsPrimitiveType(typeof(T)))
            {
                return (T)Convert.ChangeType(storedValue, typeof(T));
            }

            return JsonSerializer.Deserialize<T>(storedValue);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public bool ContainsKey(string key)
    {
        return !string.IsNullOrWhiteSpace(key) && Preferences.ContainsKey(key);
    }

    public void Remove(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return;

        try
        {
            Preferences.Remove(key);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao remover a preferência com a chave '{key}'", ex);
        }
    }

    public void Clear()
    {
        try
        {
            Preferences.Clear();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao limpar todas as preferências", ex);
        }
    }

    private static bool IsPrimitiveType(Type type)
    {
        return type.IsPrimitive || 
               type == typeof(string) || 
               type == typeof(decimal) || 
               type == typeof(DateTime) || 
               type == typeof(DateTimeOffset) ||
               type == typeof(TimeSpan) ||
               type == typeof(Guid) ||
               (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && 
                IsPrimitiveType(Nullable.GetUnderlyingType(type)!));
    }
}
