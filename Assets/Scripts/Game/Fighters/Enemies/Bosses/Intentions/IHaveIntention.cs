using System;

public interface IHaveIntention : IHaveHUD
{
    public event Action<Intention, string> OnIntentionDetermined;
}