using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Frontend.Resources.Utilidades
{
    public class PlayerMensajeria : ValueChangedMessage<PlayerMessage>
    {
        public PlayerMensajeria(PlayerMessage value) : base(value)
        {
        }
    }
}
