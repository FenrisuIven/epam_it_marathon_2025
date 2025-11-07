using CSharpFunctionalExtensions;
using Epam.ItMarathon.ApiService.Application.UseCases.User.Commands;
using Epam.ItMarathon.ApiService.Domain.Abstract;
using Epam.ItMarathon.ApiService.Domain.Shared.ValidationErrors;
using FluentValidation.Results;
using MediatR;
using RoomAggregate = Epam.ItMarathon.ApiService.Domain.Aggregate.Room.Room;

namespace Epam.ItMarathon.ApiService.Application.UseCases.User.Handlers
{
  public class DeleteUserHandler(IRoomRepository roomRepository)
    : IRequestHandler<DeleteUserRequest, Result<RoomAggregate, ValidationResult>>
  {
    ///<inheritdoc/>
    public async Task<Result<RoomAggregate, ValidationResult>> Handle(DeleteUserRequest request,
      CancellationToken cancellationToken)
    {
      var roomResult = await roomRepository.GetByUserCodeAsync(request.UserCode, cancellationToken);
      if (roomResult.IsFailure)
      {
        return roomResult;
      }

      var room = roomResult.Value;
      var deleteResult = room.DeleteUser(request.UserId);
      if (deleteResult.IsFailure)
      {
        return deleteResult;
      }

      var updateResult = await roomRepository.UpdateAsync(room, cancellationToken);
      if (updateResult.IsFailure)
      {
        return Result.Failure<RoomAggregate, ValidationResult>(new BadRequestError([
          new ValidationFailure(string.Empty, updateResult.Error)
        ]));
      }

      var updatedRoomResult = await roomRepository.GetByUserCodeAsync(request.UserCode, cancellationToken);
      if (updatedRoomResult.IsFailure)
      {
        return Result.Success<RoomAggregate, ValidationResult>(null);
      }

      return updatedRoomResult;
    }
  }
}