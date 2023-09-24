using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Chirp.CLI;

public class RESTConnection<T> : IRESTConnection<T>
{
    
    private HttpClient client;

    public RESTConnection(string url)
    {
        client = new HttpClient();
        
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(url);
    }
    
    public List<T> getRequest(string endpoint)
    {
        return client.GetFromJsonAsync<List<T>>(endpoint).Result;
    }

    public void postRequest(string endpoint, T record)
    {
        var response = client.PostAsJsonAsync(endpoint, record).Result;
    
        if(!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to post record. Status code: {response.StatusCode}");
        }
    }
}