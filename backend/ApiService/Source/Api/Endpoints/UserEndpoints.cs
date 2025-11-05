using AutoMapper;
using Epam.ItMarathon.ApiService.Api.Dto.Requests.UserRequests;
using Epam.ItMarathon.ApiService.Api.Dto.Responses.UserResponses;
using Epam.ItMarathon.ApiService.Api.Endpoints.Extension;
using Epam.ItMarathon.ApiService.Api.Endpoints.Extension.SwaggerTagExtension;
using Epam.ItMarathon.ApiService.Api.Filters.Validation;
using Epam.ItMarathon.ApiService.Application.Models.Creation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Epam.ItMarathon.ApiService.Api.Dto.ReadDtos;
using Epam.ItMarathon.ApiService.Application.UseCases.User.Commands;
using Epam.ItMarathon.ApiService.Application.UseCases.User.Queries;

namespace Epam.ItMarathon.ApiService.Api.Endpoints
{
    /// <summary>
    /// Endpoints for the Users.
    /// </summary>
    public static class UserEndpoints
    {
        /// <summary>
        /// Static method to map User's endpoints to DI container.
        /// </summary>
        /// <param name="application">The WebApplication instance.</param>
        /// <returns>Reference to input <paramref name="application"/>.</returns>
        public static WebApplication MapUserEndpoints(this WebApplication application)
        {
            var root = application.MapGroup("/api/users")
                .WithTags("User")
                .WithTagDescription("User", "User endpoints")
                .WithOpenApi();

            _ = root.MapGet("", GetUsers)
                .AddEndpointFilterFactory(ValidationFactoryFilter.GetValidationFactory)
                .Produces<List<UserReadDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithSummary("Auth by UserCode and Read all user in auth user's room.")
                .WithDescription("Return list of users.");

            _ = root.MapGet("{id:long}", GetUserWithId)
                .AddEndpointFilterFactory(ValidationFactoryFilter.GetValidationFactory)
                .Produces<List<UserReadDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithSummary("Auth by UserCode and Read user info by user Id.")
                .WithDescription("Return user info.");

            _ = root.MapDelete("{id:long}", DeleteUserWithId)
                .AddEndpointFilterFactory(ValidationFactoryFilter.GetValidationFactory)
                .Produces<List<UserReadDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithSummary("Delete user by id.")
                .WithDescription("OK if successful.");

            _ = root.MapPost("", JoinUserToRoom)
                .Produces<UserCreationResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithOpenApi(operation =>
                {
                    operation.Responses.Remove(StatusCodes.Status200OK.ToString());
                    return operation;
                })
                .WithSummary("Create and add user to a room.")
                .WithDescription("Return created user info.");

            return application;
        }

        public static async Task<IResult> DeleteUserWithId([FromRoute] ulong id, [FromQuery, Required] string? userCode,
            IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method that handles get all Users in the Room logic.
        /// </summary>
        /// <param name="userCode">User's authorization code.</param>
        /// <param name="mediator">Implementation of <see cref="IMediator"/> for handling business logic.</param>
        /// <param name="mapper">Implementation of <see cref="IMapper"/> for converting objects.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> that can be used to cancel operation.</param>
        /// <returns>Returns <seealso cref="IResult"/> depending on operation result.</returns>
        public static async Task<IResult> GetUsers([FromQuery, Required] string? userCode, IMediator mediator,
            IMapper mapper, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetUsersQuery(userCode!, null), cancellationToken);
            if (result.IsFailure)
            {
                return result.Error.ValidationProblem();
            }

            var responseUsers = mapper.Map<List<UserReadDto>>(result.Value,
                options => { options.SetUserMappingOptions(result.Value, userCode!); });
            return Results.Ok(responseUsers);
        }

        /// <summary>
        /// Get exact User by unique identifier logic.
        /// </summary>
        /// <param name="id">Unique identifier of the User.</param>
        /// <param name="userCode">User authorization code.</param>
        /// <param name="mediator">Implementation of <see cref="IMediator"/> for handling business logic.</param>
        /// <param name="mapper">Implementation of <see cref="IMapper"/> for converting objects.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> that can be used to cancel operation.</param>
        /// <returns>Returns <seealso cref="IResult"/> depending on operation result.</returns>
        public static async Task<IResult> GetUserWithId([FromRoute] ulong id, [FromQuery, Required] string? userCode,
            IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetUsersQuery(userCode!, id), cancellationToken);
            if (result.IsFailure)
            {
                return result.Error.ValidationProblem();
            }

            var responseUser = mapper.Map<List<UserReadDto>>(new[] { result.Value.First(user => user.Id.Equals(id)) },
                options => { options.SetUserMappingOptions(result.Value, userCode!); });
            return Results.Ok(responseUser);
        }

        /// <summary>
        /// Join User logic.
        /// </summary>
        /// <param name="roomCode">Room invitation code.</param>
        /// <param name="user">User's request data.</param>
        /// <param name="mediator">Implementation of <see cref="IMediator"/> for handling business logic.</param>
        /// <param name="mapper">Implementation of <see cref="IMapper"/> for converting objects.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> that can be used to cancel operation.</param>
        /// <returns>Returns <seealso cref="IResult"/> depending on operation result.</returns>
        public static async Task<IResult> JoinUserToRoom([FromQuery, Required] string roomCode,
            UserCreationRequest user, IMediator mediator, IMapper mapper, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new CreateUserInRoomRequest(
                mapper.Map<UserApplication>(user), roomCode), cancellationToken);
            return result.IsFailure
                ? result.Error.ValidationProblem()
                : Results.Created(string.Empty, mapper.Map<UserCreationResponse>(result.Value));
        }
    }
}