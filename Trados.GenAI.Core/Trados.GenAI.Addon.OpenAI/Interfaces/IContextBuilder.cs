namespace Trados.GenAI.Addon.OpenAI.Interfaces
{
    public interface IContextBuilder<TInput, TOutput>
    {
        TOutput Build(TInput input);
    }
}
