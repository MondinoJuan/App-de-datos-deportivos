using Frontend.Resources.Entities;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ANDROID
    using API_Local;
#endif

namespace Frontend.Resources.API_Clients;
public class Club_ApiClient
{
    private static HttpClient client = new HttpClient();

    static Club_ApiClient()
    {
        client.BaseAddress = new Uri("https://localhost:7279/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }


    //Clubes
    public static async Task AddAsync(Club_Dto club)
    {
        try
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("clubes", club);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error al crear el club: {ex.Message}");
        }
    }

    public static async Task DeleteAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync("clubs/" + id);
        response.EnsureSuccessStatusCode();
    }

    public static async Task<Club_Dto> GetAsync(int id)
    {
        Club_Dto? club = null;
        HttpResponseMessage response = await client.GetAsync("clubs/" + id);
        if (response.IsSuccessStatusCode)
        {
            club = await response.Content.ReadFromJsonAsync<Club_Dto>();
        }
        return club;
    }

    public static async Task<IEnumerable<Club_Dto>> GetAllAsync()
    {
        IEnumerable<Club_Dto>? clubs = null;
        HttpResponseMessage response = await client.GetAsync("clubs");
        if (response.IsSuccessStatusCode)
        {
            clubs = await response.Content.ReadFromJsonAsync<IEnumerable<Club_Dto>>();
        }
        return clubs;
    }

    public static async Task UpdateAsync(Club_Dto club)
    {
        try
        {
            HttpResponseMessage response = await client.PutAsJsonAsync("clubs", club);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error al actualizar el club: {ex.Message}");
        }
    }

    public static async Task DeleteAllAsync()
    {
        HttpResponseMessage response = await client.DeleteAsync("clubs");
        response.EnsureSuccessStatusCode();
    }
}
