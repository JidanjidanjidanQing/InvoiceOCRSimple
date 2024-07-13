using System.Text.RegularExpressions;

namespace OCRSimple.Utility
{
    public class InvoiceCode
    {
        public static string isRuleWorked(string text)
        {
            if (text.Length < 3)
            {
                return null;
            }

            int i = 0;
            while (i < text.Length)
            {
                if (!char.IsDigit(text[i]))
                    return null;
                i++;
            }

            return text;
        }

        public static string readInvoiceCode(string text)
        {
            string invoiceCode = null;

            if (true)
            {
                var invoiceNumberRegex = new Regex(@"发票号码：(\d+)");
                var invoiceNumberMatch = invoiceNumberRegex.Match(text);
                invoiceCode = invoiceNumberMatch.Groups[1].Value;

                if (InvoiceCode.isRuleWorked(invoiceCode) != null)
                    return invoiceCode;
            }

            //2024.05.25 更新规则
            if (true)
            {
                int i = 5;

                while (i < text.Length && char.IsDigit(text[i]))
                {
                    i++;
                }

                invoiceCode = text.Substring(5, i - 5);

                if (InvoiceCode.isRuleWorked(invoiceCode) != null)
                    return invoiceCode;
            }

            return null;
        }
    }
}
