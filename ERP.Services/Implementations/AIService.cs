using System.Threading.Tasks;
using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Model.Response;
using ERP.Services.Interfaces;

namespace ERP.Services.Implementations;

public class AIService :IAIService
{
    
    private readonly IGeminiClient _geminiClient;

    public AIService(IGeminiClient geminiClient)
    {
        _geminiClient = geminiClient;
    }

    public async Task<GeminiMessageResponse> TextPrompt( string context , string question  ,string instruction )
    {
        string  prompt = $@"Contexto: {context}

                          Pregunta: {question}

                          Instrucción: {instruction}";
                         
        var response = await _geminiClient.TextPrompt(prompt);
       return response;
    }
}