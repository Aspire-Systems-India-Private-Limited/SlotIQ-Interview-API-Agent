namespace SlotIQ.Interview.Logic.Commands;

public interface ICommandHandler<TCommand, TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}
