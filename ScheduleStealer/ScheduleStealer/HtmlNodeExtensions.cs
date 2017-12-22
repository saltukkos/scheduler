using System.Linq;
using HtmlAgilityPack;

namespace Scheduler.ScheduleStealer
{
    internal static class HtmlNodeExtensions
    {
        internal static HtmlNode ChildWithClass(this HtmlNode node, string value)
        {
            return node.Descendants().FirstOrDefault(
                x =>
                    x.Attributes.AttributesWithName("class").FirstOrDefault()?.Value.Contains(value) == true);
        }

        internal static string Attribute(this HtmlNode node, string attribute)
        {
            return node.Attributes.AttributesWithName(attribute).FirstOrDefault()?.Value;
        }
    }
}