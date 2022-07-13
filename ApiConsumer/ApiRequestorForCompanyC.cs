using System.Xml;
using System.Linq;
using System.Threading.Tasks;

using CSharpFunctionalExtensions;

namespace ApiConsumer
{
    public sealed class ApiRequestorForCompanyC: IApiRequestor
    {
        private readonly IApiClient _apiClient;

        public ApiRequestorForCompanyC(IApiClient apiClient)
        {
            this._apiClient = apiClient;
        }

        public async Task<Result<decimal>> Request(RequestInfo forInfo)
        {
            if (forInfo == null)
                return Result.Failure<decimal>("Please provide a valid request info");

            var apiResponseResult = await this._apiClient.PerformRequest(withBody: BuildRequestBody(forInfo));
            if (apiResponseResult.IsFailure)
                return Result.Failure<decimal>(apiResponseResult.Error);

            return new ApiResponseParserForCompanyC().Parse(apiResponse: apiResponseResult.Value);
        }

        private string BuildRequestBody(RequestInfo basedOnInfo)
        {
            var xmlDocument = new XmlDocument();

            var requestNode = xmlDocument.CreateElement(name: "Request");
            xmlDocument.AppendChild(requestNode);

            requestNode.AppendChild(BuildNode(nodeName: "Source", withAttributeName: "address",
                withAttributeValue: basedOnInfo.SourceAddress.Value, basedOnXmlDocument: xmlDocument));

            requestNode.AppendChild(BuildNode(nodeName: "Destination", withAttributeName: "address",
                withAttributeValue: basedOnInfo.DestinationAddress.Value, basedOnXmlDocument: xmlDocument));

            requestNode.AppendChild(BuildPackagesNode(
                withCartonDimensions: basedOnInfo.CartonDimensions.ToArray(), basedOnXmlDocument: xmlDocument));

            return xmlDocument.ToString();
        }

        private XmlNode BuildNode(string nodeName, string withAttributeName,
            string withAttributeValue, XmlDocument basedOnXmlDocument)
        {
            var node = basedOnXmlDocument.CreateElement(name: nodeName);
            var attributeNode = basedOnXmlDocument.CreateAttribute(name: withAttributeName);
            attributeNode.Value = withAttributeValue;
            node.Attributes.Append(attributeNode);

            return node;
        }

        private XmlNode BuildPackagesNode(CartonDimension[] withCartonDimensions, XmlDocument basedOnXmlDocument)
        {
            var packagesNode = basedOnXmlDocument.CreateElement(name: "Packages");
            foreach(var cartonDimension in withCartonDimensions)
            {
                var cartonNode = basedOnXmlDocument.CreateElement(name: "Package");
                packagesNode.AppendChild(cartonNode);

                var heightAttributeNode = basedOnXmlDocument.CreateAttribute(name: "height");
                heightAttributeNode.Value = cartonDimension.Height.ToString();
                cartonNode.AppendChild(heightAttributeNode);

                var widthAttributeNode = basedOnXmlDocument.CreateAttribute(name: "width");
                widthAttributeNode.Value = cartonDimension.Width.ToString();
                cartonNode.AppendChild(widthAttributeNode);
            }

            return packagesNode;
        }
    }
}
