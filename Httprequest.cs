﻿using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//Ключи альфа - fb262dfb-8684-455f-881c-be6fa4590dff 

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
				catch (Exception ex)
				{
					Console.WriteLine("Ошибка " + ex);
				}
			}
		}

		public static async Task DaData(List<Organization> organizations)
		{
			Console.WriteLine("Начались запросы в dadata");
			using HttpClient client = new HttpClient();
			// Ваш токен API
			int i = 0;
			foreach (Organization organization in organizations)
			{
				try
				{
					string token = "";
					i++;
					if (i <= 10000)
					{token = "a40fb39d08893335e13fdcb13466823ac1bfbd24";}
					else { token = "763c9f749411e1447ce9c7f56a87fd503f24f3e5"; }
					// Создаем объект запроса
					var queryData = new 
					{
						query = "" + organization.INN.ToString() + @""
					};
					// Сериализуем объект в JSON
					var json = System.Text.Json.JsonSerializer.Serialize(queryData);
					// Настраиваем HttpClient
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

					// Отправляем запрос
					var response = await client.PostAsync("http://suggestions.dadata.ru/suggestions/api/4_1/rs/findById/party",
						new StringContent(json, Encoding.UTF8, "application/json"));

					// Проверяем успешность ответа
					if (response.IsSuccessStatusCode)
					{
						var responseData = await response.Content.ReadAsStringAsync();
						JObject data = JObject.Parse(responseData);

						// Проверяем, является ли 'management' объектом
						if (data["suggestions"]?[0]?["data"]?["management"] is JObject management)
						{
							organization.Name = management["name"]?.ToString() ?? "Информация о руководителе отсутствует";
						}
						else
						{
							// Если 'management' не объект, ищем информацию в 'fio'
							if (data["suggestions"]?[0]?["data"]?["fio"] is JObject fio)
							{
								organization.Name = fio["surname"]?.ToString() ?? "";
								organization.Name += " " + fio["name"]?.ToString() ?? "";
								organization.Name += " " + fio["patronymic"]?.ToString() ?? "";

								// Проверяем состояние
								if (data["suggestions"]?[0]?["data"]?["state"] is JObject state)
								{
									if (state["status"]?.ToString() != "ACTIVE")
									{
										organization.Dubl = "Да";
									}
								}
							}
							else
							{
								organization.Name = "Информация о руководителе отсутствует";
							}
						}
						if (organization.Name == "Информация о руководителе отсутствует")
						{
							organization.Dubl = "Да";
							Console.WriteLine("Ошибка: Информация о руководителе отсутствует");
							// Добавьте логику обработки отсутствия информации о руководителе
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Ошибка " + ex);
				}
			}
		} 
	}
}


