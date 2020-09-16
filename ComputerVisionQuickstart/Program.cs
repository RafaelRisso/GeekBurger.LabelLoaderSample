using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ComputerVisionQuickstart
{
    class Program
    {
        public static string[] blacklist = new string[]
               { "ingredients", "processed in a facility that handles", "products" , "allergens" , "contains" };
        public const string AndWithSpace = " and ";
        public const string CommaWithSpace = " , ";
        static string subscriptionKey = "df73d9d6a2b94ef683730650309ab2bc";
        static string endpoint = "https://labelloader.cognitiveservices.azure.com/";

        static string uriBase = endpoint + "vision/v2.1/ocr";

        static async Task Main()
        {

            Console.WriteLine("Reconhecimento de Rotulos:");
            Console.Write("Insira o caminho completo da imagem e de enter");
            string imageFilePath = Console.ReadLine();

            if (File.Exists(imageFilePath))
            {

                Console.WriteLine("\n Aguarde os resultados.\n");
                await MakeOCRRequest(imageFilePath);
            }
            else
            {
                Console.WriteLine("\nCaminho inválido");
            }
            Console.WriteLine("\nPressione ENTER para sair...");
            Console.ReadLine();
        }


        static async Task MakeOCRRequest(string imageFilePath)
        {
            try
            {
                HttpClient client = new HttpClient();


                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                string requestParameters = "language=unk&detectOrientation=true";
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;


                byte[] byteData = GetImageAsByteArray(imageFilePath);


                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {

                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");


                    response = await client.PostAsync(uri, content);
                }


                string contentString = await response.Content.ReadAsStringAsync();


                Console.WriteLine("\nResposta:\n\n{0}\n",
                    JToken.Parse(contentString).ToString());
                var results = JToken.Parse(contentString).ToString();


            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }

            static byte[] GetImageAsByteArray(string imageFilePath)
            {

                using (FileStream fileStream =
                    new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    return binaryReader.ReadBytes((int)fileStream.Length);
                }
            } }


    } }
