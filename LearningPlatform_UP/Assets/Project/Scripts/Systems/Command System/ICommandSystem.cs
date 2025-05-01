public interface ICommandSystem
{
    public void Enqueue(ICommand command, object sequenceTag = null);
}
