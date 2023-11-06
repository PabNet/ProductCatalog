using Microsoft.AspNetCore.Http;

public class CookieUtility
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieUtility(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetCookie(string key, string value, int? expireMinutes = null)
    {
        var options = new CookieOptions
        {
            IsEssential = true
        };

        if (expireMinutes.HasValue)
        {
            options.Expires = DateTime.Now.AddMinutes(expireMinutes.Value);
        }

        _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
    }

    public string GetCookie(string key)
    {
        return _httpContextAccessor.HttpContext.Request.Cookies[key];
    }

    public void RemoveCookie(string key)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
    }

    public void UpdateCookie(string key, string value, int? expireMinutes = null)
    {
        if (GetCookie(key) != null)
        {
            RemoveCookie(key);
        }
        SetCookie(key, value, expireMinutes);
    }

    public bool CookieExists(string key)
    {
        return GetCookie(key) != null;
    }
}
