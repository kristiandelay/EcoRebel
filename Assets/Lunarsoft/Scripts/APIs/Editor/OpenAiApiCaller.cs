using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;
using Lunarsoft;
using System.IO;

public class OpenAiApiCaller : BaseApi
{
    public class Model
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string ObjectType { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("owned_by")]
        public string OwnedBy { get; set; }

        [JsonProperty("permission")]
        public List<Permission> Permission { get; set; }

        [JsonProperty("root")]
        public string Root { get; set; }

        [JsonProperty("parent")]
        public string Parent { get; set; }
    }

    public class Permission
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string ObjectType { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("allow_create_engine")]
        public bool AllowCreateEngine { get; set; }

        [JsonProperty("allow_sampling")]
        public bool AllowSampling { get; set; }

        [JsonProperty("allow_logprobs")]
        public bool AllowLogprobs { get; set; }

        [JsonProperty("allow_search_indices")]
        public bool AllowSearchIndices { get; set; }

        [JsonProperty("allow_view")]
        public bool AllowView { get; set; }

        [JsonProperty("allow_fine_tuning")]
        public bool AllowFineTuning { get; set; }

        [JsonProperty("organization")]
        public string Organization { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("is_blocking")]
        public bool IsBlocking { get; set; }
    }

    public class ModelResponse
    {
        [JsonProperty("object")]
        public string ObjectType { get; set; }

        [JsonProperty("data")]
        public List<Model> Data { get; set; }
    }

    public class ChatCompletionResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("usage")]
        public Usage Usage { get; set; }

        [JsonProperty("choices")]
        public List<Choice> Choices { get; set; }
    }

    public class Usage
    {
        [JsonProperty("prompt_tokens")]
        public int PromptTokens { get; set; }

        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }
    }

    public class Choice
    {
        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("finish_reason")]
        public string FinishReason { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }
    }

    public class Message
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    virtual public void Start()
    {
        baseUrl = "https://api.openai.com";

        //StartCoroutine(CallModelsApi());
        //StartCoroutine(CallModelDetailsApi("gpt-3.5-turbo-0301"));

        //StartCoroutine(CallOpenAiApi("Create a new 2d base class for a side scroller beatem up style game like streets of rage."));
        //StartCoroutine(CallDavinciApi());
    }

    protected List<ChatCompletionResponse> LoadChatHistoryFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<ChatCompletionResponse>();
        }

        string fileContent = File.ReadAllText(filePath);
        JArray chatHistory = JArray.Parse(fileContent);
        return chatHistory.ToObject<List<ChatCompletionResponse>>();
    }

    protected void SaveChatCompletionResponseToFile(ChatCompletionResponse chatCompletionResponse, string filePath)
    {
        JArray chatHistory;
        if (File.Exists(filePath))
        {
            string fileContent = File.ReadAllText(filePath);
            chatHistory = JArray.Parse(fileContent);
        }
        else
        {
            chatHistory = new JArray();
        }

        string jsonString = JsonConvert.SerializeObject(chatCompletionResponse);
        chatHistory.Add(JObject.Parse(jsonString));
        File.WriteAllText(filePath, chatHistory.ToString(Formatting.Indented));
    }

    [SerializeField]
    public virtual List<string> Requirements { get; } = new List<string>()
        {
            "The script is a component of a GameObject.",
            "Include all the necessary imports.",
            "Add a RequireComponent attribute to any components used.",
            "Do not require any other custom scripts.",
            "Generate tooltips for all properties.",
            "All properties should have default values.",
            "Add explanatory comments to the script.",
            "I only need the script body. Don’t add any explanation.",
            "Add namespace Lunarsoft around all classes."
    };

    protected string requirementsString;

    private void OnValidate()
    {
        requirementsString = string.Join("\n - ", Requirements);
    }


    protected JObject BuildMessagePromptType(string role = "System", string content = "You are a helpful unity c# coding bot assistant.")
    {
        return new JObject {    
            {"role", role },
            {"content", content},
        };
    }

    private IEnumerator CallOpenAiApi(string newQuestion = "Where was it played?")
    {
        // Load chat history from file
        string filePath = Path.Combine(Application.dataPath, "ChatHistory.json");
        List<ChatCompletionResponse> chatHistory = LoadChatHistoryFromFile(filePath);

        string endpoint = "/v1/chat/completions";
        string requirementsString = string.Join("\n - ", Requirements);


        JObject body = new JObject
        {
            { "model", "gpt-3.5-turbo" },
            { "messages", new JArray {

                // TODO: Load Previous prompts and answers here
                BuildMessagePromptType("system", "You are a helpful unity bot assistant."),
                BuildMessagePromptType("user", " Write code of a C# Unity script for a given task. " +
                "Some requirements for the script: " + " - " + requirementsString +
                " The task is described as follows: " + newQuestion),
            }
        },
        { "temperature", 1 },
        { "top_p", 1 },
        { "n", 1 },
        { "stream", false },
        { "max_tokens", 3500 },
        { "presence_penalty", 0 },
        { "frequency_penalty", 0 }
        };


        yield return Post(endpoint, body,
            onSuccess: response =>
            {
                Debug.Log("API response: " + response);

                ChatCompletionResponse chatCompletionResponse = JsonConvert.DeserializeObject<ChatCompletionResponse>(response);
                string assistantMessage = chatCompletionResponse.Choices[0].Message.Content;
                Debug.Log("assistantMessage: " + assistantMessage);

                if (!FixCode(ref assistantMessage))
                {
                    var message = "ChatGPT did not generate valid code. " +
                                    "Please try again with a different prompt or a different temperature.";

                    EditorUtility.DisplayDialog("ChatGPT", message, "OK");
                    return;
                }
                Debug.Log("Code: " + assistantMessage);

                SaveChatCompletionResponseToFile(chatCompletionResponse, filePath);
            },
            onError: error =>
            {
                Debug.LogError("API error: " + error);
                ApiErrorHandler.HandleError(error);

            }
        );
    }

    private IEnumerator CallDavinciApi(string prompt = "create a baseCharacter class in unity c# wrap all classes in namespace Lunarsoft")
    {
        string endpoint = "/v1/completions";
        JObject body = new JObject
    {
        { "model", "text-davinci-003" },
        { "prompt", prompt },
        { "max_tokens", 2500 },
        { "temperature", 0.7 }
    };

        yield return Post(endpoint, body,
            onSuccess: response =>
            {
                Debug.Log("API response: " + response);
            },
            onError: error =>
            {
                Debug.LogError("API error: " + error);
                ApiErrorHandler.HandleError(error);

            }
        );
    }

    private IEnumerator CallModelsApi()
    {
        string endpoint = "/v1/models";

        yield return Get(endpoint,
            onSuccess: response =>
            {
                Debug.Log("API response: " + response);

                ModelResponse modelResponse = JsonConvert.DeserializeObject<ModelResponse>(response);
                foreach (var model in modelResponse.Data)
                {
                    Debug.Log("Model ID: " + model.Id);
                    Debug.Log("Model Created: " + model.Created);
                    Debug.Log("Model Owned By: " + model.OwnedBy);
                    // Access other properties if needed
                }
            },
            onError: error =>
            {
                Debug.LogError("API error: " + error);
                ApiErrorHandler.HandleError(error);

            }
        );
    }

    private IEnumerator CallModelDetailsApi(string modelName)
    {
        string endpoint = $"/v1/models/{modelName}";

        yield return Get(endpoint,
            onSuccess: response =>
            {
                Debug.Log("API response: " + response);

                Model model = JsonConvert.DeserializeObject<Model>(response);
                Debug.Log("Model ID: " + model.Id);
                Debug.Log("Model Created: " + model.Created);
                Debug.Log("Model Owned By: " + model.OwnedBy);
                // Access other properties if needed
            },
            onError: error =>
            {
                Debug.LogError("API error: " + error);
                ApiErrorHandler.HandleError(error);

            }
        );
    }

    private bool FixCode(ref string code)
    {
        code = code.Trim();
        code = code.Trim('\n');
        code = code.Trim('`');
        code = code.Trim('\n');

        var validCodeStart = code.StartsWith("using") || code.StartsWith("namespace") || code.StartsWith("public") ||
                             code.StartsWith("private") || code.StartsWith("protected") ||
                             code.StartsWith("internal") || code.StartsWith("#");
        var validCodeEnd = code.EndsWith("}");

        if (!validCodeStart || !validCodeEnd)
        {
            return false;
        }

        code += "\n";
        return true;
    }

}
    