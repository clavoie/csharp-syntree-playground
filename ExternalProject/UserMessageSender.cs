using Contracts;

namespace ExternalProject;

public class UserMessageSender
{
    private readonly SnsShim _sns = new SnsShim();

    public void Send(int userId)
    {
        int iphone = 33;
        _sns.Send(new UserContract
        {
            UserId = userId
        });
    }
}