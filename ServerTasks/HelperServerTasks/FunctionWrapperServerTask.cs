using Task_Server_2.ServerTasks.ActivationConditions;

namespace Task_Server_2.ServerTasks;

public class FunctionWrapperServerTask : ServerTask
{
    /// <summary>
    /// The action to run when the task is activated.
    /// </summary>
    private readonly Delegate _action;

    /// <summary>
    /// The arguments to pass to the action.
    /// </summary>
    private readonly object[] _args;

    /// <summary>
    /// A class to wrap functions, so they can be called by the server task manager.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="activationCondition"></param>
    /// <param name="action">The function being called</param>
    /// <param name="args">
    /// Arguments for the delegate if there are any.
    /// AVOID USING THIS IF YOU CAN.
    /// If you need to use arguments, you might as well make a new ServerTask.
    /// </param>
    public FunctionWrapperServerTask(string name, IActivationCondition activationCondition, Action action,
        params object[] args)
        : base(name, activationCondition)
    {
        _action = action;

        // Copy the arguments to the field
        _args = new object[args.Length];
        args.CopyTo(_args, 0);
    }

    public FunctionWrapperServerTask(string name, IActivationCondition activationCondition, Delegate action,
        params object[] args)
        : base(name, activationCondition)
    {
        _action =  action;

        // Copy the arguments to the field
        _args = new object[args.Length];
        args.CopyTo(_args, 0);
    }

    protected override void TaskLogic(ServerTaskManager serverTaskManager, ServerTaskProject serverTaskProject)
    {
        // Invoke the action dynamically if it is a delegate
        if (_action is Action action)
            action.Invoke();

        // Otherwise, invoke the action with the arguments
        else
            _action.DynamicInvoke(_args);
    }
}