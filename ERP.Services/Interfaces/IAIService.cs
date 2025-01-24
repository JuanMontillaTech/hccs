using System.Threading.Tasks;
using DotnetGeminiSDK.Model.Response;

namespace ERP.Services.Interfaces;

public interface IAIService
{
    public Task<GeminiMessageResponse> TextPrompt(string context, string question, string instruction);
}