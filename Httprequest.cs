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

        public static async Task Request(Organization organization)
        {
            // Создаем HTTP-клиент
            HttpClient client = new HttpClient();

            // Устанавливаем заголовок запроса "Content-Type" и "API-key"
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("API-key", "fb262dfb-8684-455f-881c-be6fa4590dff");

            // Содержимое запроса в виде JSON
            string json = @"{
""organizationInfo"": {
    ""inn"": " + organization.INN + @"
},
""contactInfo"": [
    {
        ""phoneNumber"": " + organization.Number + @"
    }
],
""productInfo"": [
    {
        ""productCode"": ""LP_RKO""
    }
]}";

            // Создаем HTTP-запрос с содержимым в теле запроса
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://partner.alfabank.ru/public-api/v2/checks")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            // Отправляем HTTP-запрос и получаем ответ
            HttpResponseMessage response = await client.SendAsync(request);
            Console.WriteLine(response.ToString());
            Console.WriteLine(response.Content.ToString());
            // Проверяем статус ответа
            if (response.IsSuccessStatusCode)
            {
                // Получаем содержимое ответа
                string content = await response.Content.ReadAsStringAsync();

                // Десериализуем содержимое ответа в JSON-объект
                JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(content);

                // Выводим содержимое JSON-ответа
                Console.WriteLine(jsonResponse.ToString());
            }
            else
            {
                // Ошибка при отправке запроса
                Console.WriteLine("Ошибка: " + response.StatusCode);
            }
        }
    }
}


