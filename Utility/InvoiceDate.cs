using System.Globalization;

namespace OCRSimple.Utility
{
    public class InvoiceDate
    {
        public static string formatDate(string text)
        {
            DateTime date;

            // 尝试解析日期字符串
            if (DateTime.TryParseExact(text, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                // 如果解析成功，返回格式化的日期字符串
                return date.ToString("yyyy-MM-dd");
            }

            // 如果解析失败，可以选择返回一个特定的值或者空字符串
            // 这里我们选择返回空字符串
            return string.Empty;
        }
        public static string isRuleWorked(string text)
        {
            if (text.Length != 10)
            {
                return null;
            }

            int i = 0;
            while (i < text.Length)
            {
                if (!text[i].Equals('-') && !char.IsDigit(text[i]))
                    return null;
                i++;
            }

            return text;
        }

        public static string readInvoiceDate(string text)
        {
            string invoiceDate = null;

            if (true)
            {
                //var invoiceDateRegex = new Regex(@"开票日期：(\d{4}年\d{2}月\d{2}日)");
                //var invoiceDateMatch = invoiceDateRegex.Match(text);
                //invoiceDate = invoiceDateMatch.Groups[1].Value;
                //invoiceDate = Regex.Replace(invoiceDate, @"年|月", "-");
                //invoiceDate = Regex.Replace(invoiceDate, @"日", "");

                invoiceDate = text.Substring(5,4) 
                    + text.Substring(text.IndexOf("年")+1, 2) 
                    + text.Substring(text.IndexOf("月")+1, 2);

                invoiceDate = InvoiceDate.formatDate(invoiceDate);

                if (InvoiceDate.isRuleWorked(invoiceDate) != null)
                    return invoiceDate;
            }

            //2024.05.25 更新规则
            if (true)
            {
                int i = 5;

                while (i < text.Length && char.IsDigit(text[i]))
                {
                    i++;
                }

                invoiceDate = InvoiceDate.formatDate(text.Substring(5, i - 5));

                if (InvoiceDate.isRuleWorked(invoiceDate) != null)
                    return invoiceDate;
            }
            return null;
        }
    }
}
