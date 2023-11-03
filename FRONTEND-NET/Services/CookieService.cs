using Microsoft.JSInterop;
using System.Threading.Tasks;

public class CookieService
{
    public static async Task<string> ReadCookieValue(IJSRuntime jsRuntime)
    {
        var js = $"document.cookie.split('; ').find(c => c.startsWith('email='))";
        var cookie = await jsRuntime.InvokeAsync<string>("eval", js);

        if (!string.IsNullOrEmpty(cookie))
        {
            return cookie.Substring(6);
        }
        else
        {
            // Redirigir a la página de inicio de sesión con JSRuntime.
            var redirectToLoginScript = "window.location.href = '/login';";
            await jsRuntime.InvokeVoidAsync("eval", redirectToLoginScript);
            return null; // No se encontró la cookie, se redirigió a la página de inicio de sesión.
        }
    }
    public static async Task RedirectToHomeIfCookieExists(IJSRuntime jsRuntime)
    {
        var js = $"document.cookie.split('; ').find(c => c.startsWith('email='))";
        var cookie = await jsRuntime.InvokeAsync<string>("eval", js);

        if (!string.IsNullOrEmpty(cookie))
        {
            var redirectToHomeScript = "window.location.href = '/';";
            await jsRuntime.InvokeVoidAsync("eval", redirectToHomeScript);
        }
    }
    public static async Task<int?> ReadUserCookie(IJSRuntime jsRuntime)
    {
        var js = "document.cookie.split(';').find(c => c.trim().startsWith('user='))";
        var cookie = await jsRuntime.InvokeAsync<string>("eval", js);

        if (!string.IsNullOrEmpty(cookie))
        {
            var userIndex = cookie.IndexOf("user=");
            if (userIndex >= 0)
            {
                userIndex += 5; // Longitud de "user=".
                var userString = cookie.Substring(userIndex).Trim();
                if (int.TryParse(userString, out int user))
                {
                    return user;
                }
            }
        }

        return null; // No se encontró la cookie o no se pudo analizar el valor como entero.
    }
}
