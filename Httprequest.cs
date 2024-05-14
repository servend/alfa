using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace alfa
{
    internal class Httprequest
    {

        public static async Task Request(List<Organization> organizations)
        {
            foreach (Organization organization in organizations)
            {
                try
                {
                    // Создаем HTTP-клиент
                    using HttpClient client = new HttpClient();

                    // Устанавливаем заголовок "API-key"
                    client.DefaultRequestHeaders.Add("API-key", "fb262dfb-8684-455f-881c-be6fa4590dff");

                    // Содержимое запроса в виде JSON
                    string json = null;

                    // Проверяем, что организация не равна null
                    if (organization != null)
                    {
                        json = @"{
""organizationInfo"": {
    ""inn"": """ + organization.INN.ToString() + @"""
},
""contactInfo"": [
    {
        ""phoneNumber"": """ + organization.Number.ToString() + @"""
    }
],
""productInfo"": [
    {
        ""productCode"": ""LP_RKO""
    }
]}";
                    }

                    // Создаем HTTP-запрос с содержимым в теле запроса
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://partner.alfabank.ru/public-api/v2/checks")
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };

                    // Отправляем HTTP-запрос и получаем ответ
                    HttpResponseMessage response = await client.SendAsync(request);
                    Console.WriteLine($"{request.Method} {request.RequestUri}");

                    // Выводим заголовки запроса
                    Console.WriteLine("Headers:");
                    foreach (var header in request.Headers)
                    {
                        Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }

                    // Выводим содержимое тела запроса (JSON)
                    Console.WriteLine("Body:");
                    Console.WriteLine(json);

                    if (response.IsSuccessStatusCode)
                    {
                        // Получаем содержимое ответа
                        string content = await response.Content.ReadAsStringAsync();

                        // Десериализуем содержимое ответа в JSON-объект
                        JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(content);

                        // Проверяем, что jsonResponse не равен null





                        if (jsonResponse != null)
                        {
                            organization.Dubl = "Да";
                            // Выводим содержимое JSON-ответа
                            Console.WriteLine(jsonResponse.ToString());
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // В случае ошибки 400 (Bad Request) выводим подробности
                        string errorDetails = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Ошибка: {response.StatusCode} {errorDetails}");
                        organization.Dubl = "Да";
                    }
                    else
                    {
                        // Ошибка при отправке запроса
                        Console.WriteLine("Ошибка: " + response.StatusCode); organization.Dubl = "Да";
                    }
                }
                catch (Exception
                
                )
                {
                    Console.WriteLine("Ошибка");
                }
            }
        }
    }
}


