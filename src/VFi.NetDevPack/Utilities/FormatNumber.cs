using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;
using Microsoft.AspNetCore.Components.Forms;

namespace VFi.NetDevPack.Utilities;

public static class FormatNumber
{
    public static string AddPeriod(decimal? number, int? numberAferDot = 0)
    {
        if (number == null || number == 0)
        {
            return "0";
        }
        if (((0 < number) && (number < 1)) || ((-1 < number) && (number < 0)))
        {
            return Math.Round(number ?? 0, numberAferDot ?? 0).ToString(CultureInfo.GetCultureInfo("de-DE"));
        }
        string result = "";
        switch (numberAferDot)
        {
            case 0:
                result = String.Format("{0:#,###}", number);
                break;
            case 1:
                result = String.Format("{0:#,###.#}", number);
                break;
            case 2:
                result = String.Format("{0:#,###.##}", number);
                break;
            case 3:
                result = String.Format("{0:#,###.###}", number);
                break;
            case 4:
                result = String.Format("{0:#,###.####}", number);
                break;
            case 5:
                result = String.Format("{0:#,###.#####}", number);
                break;
            default:
                result = String.Format("{0:#,###}", number);
                break;
        }
        return result.Replace(".", "#").Replace(",", ".").Replace("#", ",");

    }
    public static string AddComma(decimal? number, int? numberAferDot = 0)
    {
        if (number == null)
        {
            return "";
        }
        string result = "";
        switch (numberAferDot)
        {
            case 0:
                result = String.Format("{0:#,###}", number);
                break;
            case 1:
                result = String.Format("{0:#,###.#}", number);
                break;
            case 2:
                result = String.Format("{0:#,###.##}", number);
                break;
            case 3:
                result = String.Format("{0:#,###.###}", number);
                break;
            case 4:
                result = String.Format("{0:#,###.####}", number);
                break;
            case 5:
                result = String.Format("{0:#,###.#####}", number);
                break;
            default:
                result = String.Format("{0:#,###}", number);
                break;
        }
        return result;
    }
}
