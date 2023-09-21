namespace Chirp.CLI;

public interface IRESTConnection<T>
{
    public List<T> getRequest(string endpoint);
    
    public void postRequest(string endpoint, T record);
    
}