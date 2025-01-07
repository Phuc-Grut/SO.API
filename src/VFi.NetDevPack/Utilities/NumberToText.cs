using System.Numerics;
using Microsoft.AspNetCore.Components.Forms;

namespace VFi.NetDevPack.Utilities;

public static class Utilities
{
    public static string NumberToText(double? inputNumber)
    {
        if (inputNumber == null)
        {
            return "";
        }
        else
        {
            string[] unitNumbers = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] placeValues = new string[] { "", "nghìn", "triệu", "tỷ" };
            bool isNegative = false;

            // -12345678.3445435 => "-12345678"
            string sNumber = inputNumber.ToString();
            double number = Convert.ToDouble(sNumber);
            if (number < 0)
            {
                number = -number;
                sNumber = number.ToString();
                isNegative = true;
            }


            int ones, tens, hundreds;

            int positionDigit = sNumber.Length;   // last -> first

            string result = " ";


            if (positionDigit == 0)
                result = unitNumbers[0] + result;
            else
            {
                // 0:       ###
                // 1: nghìn ###,###
                // 2: triệu ###,###,###
                // 3: tỷ    ###,###,###,###
                int placeValue = 0;

                while (positionDigit > 0)
                {
                    // Check last 3 digits remain ### (hundreds tens ones)
                    tens = hundreds = -1;
                    ones = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                    positionDigit--;
                    if (positionDigit > 0)
                    {
                        tens = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                        positionDigit--;
                        if (positionDigit > 0)
                        {
                            hundreds = Convert.ToInt32(sNumber.Substring(positionDigit - 1, 1));
                            positionDigit--;
                        }
                    }

                    if ((ones > 0) || (tens > 0) || (hundreds > 0) || (placeValue == 3))
                        result = placeValues[placeValue] + result;

                    placeValue++;
                    if (placeValue > 3)
                        placeValue = 1;

                    if ((ones == 1) && (tens > 1))
                        result = "một " + result;
                    else
                    {
                        if ((ones == 5) && (tens > 0))
                            result = "lăm " + result;
                        else if (ones > 0)
                            result = unitNumbers[ones] + " " + result;
                    }
                    if (tens < 0)
                        break;
                    else
                    {
                        if ((tens == 0) && (ones > 0))
                            result = "lẻ " + result;
                        if (tens == 1)
                            result = "mười " + result;
                        if (tens > 1)
                            result = unitNumbers[tens] + " mươi " + result;
                    }
                    if (hundreds < 0)
                        break;
                    else
                    {
                        if ((hundreds > 0) || (tens > 0) || (ones > 0))
                            result = unitNumbers[hundreds] + " trăm " + result;
                    }
                    result = " " + result;
                }
            }

            //"Loại bỏ trường hợp x00
            result = result.Replace("không mươi không", "");

            //"Loại bỏ trường hợp 00x
            result = result.Replace("không mươi", "linh ");

            //"Loại bỏ trường hợp x0, x>=2
            result = result.Replace("mươi không", "mươi");

            //"Fix trường hợp 10
            result = result.Replace("một mươi", "mười");

            //"Fix trường hợp x4, x>=2
            //result = result.Replace("mươi bốn", "mươi tư");

            //"Fix trường hợp x04
            //result = result.Replace("linh bốn", "linh tư");

            //"Fix trường hợp x5, x>=2
            result = result.Replace("mươi năm", "mươi lăm");

            //"Fix trường hợp x1, x>=2
            result = result.Replace("mươi một", "mươi mốt");

            //"Fix trường hợp x15
            result = result.Replace("mười năm", "mười lăm");

            //"Bỏ ký tự space
            result = result.Trim();
            if (isNegative)
                result = "Âm " + result;

            if (result == null || result == "")
            {
                return null;
            }
            string f = result.Substring(0, 1).ToUpper();
            result = f + result.Substring(1);
            return result;
        }
    }

    private class Currency_CurrencyName
    {
        public string? Currency { get; set; }
        public string? CurrencyName { get; set; }
    }
    public static string NumberToText_Currency(double? number, string currency = "")
    {
        double? inputNumber = currency == "VND" ? Math.Round(number ?? 0, 0, MidpointRounding.AwayFromZero) : Math.Round(number ?? 0, 2, MidpointRounding.AwayFromZero);
        List<Currency_CurrencyName> currencyExchange = new List<Currency_CurrencyName>();
        currencyExchange.Add(new Currency_CurrencyName { Currency = "VND", CurrencyName = "đồng" });
        currencyExchange.Add(new Currency_CurrencyName { Currency = "USD", CurrencyName = "dollar Mỹ" });
        currencyExchange.Add(new Currency_CurrencyName { Currency = "GBP", CurrencyName = "bảng Anh" });
        currencyExchange.Add(new Currency_CurrencyName { Currency = "EUR", CurrencyName = "euro" });
        currencyExchange.Add(new Currency_CurrencyName { Currency = "CAD", CurrencyName = "dollar Canada" });
        currencyExchange.Add(new Currency_CurrencyName { Currency = "HKD", CurrencyName = "dollar Hồng Kông" });
        currencyExchange.Add(new Currency_CurrencyName { Currency = "AUD", CurrencyName = "dollar Úc" });
        currencyExchange.Add(new Currency_CurrencyName { Currency = "CNY", CurrencyName = "nhân dân tệ" });
        currencyExchange.Add(new Currency_CurrencyName { Currency = "SGD", CurrencyName = "dollar Singapore" });
        currencyExchange.Add(new Currency_CurrencyName { Currency = "JPY", CurrencyName = "yên" });
        if (inputNumber == null)
        {
            return "";
        }
        else
        {
            string sNumber = inputNumber.ToString();
            string sBeforeDot = "";
            string sAfterDot = "";
            if (sNumber.Contains("."))
            {
                sBeforeDot = sNumber.Substring(0, sNumber.IndexOf("."));
                string sa = sNumber.Substring(sNumber.IndexOf(".") + 1);
                sAfterDot = sa.Substring(0, sa.Length);
            }
            string result = "";
            if (Convert.ToDouble(sNumber.Contains(".") ? sBeforeDot : sNumber) > 0)
            {
                string s = NumberToText(Convert.ToDouble(sNumber.Contains(".") ? sBeforeDot : sNumber));
                result = result + s;
            }
            if (Convert.ToDouble(sAfterDot == "" ? 0 : sAfterDot) > 0)
            {
                string s = NumberToText(Convert.ToDouble(sAfterDot));
                string f = s.Substring(0, 1).ToLower();
                s = f + s.Substring(1);
                result = result + " phẩy " + s;
            }
            if (currency != "")
            {
                var currencyName = currencyExchange.Where(a => a.Currency == currency).FirstOrDefault();
                result = result + " " + (currencyName?.CurrencyName ?? currency);
            }
            return result;
        }
    }
    public static decimal? GetExchangeRate(string? calculation, decimal? exchangeRate)
    {
        return calculation == "/" ? (1 / (exchangeRate ?? 1)) : (exchangeRate ?? 1);
    }
}
