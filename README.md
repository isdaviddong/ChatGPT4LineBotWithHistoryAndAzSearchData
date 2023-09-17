# ChatGPT Line Bot With Chat History
LineBot WebHook supporting context (using ChatGPT4 - Azure OpenAI API)

### Usage
The main LINE Bot WebHook is http://yourDomain/api/LineBotChatGPTWebHook

#### Please replace the variable data below, before you can use:
A> AzureOpenAIEndpoint, AzureOpenAIModelName, AzureOpenAIToken, AzureOpenAIVersion in the ChatGPT.cs file
B> ChannelAccessToken, AdminUserId in LineBotChatGPTWebHookController.cs
C> azSearchIndexName, azSearchEndpoint, azSearchKey in the ChatGPT.cs file

# ChatGPT Line Bot With Chat History
支援前後文與YourOwnData(Azure Search)的 LineBot WebHook (using ChatGPT4 - Azure OpenAI API)

### Usage
主要 LINE Bot WebHook 為 http://yourDomain/api/LineBotChatGPTWebHook

#### 請替換底下的變數資料後，即可使用:
A> ChatGPT.cs 檔案中的 AzureOpenAIEndpoint, AzureOpenAIModelName, AzureOpenAIToken, AzureOpenAIVersion    
B> LineBotChatGPTWebHookController.cs 中的 ChannelAccessToken, AdminUserId   
C> ChatGPT.cs 檔案中的 azSearchIndexName, azSearchEndpoint, azSearchKey 
