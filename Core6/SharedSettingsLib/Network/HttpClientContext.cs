using SharedSettingsLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SharedSettingsLib.Network;
[InjectScoped]

public class HttpClientContext
{
  private readonly IHttpClientFactory _clientFactory;

  public HttpClientContext(IHttpClientFactory clientFactory)
  {
    _clientFactory = clientFactory;
  }
  public async Task<HttpResponseMessage> GetAsync(string url)
  {
    var client = _clientFactory.CreateClient();
    return await client.GetAsync(url);
  }
  public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content, TimeSpan? timeSpan = null)
  {
    var client = _clientFactory.CreateClient();
    if (timeSpan != null)
    {
      client.Timeout = (TimeSpan)timeSpan;
    }
    return await client.PostAsync(url, content);
  }
  public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
  {
    var client = _clientFactory.CreateClient();
    return await client.PutAsync(url, content);
  }
  public async Task<HttpResponseMessage> DeleteAsync(string url)
  {
    var client = _clientFactory.CreateClient();
    return await client.DeleteAsync(url);
  }

  public async Task<HttpResponseMessage> Send(string url)
  {
    var request = new HttpRequestMessage(HttpMethod.Get, url);
    var client = _clientFactory.CreateClient();
    var response = await client.SendAsync(request);
    return response;
  }

  public async Task<HttpResponseMessage> Send(string url, HttpMethod method)
  {
    var request = new HttpRequestMessage(method, url);
    var client = _clientFactory.CreateClient();
    var response = await client.SendAsync(request);
    return response;
  }

  public async Task<HttpResponseMessage> Send(string url, HttpMethod method, HttpContent content)
  {
    var request = new HttpRequestMessage(method, url);
    request.Content = content;
    var client = _clientFactory.CreateClient();
    var response = await client.SendAsync(request);
    return response;
  }

}
