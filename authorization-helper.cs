/// <summary>
/// Create a HTTP request message with the default requested header and authorization header
/// </summary>
private HttpRequestMessage CreateSignedHttpRequestMessage(HttpMethod method, string sasUrl)
{
    var requestMessage = new HttpRequestMessage(method, sasUrl);

    requestMessage.Headers.Add("Accept", "application/json;odata=nometadata");
    requestMessage.Headers.Add("DataServiceVersion", "3.0;NetFx");     
    requestMessage.Headers.Add("x-ms-version", "2018-03-28");

    // Etag is required for delete entity operation
    if (method == HttpMethod.Delete) requestMessage.Headers.Add("If-Match", "*"); 

    requestMessage.Headers.Authorization = CreateSharedKeyLiteAuthorizationHeader(requestMessage);
    return requestMessage;
}

/// <summary>
/// Create the signed authorization header for REST API
/// </summary>
private AuthenticationHeaderValue CreateSharedKeyLiteAuthorizationHeader(HttpRequestMessage httpRequestMessage)
{
    httpRequestMessage.Headers.Add("Date", DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture));
    var canonicalizedResource = string.Format("/{0}{1}", this.storageAccountName, httpRequestMessage.RequestUri.AbsolutePath);
    var StringToSign = string.Format(
        "{0}\n{1}",
        httpRequestMessage.Headers.GetValues("Date").FirstOrDefault(),
        canonicalizedResource);

    var SHA256 = new HMACSHA256(Convert.FromBase64String(this.storageAccountKey));
    var signature = Convert.ToBase64String(SHA256.ComputeHash(Encoding.UTF8.GetBytes(StringToSign)));

    return new AuthenticationHeaderValue(
        "SharedKeyLite",
        string.Format("{0}:{1}", this.storageAccountName, signature));
}
