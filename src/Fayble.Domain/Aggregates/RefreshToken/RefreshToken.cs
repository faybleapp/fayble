using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.RefreshToken;

public class RefreshToken : IdentifiableEntity<Guid>, IAggregateRoot
{
    public string Token { get; private set; }

    public DateTimeOffset Expiration { get; private set; }

    public Guid UserId { get; private set; }

    public virtual User.User User { get; private set; }

    public bool Revoked { get; private set; }

    public DateTimeOffset Created { get; private set; }


    private RefreshToken()
    {
    }

    public RefreshToken(string token, DateTimeOffset expiration, Guid userId, DateTimeOffset? created)
    {
        Token = token;
        Expiration = expiration;
        UserId = userId;
        Created = created ?? DateTimeOffset.Now;
    }

    public void Revoke()
    {
        Revoked = true;
    }
}
