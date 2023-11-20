using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlTypes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.XPath;
using Avalonia.Platform.Storage;
using OneText.Client.Models;

namespace OneText.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IStorageProvider _storageProvider;
    private readonly NameValueCollection _appSettings;
    private AuthState? _authState;
    public AuthState AuthStateInformation => _authState;
    public event Action<AuthService,LoginState>? OnChange;


    public AuthService()
    {
        _httpClient = App.Services.GetService(typeof(HttpClient)) as HttpClient ??
                           throw new NullReferenceException();
        //_storageProvider = App.Services.GetService(typeof(IStorageProvider)) as IStorageProvider ??
        //                        throw new NullReferenceException();
        _appSettings = ConfigurationManager.AppSettings;
        
    }
    
    public async void Initialize()
    {
        var token = _appSettings["authToken"];
        if (token != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _authState = await _httpClient.GetFromJsonAsync<AuthState>("/profile");
            OnChange?.Invoke(this, LoginState.LoggedIn);
        }
        else
        {
            OnChange?.Invoke(this, LoginState.LoggedOut);
        }
    } 
     

    public async Task<AuthResult> Register(RegisterModel registerModel)
    {
        var result = await _httpClient.PostAsJsonAsync("/register", registerModel);
        if (result.IsSuccessStatusCode) return new AuthResult(true);
        var errorJson = await result.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<Dictionary<string,List<string>>>(errorJson);
        return new AuthResult(false,error);

    }

    public async Task<AuthResult> Login(LoginModel loginModel)
    {
        var result = await _httpClient.PostAsync($"/login?Email={UrlEncoder.Default.Encode(loginModel.Email)}&Password={UrlEncoder.Default.Encode(loginModel.Password)}", null);
        if (result.IsSuccessStatusCode)
        {
            var authState = await JsonSerializer.DeserializeAsync<AuthState>(await result.Content.ReadAsStreamAsync());

            
            
            _appSettings["authToken"] = authState!.Token;
            _authState = authState;
            
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authState!.Token);
            OnChange?.Invoke(this, LoginState.LoggedIn);
            return new AuthResult(true);
        }

        var errors = await JsonSerializer.DeserializeAsync<Dictionary<string,List<string>>>(await result.Content.ReadAsStreamAsync());
        return new AuthResult(false,errors);

    }

    public void Logout()
    {
        _appSettings["authToken"] = null;
        OnChange?.Invoke(this, LoginState.LoggedOut);
    }

    public enum LoginState
    {
        LoggedIn,
        LoggedOut
    }
    

    public record struct AuthResult(bool Success, Dictionary<string, List<string>>? Errors = null)
    {
        public static implicit operator bool(AuthResult result) => result.Success;  
        
    };

    public class AuthState
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = default!;
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = default!;
        [JsonPropertyName("email")]
        public string Email { get; set; } = default!;
        [JsonPropertyName("token")]
        public string Token { get; init; } = default!;
    }


}