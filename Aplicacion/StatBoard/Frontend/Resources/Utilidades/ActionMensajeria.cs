using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Frontend.Resources.Utilidades
{
    public class ActionMensajeria : ValueChangedMessage<ActionMessage>
    {
        public ActionMensajeria(ActionMessage value) : base(value)
        {
        }
    }
}
