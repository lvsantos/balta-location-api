using DocumentFormat.OpenXml.CustomProperties;

namespace Baltaio.Location.Api.Domain;

public sealed class City
{
    public City(int code, string name, State state)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        if (code <= 0)
            throw new ArgumentException("O código do IBGE deve ser maior que zero.", nameof(code));

        Code = code;
        Name = name;
        StateCode = state.Code;
        State = state;
    }
    private City() { }

    public int Code { get; init; }
    public string Name { get; private set; }
    public int StateCode { get; private set; }
    public State State { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTime? RemovedAt { get; private set; }

    internal void Remove()
    {
        if (IsRemoved)
            throw new InvalidOperationException("A cidade já foi removida.");

        IsRemoved = true;
        RemovedAt = DateTime.UtcNow;
    }
    internal void Restore()
    {
        IsRemoved = false;
        RemovedAt = null;
    }
    internal void Update(string newName, State newState)
    {
        EnsureDataIsValid();

        Name = newName;
        State = newState;
        StateCode = newState.Code;

        void EnsureDataIsValid()
        {
            if (string.IsNullOrEmpty(newName))
                throw new ArgumentException("O nome da cidade é obrigatório.", nameof(newName));
            ArgumentNullException.ThrowIfNull(newState, nameof(State));
        }
    }
}
