/* REQUEST
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
    <soap:Body>
        <NumberToWords xmlns="http://www.dataaccess.com/webservicesserver/">
            <ubiNum>500</ubiNum>
        </NumberToWords>
    </soap:Body>
</soap:Envelope>
 */

/* RESPONSE
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope">
    <soap:Body>
        <m:NumberToWordsResponse xmlns:m="http://www.dataaccess.com/webservicesserver/">
            <m:NumberToWordsResult>five hundred </m:NumberToWordsResult>
        </m:NumberToWordsResponse>
    </soap:Body>
</soap:Envelope>
 */
using System.Text;
using System.Xml;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/ws", async () =>
{
    var client = new HttpClient();

    XmlSerializer ser = new(typeof(Envelope<Body>));
    var xml = "";

    using (var sww = new StringWriter())
    {
        using (XmlWriter writer = XmlWriter.Create(sww))
        {
            ser.Serialize(writer, new Envelope<Body> { Body = new() { NumberToWords = new() { UbiNum = 500 } } });
            xml = sww.ToString(); // Your XML
        }
    }

    var r = await SoapResponseHelper.GetSoapResponse<NumberToWordsResponse>("https://www.dataaccess.com/webservicesserver/NumberConversion.wso", xml);
    return Results.Ok(r);
});

app.Run();

[XmlRoot("m", ElementName = "NumberToWordsResponse", Namespace = "http://www.dataaccess.com/webservicesserver/")]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://www.dataaccess.com/webservicesserver/")]
public class NumberToWordsResponse
{
    [XmlElement("m", ElementName = "NumberToWordsResult")]
    public string? NumberToWordsResult { get; set; }
}

[XmlRoot("soap", ElementName = "Body")]
[System.Runtime.Serialization.DataContractAttribute()]
public class ResponseBody
{
    public NumberToWordsResponse? NumberToWordsResponse { get; set; }
}

[XmlRoot(ElementName = "NumberToWords", Namespace = "http://www.dataaccess.com/webservicesserver/")]
public class NumberToWords
{
    [XmlElement(ElementName = "ubiNum", Namespace = "http://www.dataaccess.com/webservicesserver/")]
    public int UbiNum { get; set; }
}

[XmlRoot("soap", ElementName = "Body")]
[System.Runtime.Serialization.DataContractAttribute()]
public class Body
{
    [XmlElement("m", ElementName = "NumberToWords", Namespace = "http://www.dataaccess.com/webservicesserver/")]
    public NumberToWords? NumberToWords { get; set; }
}

[XmlRoot("soap", ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
[System.Runtime.Serialization.DataContractAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class Envelope<T>
{
    public T? Body { get; set; }
}

public static class SoapResponseHelper
{
    public static async Task<T> GetSoapResponse<T>(string webServiceUrl, string requestString)
    {
        var uri = new Uri(webServiceUrl);
        var httpContent = new StringContent(requestString, Encoding.UTF8, "text/xml");

        var response = FetchSoapResponse<T>(uri, httpContent);
        return await response;
    }

    private static async Task<T> FetchSoapResponse<T>(Uri uri, HttpContent httpContent)
    {
        using (var client = new HttpClient())
        {
            var result = await client.PostAsync(uri, httpContent);
            var resultContent = result.Content.ReadAsStringAsync().Result;

            var responseContainer = DeserializeInnerSoapObject<T>(resultContent);
            return responseContainer;
        }
    }

    private static T DeserializeInnerSoapObject<T>(string soapResponse)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(soapResponse);

        var soapBody = xmlDocument.GetElementsByTagName("soap:Body")[0];
        string innerObject = soapBody!.InnerXml;

        return XmlHelper.Deserializer<T>(innerObject);
    }
}

public static class XmlHelper
{
    public static T Deserializer<T>(string objectAsString)
    {
        Console.WriteLine(objectAsString);
        XmlSerializer deserializer = new XmlSerializer(typeof(T));

        using (StringReader reader = new StringReader(objectAsString))
        {
            return (T)deserializer.Deserialize(reader)!;
        }
    }
}
