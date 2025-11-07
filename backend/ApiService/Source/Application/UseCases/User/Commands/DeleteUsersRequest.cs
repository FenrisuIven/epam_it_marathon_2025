using CSharpFunctionalExtensions;
using FluentValidation.Results;
using MediatR;
using RoomAggregate = Epam.ItMarathon.ApiService.Domain.Aggregate.Room.Room;

namespace Epam.ItMarathon.ApiService.Application.UseCases.User.Commands
{
    public record DeleteUserRequest(string UserCode, ulong? UserId)
        : IRequest<Result<RoomAggregate, ValidationResult>>;
}