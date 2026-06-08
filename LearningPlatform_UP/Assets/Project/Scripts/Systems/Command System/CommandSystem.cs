using System.Collections.Generic;

public class CommandSystem : ICommandSystem
{
    private const string DEFAULT_QUEUE_TAG = "default";
    private readonly Dictionary<object, Queue<ICommand>> _commandsDict = new();
    public void Enqueue(ICommand command, object sequenceTag = null)
    {
        if(sequenceTag == null)
        {
            sequenceTag = DEFAULT_QUEUE_TAG;
        }
        bool isStarted = _commandsDict.ContainsKey(sequenceTag);
        if (!isStarted)
        {
            _commandsDict.Add(sequenceTag, new Queue<ICommand>());
        }
        var commandQueue = _commandsDict[sequenceTag];
        commandQueue.Enqueue(command);
        if(!isStarted)
        {
            RunCommands(sequenceTag);
        }
    }

    private void RunCommands(object sequenceTag)
    {
        var commandQueue = _commandsDict[sequenceTag];
        if(commandQueue.Count == 0)
        {
            _commandsDict.Remove(sequenceTag);
            return;
        }
        var command = commandQueue.Dequeue();
        command.Execute(onComplete: () =>
        {
            RunCommands(sequenceTag);
        });
    }
}
