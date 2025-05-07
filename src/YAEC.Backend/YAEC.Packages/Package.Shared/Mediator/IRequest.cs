namespace Package.Shared.Mediator;

public interface IRequest;

public interface IRequest<out TResponse> : IRequest;