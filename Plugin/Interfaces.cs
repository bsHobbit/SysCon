namespace Plugin
{
    public enum AccessModifier
    {
        Public,
        Private,
    }

    public interface IAccessible
    {
        AccessModifier AccessModifier { get; set; }
    }

    
}
