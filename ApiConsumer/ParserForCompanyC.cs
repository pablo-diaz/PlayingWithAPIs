using System;
using System.Xml;
using System.Xml.Linq;

using CSharpFunctionalExtensions;

namespace ApiConsumer
{
    public sealed class ParserForCompanyC
    {
        public Result<decimal> Parse(string apiResponse) =>
            string.IsNullOrEmpty(apiResponse)
            ? Result.Failure<decimal>("API response was not provided")
            : ParseXML(xmlDocument: apiResponse);


        private Result<decimal> ParseXML(string xmlDocument)
        {
            try
            {
                var xmlElement = XElement.Parse(xmlDocument.ToLower());
                if(xmlElement.Name.LocalName != "quote")
                    return Result.Failure<decimal>("API response was not formated as expected");

                var maybeTotal = GetTotal(fromXmlElement: xmlElement);
                if (maybeTotal.HasNoValue)
                    return Result.Failure<decimal>("API response was not formated as expected");

                return Convert.ToDecimal(maybeTotal.Value);
            }
            catch(FormatException)
            {
                return Result.Failure<decimal>("API response was not formated as expected");
            }
            catch (XmlException)
            {
                return Result.Failure<decimal>("API response was not formated as expected");
            }
        }

        private Maybe<string> GetTotal(XElement fromXmlElement) =>
            (string) fromXmlElement.Attribute("total") ?? Maybe<string>.None;
    }
}
