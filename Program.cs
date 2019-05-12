using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;




namespace EbayScraperConsoleAlpha
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHtmlAsync();

            
            Console.ReadLine();
        }

        private static async void GetHtmlAsync()
        {
            var url = "https://www.ebay.co.uk/sch/i.html?_nkw=xbox+one&_in_kw=1&_ex_kw=&_sacat=0&_udlo=&_udhi=&_ftrt=901&_ftrv=1&_sabdlo=&_sabdhi=&_samilow=&_samihi=&_sadis=15&_stpos=CF434TP&_sargn=-1%26saslc%3D1&_salic=3&_sop=12&_dmd=1&_ipg=200";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var ProductsHtml = htmlDocument.DocumentNode.Descendants("ul")
                .Where(node => node.GetAttributeValue("id","")
                .Equals("ListViewInner")).ToList(); //displays all nodes with id in the list

            var ProductListItems = ProductsHtml[0].Descendants("li")
                .Where(node => node.GetAttributeValue("id", "")
                .Contains("item")).ToList();//thins down the information and isolates each id by item

            foreach (var ProductListItem in ProductListItems)
            {
                //id
                Console.WriteLine(ProductListItem.GetAttributeValue("listingid", ""));
                

                //Product name
                Console.WriteLine(ProductListItem.Descendants("h3")
                    .Where(HtmlNode => HtmlNode.GetAttributeValue("class", "")
                    .Equals("lvtitle")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')  //gets the inner text of the element
                    );
                

                //Price
                Console.WriteLine(
                    Regex.Match(
                        ProductListItem.Descendants("li")
                        .Where(HtmlNode => HtmlNode.GetAttributeValue("class", "")
                        .Equals("lvprice prc")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t') //trims return, newline and tab
                       ,@"\d+.\d+") //for some reason cant get this to work regex.match
                        );

                //Listing Type lvformat
                Console.WriteLine(
                    ProductListItem.Descendants("li")
                    .Where(HtmlNode => HtmlNode.GetAttributeValue("class", "")
                    .Equals("lvformat")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                    );


                //url
                Console.WriteLine(
                ProductListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", "").Trim('\r', '\n', '\t'));
                Console.WriteLine();
                
            }

            

          
            
        }
    }
}


//info gained from https://www.youtube.com/watch?v=BE708X6r24o&t=752s