using Contracts;

namespace ExternalProject;

public class SnsShim
{
    public void Send(UserContract userContract)
    {
        Console.WriteLine(userContract);
    }
}