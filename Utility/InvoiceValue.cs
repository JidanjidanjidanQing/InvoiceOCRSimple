using System.Text.RegularExpressions;

namespace OCRSimple.Utility
{
    public class InvoiceValue
    {
        public static string readInvoiceValue(string text)
        {
            //去除所有不是数字或点的字符
            return Regex.Replace(text, "[^0-9.]","");
        }
    }
}
