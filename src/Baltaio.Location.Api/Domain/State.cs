namespace Baltaio.Location.Api.Domain;

public sealed class State
{
    public State(int code, string abbreviation, string name)
    {
        Code = code;
        Abbreviation = abbreviation;
        Name = name;
    }
    private State() { }

    public int Code { get; set; }
    public string Abbreviation { get; set; }
    public string Name { get; set; }
}
