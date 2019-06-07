# azure-storage-rest-api-authorization
Code snippet for generating the required header for Azure Table Storage REST API.

On the documentation, it is said that the shared key or shared key lite is required for using REST API.
But you can simply just append the sas token at the end of your URI.

```csharp
// grab the Table storage SAS URI string from Azure Portal
var sasUri = new Uri("sas-uri-string"); 

// parse out table storage endpoint
var uriPrefix = sasUri.GetLeftPart(UriPartial.Path).TrimEnd('/'); 

// parse out the token part and no query symbol here
var sasToken = sasUri.Query.TrimStart('?'); 

// sas uri format for entity query operation
var sasUri = string.Format(
                  "{0}/{1}(PartitionKey='{2}',RowKey='{3}')?{4}",
                   uriPrefix,
                   tableName,
                   partitionKey,
                   rowKey,
                   sasToken);

// sas uri format for entity query operation when you add the $filter, same thing apply to $select
var sasUriWithFilter= string.Format(
                  "{0}/{1}()?$filter=PartitionKey%20eq%20'{2}'&{3}",
                  uriPrefix,
                  tableName,
                  partitionKey,
                  sasToken);
```
