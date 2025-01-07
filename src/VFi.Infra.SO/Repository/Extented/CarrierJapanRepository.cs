using System.Text;
using Flurl.Http;
using Flurl.Http.Configuration;
using VFi.Domain.SO.Interfaces.Extented;

namespace VFi.Infra.SO.Repository.Extented;

public class CarrierJapanRepository : ICarrierJapanRepository
{
    private readonly IFlurlClientFactory _flurlClientFac;

    private const string PATH_YAMATO = "https://toi.kuronekoyamato.co.jp/cgi-bin/tneko";
    private const string PATH_SAGAWA = "https://k2k.sagawa-exp.co.jp/p/sagawa/web/okurijoinput.jsp";
    private const string PATH_JAPANPOST = "https://trackings.post.japanpost.jp/services/srv/search?requestNo1={0}&search.x=102&search.y=15&locale=ja";
    public CarrierJapanRepository(IFlurlClientFactory flurlClientFac)
    {
        _flurlClientFac = flurlClientFac;
    }

    public async Task<string> GetYamato(string tracking)
    {
        var _flurlClient = _flurlClientFac.Get("https://toi.kuronekoyamato.co.jp");

        var result = await _flurlClient.Request(PATH_YAMATO)
            .WithHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7")
            .WithHeader("Accept-Language", "en,vi;q=0.9,en-US;q=0.8,ja;q=0.7")
            .WithHeader("Cache-Control", "max-age=0")
            .WithHeader("Connection", "keep-alive")
            .WithHeader("Origin", "https://toi.kuronekoyamato.co.jp")
            .WithHeader("Referer", "https://toi.kuronekoyamato.co.jp/cgi-bin/tneko")
            .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36")
            .PostUrlEncodedAsync(new
            {
                backrequest = "get",
                number01 = tracking
            })
            .ReceiveString();
        return result;
    }

    public async Task<string> GetJapanPost(string tracking)
    {
        var _flurlClient = _flurlClientFac.Get("https://trackings.post.japanpost.jp");

        var result = await _flurlClient.Request(string.Format(PATH_JAPANPOST, tracking))
            .WithHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7")
            .WithHeader("Accept-Language", "en,vi;q=0.9,en-US;q=0.8,ja;q=0.7")
            .WithHeader("Cache-Control", "max-age=0")
            .WithHeader("Connection", "keep-alive")
            .WithHeader("Cookie", "JSESSIONID=tejhSXLiUphOZBoXZ-le0cnT0nIAU_El0g3BHMGzm1Ddia41ls4_!-2129221750; TS01b09507=01676585ca77b9e3bfe4ccba63db7014bac91ed7824917c8cd39d34f99a6149f2efd5de71c6c801ae8a88ea1261f2bf86422de2904eaf5f8630035e6ae0dfd45f2ca078ad5; ac=17162124372670; _ga=GA1.2.1905920297.1720446522; _ga_8H7BG0TWLR=GS1.1.1720451739.2.0.1720451739.0.0.0; BIGipServerRD0545_Pool_172.19.16.150_80=rd545o00000000000000000000ffffac130245o7005; BIGipServerRD0545_Pool_172.19.16.148_443=rd545o00000000000000000000ffffac130243o8081; TS01884c41=01676585ca4d0a1b7cff033f5409ac4a976ce2c6134917c8cd39d34f99a6149f2efd5de71c8fedee7b333a38fb8f33c4c20bbc816630a4face576175fe11b3baa371a99baddba18bed0ec795d3f9b54ddea4c76689")
            .WithHeader("Referer", "https://trackings.post.japanpost.jp/services/srv/search/input")
            .WithHeader("Sec-Fetch-Dest", "document")
            .WithHeader("Sec-Fetch-Mode", "navigate")
            .WithHeader("Sec-Fetch-Site", "same-origin")
            .WithHeader("Sec-Fetch-User", "?1")
            .WithHeader("Upgrade-Insecure-Requests", "1")
            .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36")
            .WithHeader("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Google Chrome\";v=\"126\"")
            .WithHeader("sec-ch-ua-mobile", "?0")
            .WithHeader("sec-ch-ua-platform", "\"Windows\"")
            .GetStringAsync();
        return result;
    }

    public async Task<string> GetSagawa(string tracking)
    {
        var _flurlClient = _flurlClientFac.Get("https://k2k.sagawa-exp.co.jp");

        var requestObject = new object();


        var collection = new List<KeyValuePair<string, string>>();
        collection.Add(new("jsf_tree_64", "H4sIAAAAAAAAAK1VzU4UQRDuIAQVE6MeuJpoYjRmdnd++AkXgQRZAiERMCF7mDQzzTJkprvtqYHl4sV49wF8AsMT+ATevPoSXj3b0zvs7PaUQqKH7Wx/+bqqvvpqui9/kplckddC9R0qaXTCnOzimEYs11uZJhGFRHBnXzG2B6qIoFBsh3LaZ+ppDa6LTArOOGwczv/4vPXr+xSZ7pE7YXSSpLHSONnqbesUrWGKVpWiNZaiddMUK9vkfhhd7dZTmudAHm6f0jPaSinvtzQ/4X1Nu1fTuvE78p5M9chsWGYGfeRRb+zM7tEpi2BlIAtVlfo/urHy9fLZtwcfP32ZImQgCSFzeVkGKcp1pt5LIM/LWgbOMNeobOcEstTZ1MtuAbKAfTYAINNhEndkdbRc7w4xF8E8BPMRLECwBQRbRLAlBFu2sJmy5jYGdrT61VOpRTs57dNzGrKBdI6j4og5URpClDj5RQ4sG2tLkdT/D7qbjMbDrGiCq7aMuu5ZJEggZdhJu3kGHHUPiIvMieWdNsz4t8NyrY7lVZCgDvLq79rL35+Er1HOmapCTrjVrkDbLgPafhkQM8zF+unao2dAe/YMiPXPtafPgPb4GdCePwNiilxMkYsp8jBFHqbIwxR5tqLZTPC+4I0AXHTCTu2xd/NB6fLRd35LR8GKmOjgoeGhxQZNHuaIt9DkYSZ5i00e5pu31ORhVnrLTR7mrt9u8jDD/U6Th82A71q8ad3ndm3Wi2uuYuPQZhLH5bs2FwmlWGrehTrEy2tC6BciozxeKwAEr6qasOYDkNsgkj2gCiwJc1KdRRc7Zu4wdZh1PuaTj5niL+ryn1xT/oZQmW5bRhO0gglzqpvIX0bAoI2BHQzEBjzApjkYu5/X/ulZWU313VoFDXS0x3hXDrpvE3b+RgiQ8jceJHLsTgkAAA=="));
        collection.Add(new("jsf_state_64", "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"));
        collection.Add(new("jsf_viewid", "/web/okurijoinput.jsp"));
        collection.Add(new("main:correlation", "1"));
        collection.Add(new("main_SUBMIT", "1"));
        collection.Add(new("main:no1", tracking));
        collection.Add(new("main:no2", ""));
        collection.Add(new("main:no3", ""));
        collection.Add(new("main:no4", ""));
        collection.Add(new("main:no5", ""));
        collection.Add(new("main:no6", ""));
        collection.Add(new("main:no7", ""));
        collection.Add(new("main:no8", ""));
        collection.Add(new("main:no9", ""));
        collection.Add(new("main:no10", ""));
        collection.Add(new("main:toiStart", "&#12362;&#21839;&#12356;&#21512;&#12431;&#12379;&#38283;&#22987;"));
        //var content = new FormUrlEncodedContent(collection);


        var result = await _flurlClient.Request(PATH_SAGAWA)
            .WithHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7")
            .WithHeader("accept-language", "en,vi;q=0.9,en-US;q=0.8,ja;q=0.7")
            .WithHeader("cache-control", "max-age=0")
            .WithHeader("content-type", "application/x-www-form-urlencoded")
            .WithHeader("origin", "https://k2k.sagawa-exp.co.jp")
            .WithHeader("priority", "u=0, i")
            .WithHeader("referer", "https://k2k.sagawa-exp.co.jp/p/sagawa/web/okurijoinput.jsp")
            .WithHeader("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Google Chrome\";v=\"126\"")
            .WithHeader("sec-ch-ua-mobile", "?0")
            .WithHeader("sec-ch-ua-platform", "\"Windows\"")
            .WithHeader("sec-fetch-dest", "document")
            .WithHeader("sec-fetch-mode", "navigate")
            .WithHeader("sec-fetch-site", "same-origin")
            .WithHeader("sec-fetch-user", "?1")
            .WithHeader("Accept-Charset", "utf-8")
            .WithHeader("upgrade-insecure-requests", "1")
            .WithHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36")
            .WithHeader("Cookie", "JSESSIONID=D5BB46BB0F1741FA0CDAD163653A502A; NSC_l2l-ttm-wt-001=ffffffff0934cd6045525d5f4f58455e445a4a423660")
        .PostUrlEncodedAsync(collection);

        var contentBytes = await result.GetBytesAsync();

        // Decode the byte array using Windows-31J encoding
        var htmlContent = Encoding.GetEncoding("shift_jis").GetString(contentBytes);

        return htmlContent;
    }
}
