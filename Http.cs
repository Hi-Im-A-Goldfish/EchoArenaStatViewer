using System;
using System.Net;
using System.Collections.Specialized;

public class Http
{
	public static byte[] Post(string uri, NameValueCollection pairs)
    {
        using (WebClient webClient = new WebClient())
        {
            return webClient.UploadValues(uri, pairs);
        }
    }
}
