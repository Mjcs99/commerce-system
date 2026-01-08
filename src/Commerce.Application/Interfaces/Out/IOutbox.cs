namespace Commerce.Application.Interfaces.Out;
public interface IOutbox
{
    void Enqueue(string type, string payload);
}