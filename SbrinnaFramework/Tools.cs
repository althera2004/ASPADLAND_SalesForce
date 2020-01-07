// --------------------------------
// <copyright file="tools.cs" company="OpenFramework">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework
{
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Web;

    /// <summary>Framweork tools</summary>
    public static class Tools
    {
        public static string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
            {
                return string.Empty;
            }

            if (base64EncodedData.EndsWith("%3d", StringComparison.Ordinal))
            {
                base64EncodedData = base64EncodedData.Replace("%3d", "=");
            }

            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string GeneratePassword
        {
            get
            {
                return RandomPassword.Generate(5, 8);
            }
        }

        public static string LimitLength(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length < length)
            {
                return text;
            }

            return text.Substring(0, length);
        }

        public static string Capitalize(string value)
        {
            return Capitalize(value, true);
        }

        public static string Capitalize(string value, bool onlyFirst)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }

            value = value.Trim();
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.Length == 1)
            {
                return value.ToUpperInvariant();
            }

            value = value.ToLowerInvariant();

            if (onlyFirst)
            {
                return value.Substring(0, 1).ToUpperInvariant() + value.Substring(1).ToLowerInvariant();
            }

            var res = new StringBuilder();
            foreach (string word in value.Split(' '))
            {
                res.Append(Capitalize(word)).Append(" ");
            }

            return res.ToString().Trim();
        }

        /// <summary>
        /// Elimina el texto entre paréntisis de la entrada
        /// </summary>
        /// <param name="input">Texto de entrada</param>
        /// <returns>Texto sin paréntesis</returns>
        public static string CleanText(string input)
        {
            return input.Split('"')[0].Split('(')[0].Trim();
        }

        public static string GetReferenceValue(Entity entity, string fieldName)
        {
            if (!entity.Contains(fieldName))
            {
                return string.Empty;
            }

            return ((EntityReference)entity[fieldName]).Name;
        }

        public static Guid GetReferenceId(Entity entity, string fieldName)
        {
            if (!entity.Contains(fieldName))
            {
                return Guid.Empty;
            }

            return ((EntityReference)entity[fieldName]).Id;
        }

        public static string GetAliasedValue(Entity entity, string fieldName)
        {
            if (!entity.Contains(fieldName))
            {
                return string.Empty;
            }

            var alias = (AliasedValue)entity[fieldName];

            if (alias.Value.GetType().ToString() == "System.Guid" || alias.Value.GetType().ToString() == "System.String" || alias.Value.GetType().ToString() == "System.Boolean" || alias.Value.GetType().ToString() == "System.Int32")
            {
                return alias.Value.ToString();
            }

            if (alias.Value.GetType().ToString() == "System.Double")
            {
                return Convert.ToDouble(alias.Value).ToString("###0.000000", CultureInfo.GetCultureInfo("en-us")).Replace(',', '.');
            }

            var referencia = (EntityReference)alias.Value;

            return referencia.Name;
        }

        public static Guid GetAliasedId(Entity entity, string fieldName)
        {
            if (!entity.Contains(fieldName))
            {
                return Guid.Empty;
            }

            var alias = (AliasedValue)entity[fieldName];

            switch (alias.Value.GetType().ToString())
            {
                case "Microsoft.Xrm.Sdk.EntityReference":
                    return ((EntityReference)alias.Value).Id;
                case "System.Guid":
                default:
                    return new Guid(alias.Value.ToString());
            }

            return Guid.Empty;
        }

        public static string GetOptionValue(Entity entity, string fieldName)
        {
            if (!entity.Contains(fieldName))
            {
                return string.Empty;
            }

            var option = (OptionSetValue)entity[fieldName];

            return option.Value.ToString();
        }

        public static string GetStringValue(Entity entity, string fieldName)
        {
            if (!entity.Contains(fieldName))
            {
                return string.Empty;
            }

            return entity[fieldName].ToString();
        }

        public static decimal GetMoneyValue(Entity entity, string fieldName)
        {
            if (!entity.Contains(fieldName))
            {
                return 0;
            }

            var data = (Money)entity[fieldName];
            return Convert.ToDecimal(data.Value);
        }

        public static string PdfMoneyFormat(decimal value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", value).Replace(",", "*").Replace(".", ",").Replace("*", ".");
        }

        /// <summary>
        /// Make a string with decimal format
        /// </summary>
        /// <param name="value">Decimal value</param>
        /// <returns>String with decimal format</returns>
        public static string NumberFormat(decimal? value)
        {
            if (value.HasValue)
            {
                return NumberFormat(value.Value);
            }

            return string.Empty;
        }

        /// <summary>
        /// Make a string with decimal format
        /// </summary>
        /// <param name="value">Decimal value</param>
        /// <returns>String with decimal format</returns>
        public static string NumberFormat(decimal value)
        {
            string res = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.000000}", value);
            while (res.EndsWith("0"))
            {
                res = res.Substring(0, res.Length - 1);
            }

            if (res.EndsWith("."))
            {
                res = res.Substring(0, res.Length - 1);
            }

            return res.Replace(".", ",");
        }

        /// <summary>
        /// Creates resume for a text
        /// </summary>
        /// <param name="text">Text to resume</param>
        /// <param name="length">Maximum length</param>
        /// <returns>Resume of text</returns>
        public static string Resume(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            text = text.Substring(0, length);
            int pos = text.LastIndexOf(' ');
            if (pos != -1)
            {
                text = text.Substring(0, pos);
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}...", text);
        }

        /// <summary>
        /// Prepare a texto to be HTML compliant
        /// </summary>
        /// <param name="text">Text to Htmlize</param>
        /// <returns>Html compliant text</returns>
        public static string SetHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string res = text.Replace("\"", "&quot;");
            res = res.Replace("&", "&amp;");
            return res;
        }

        /// <summary>
        /// Prepares a text to be a tooltip
        /// </summary>
        /// <param name="text">Texto to prepare</param>
        /// <returns>Text ready for to be a tooltip</returns>
        public static string SetTooltip(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace("\"", "˝");
        }

        /// <summary>
        /// Create a JSON pair key/value from a string value
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">String value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, string value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":""{1}""", key, JsonCompliant(value));
        }

        /// <summary>
        /// Create a JSON pair key/value from a string value
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">String value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, bool value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":{1}", key, value == true ? "true" : "false");
        }

        /// <summary>
        /// Create a JSON pair key/value from a DateTime value
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">DateTime value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, DateTime value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":{1:yyyyMMdd}", key, value);
        }

        /// <summary>
        /// Create a JSON pair key/value from a integer value
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">INteger value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, int value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":{1}", key, value);
        }

        /// <summary>
        /// Create a JSON pair key/value from a long value
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Long value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, long value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":{1}", key, value);
        }

        /// <summary>
        /// Create a JSON pair key/value from a decimal value
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":{1:#0.00}", key, value);
        }

        /// <summary>
        /// Create a JSON pair key/value from a decimal value
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <param name="decimals">Number of decimals</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal value, int decimals)
        {
            string pattern = @"""{0}"":{1:#0.";
            for (int x = 0; x < decimals; x++)
            {
                pattern += "0";
            }

            pattern += "}";
            return string.Format(CultureInfo.GetCultureInfo("en-us"), pattern, key, value);
        }

        /// <summary>
        /// Create a JSON pair key/value from a decimal nullable value
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal? value)
        {
            if (value.HasValue)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":{1:#0.00}", key, value);
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":null", key);
        }

        /// <summary>
        /// Create a JSON pair key/value from a decimal nullable value
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <param name="decimals">Number of decimals</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal? value, int decimals)
        {
            if (value.HasValue)
            {
                string pattern = @"""{0}"":{1:#0.";
                for (int x = 0; x < decimals; x++)
                {
                    pattern += "0";
                }

                pattern += "}";
                return string.Format(CultureInfo.GetCultureInfo("en-us"), pattern, key, value);
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":null", key);
        }

        /// <summary>
        /// Create a JSON pair key/value from a Double value
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Double value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, double value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":{1:#0.00}", key, value);
        }

        /// <summary>
        /// Create a JSON pair key/value from a string value
        /// </summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">String value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, DateTime? value)
        {
            if (value == null)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":null", key);
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"":{1:yyyyMMdd}", key, value);
        }

        /// <summary>
        /// Replace quote for literal quote
        /// </summary>
        /// <param name="text">Original text</param>
        /// <returns>Text with literal quotes</returns>
        public static string LiteralQuote(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace('\'', '´');
        }

        /// <summary>
        /// Create a JSON compliant text
        /// </summary>
        /// <param name="text">Original text</param>
        /// <returns>JSON compliant text</returns>
        public static string JsonCompliant(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = text.Replace("\n", "\\n").Replace("\\r", string.Empty);
            return text.Replace("\"", "\\\"");
        }

        /// <summary>
        /// Gets a limited length string
        /// </summary>
        /// <param name="text">Original string</param>
        /// <param name="length">Maximum length</param>
        /// <returns>A limited length string</returns>
        public static string LimitedText(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            return text.Substring(0, length);
        }

        /// <summary>
        /// Gets trasnlated month name
        /// </summary>
        /// <param name="monthNumber">Month number</param>
        /// <param name="dictionary">Dictionary for month names</param>
        /// <returns>Translated month name</returns>
        public static string TranslatedMonth(int monthNumber, Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return string.Empty;
            }

            string month = string.Empty;
            switch (monthNumber)
            {
                case 1:
                    month = dictionary["Common_MonthName_January"];
                    break;
                case 2:
                    month = dictionary["Common_MonthName_February"];
                    break;
                case 3:
                    month = dictionary["Common_MonthName_March"];
                    break;
                case 4:
                    month = dictionary["Common_MonthName_April"];
                    break;
                case 5:
                    month = dictionary["Common_MonthName_May"];
                    break;
                case 6:
                    month = dictionary["Common_MonthName_June"];
                    break;
                case 7:
                    month = dictionary["Common_MonthName_July"];
                    break;
                case 8:
                    month = dictionary["Common_MonthName_August"];
                    break;
                case 9:
                    month = dictionary["Common_MonthName_September"];
                    break;
                case 10:
                    month = dictionary["Common_MonthName_October"];
                    break;
                case 11:
                    month = dictionary["Common_MonthName_November"];
                    break;
                case 12:
                    month = dictionary["Common_MonthName_December"];
                    break;
                default:
                    month = string.Empty;
                    break;
            }

            return month;
        }

        /// <summary>
        /// Search the text on dictionary
        /// </summary>
        /// <param name="text">Dictionary key</param>
        /// <returns>Translated text by dictionary, or original text if not found</returns>
        public static string Translate(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            if (dictionary.ContainsKey(text))
            {
                return dictionary[text];
            }

            return text;
        }
    }
}