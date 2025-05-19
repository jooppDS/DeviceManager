namespace task2;

public class ConnectionException : Exception
{
    public ConnectionException() : base("Connection failed.") { }
}