namespace TalentoPlus.Web.Services.Interfaces
{
    public interface IGeminiService
    {
        Task<string> AskGeminiAsync(string question);
    }
}
