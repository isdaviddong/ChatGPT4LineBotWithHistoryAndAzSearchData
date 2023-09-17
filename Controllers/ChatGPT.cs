using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace isRock.Template
{
    public class ChatGPT
    {
        const string AzureOpenAIEndpoint = "https://______.openai.azure.com";  //👉replace it with your Azure OpenAI Endpoint
        const string AzureOpenAIModelName = "___gpt432k___"; //👉repleace it with your Azure OpenAI Model Name
        const string AzureOpenAIToken = "______AzureOpenAIToken______"; //👉repleace it with your Azure OpenAI Token
        const string AzureOpenAIVersion = "2023-08-01-preview";  //👉replace  it with your Azure OpenAI Model Version
        const string azSearchIndexName = "___azSearchIndexName___";  //👉replace  it with your Azure Search Index Name
        const string azSearchEndpoint = "https://___________.search.windows.net";   //👉replace  it with your Azure Search Endpoint
        const string azSearchKey = "_____azSearchKey_______";  //👉replace  it with your Azure Search Key

        public static string CallAzureOpenAIChatAPI(
            string endpoint, string modelName, string apiKey, string apiVersion, object requestData)
        {
            var client = new HttpClient();

            // 設定 API 網址
            var apiUrl = $"{endpoint}/openai/deployments/{modelName}/extensions/chat/completions?api-version={apiVersion}";

            // 設定 HTTP request headers
            client.DefaultRequestHeaders.Add("api-key", apiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT heade
            // 將 requestData 物件序列化成 JSON 字串
            string jsonRequestData = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            // 建立 HTTP request 內容
            var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");
            // 傳送 HTTP POST request
            var response = client.PostAsync(apiUrl, content).Result;
            // 取得 HTTP response 內容
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseContent);
            return obj.choices[0].message.content.Value;
        }


        public static string getResponseFromGPT(string Message, List<Message> chatHistory)
        {
            //建立對話紀錄
            var messages = new List<ChatMessage>
                    {
                        new ChatMessage {
                            role = Role.system ,
                            content = @"
                                你是一個AI助理，請參考上傳的pdf文件，該文件是開課資訊。請回答用戶的提問。
                                ----------------------
"
                        }
                    };

            //添加歷史對話紀錄
            foreach (var HistoryMessageItem in chatHistory)
            {
                //添加一組對話紀錄
                messages.Add(new ChatMessage()
                {
                    role = Role.user,
                    content = HistoryMessageItem.UserMessage
                });
                messages.Add(new ChatMessage()
                {
                    role = Role.assistant,
                    content = HistoryMessageItem.ResponseMessage
                });
            }
            messages.Add(new ChatMessage()
            {
                role = Role.user,
                content = Message
            });
            //回傳呼叫結果
            return ChatGPT.CallAzureOpenAIChatAPI(
               AzureOpenAIEndpoint, AzureOpenAIModelName, AzureOpenAIToken, AzureOpenAIVersion,
                new
                {
                    model = "gpt-3.5-turbo",
                    dataSources = new List<DataSource>
                    {
                        new DataSource
                        {
                            type = "AzureCognitiveSearch",
                            parameters = new Parameters
                            {
                                endpoint = azSearchEndpoint,
                                key = azSearchKey,
                                indexName = azSearchIndexName
                            }
                        }
                    },
                    messages = messages
                }
             );
        }
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum Role
    {
        assistant, user, system
    }

    public class ChatMessage
    {
        public Role role { get; set; }
        public string content { get; set; }
    }

    public class DataSource
    {
        public string type { get; set; }
        public Parameters parameters { get; set; }
    }

    public class Parameters
    {
        public string endpoint { get; set; }
        public string key { get; set; }
        public string indexName { get; set; }
    }

}
