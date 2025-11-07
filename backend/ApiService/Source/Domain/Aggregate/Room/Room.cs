using CSharpFunctionalExtensions;
using Epam.ItMarathon.ApiService.Domain.Abstract;
using Epam.ItMarathon.ApiService.Domain.Builders;
using Epam.ItMarathon.ApiService.Domain.Entities.User;
using Epam.ItMarathon.ApiService.Domain.Shared.ValidationErrors;

using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Internal;

using Epam.ItMarathon.ApiService.Domain.Aggregate.Room;

namespace Epam.ItMarathon.ApiService.Domain.Aggregate.Room
{
  /// <summary>
  /// An aggregate of Room, encapsulates all required fields and invariants.
  /// </summary>
  public sealed class Room : BaseAggregate
  {
    internal const int NameCharLimit = 40;
    internal const int DescriptionCharLimit = 200;
    internal const int InvitationNoteCharLimit = 1000;
    internal const ulong RoomMaximumBudget = 100_000;

    /// <summary>
    /// Time when Room was closed.
    /// </summary>
    public DateTime? ClosedOn { get; private set; }

    /// <summary>
    /// Code for invitation link.
    /// </summary>
    public string InvitationCode { get; private set; } = null!;

    /// <summary>
    /// Minimal limit of Users in Room for draft.
    /// </summary>
    public uint MinUsersLimit { get; private set; }

    /// <summary>
    /// Maximum amount of Users in Room.
    /// </summary>
    public uint MaxUsersLimit { get; private set; }

    /// <summary>
    /// Maximum amount of wishes per User.
    /// </summary>
    public uint MaxWishesLimit { get; private set; }

    /// <summary>
    /// Room's name.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Room's description.
    /// </summary>
    public string Description { get; private set; } = null!;

    /// <summary>
    /// Room's invitation note (attached to invitation).
    /// </summary>
    public string InvitationNote { get; private set; } = null!;

    /// <summary>
    /// Date for gifts to be exchanged.
    /// </summary>
    public DateTime GiftExchangeDate { get; private set; }

    /// <summary>
    /// Maximum budget of the Room.
    /// </summary>
    public ulong GiftMaximumBudget { get; private set; }

    /// <summary>
    /// Indicates whether there can be no more Users in Room.
    /// </summary>
    public bool IsFull => Users.Count >= MaxUsersLimit;

    /// <summary>
    /// List of Users stored in Room.
    /// </summary>
    public IList<User> Users { get; private set; } = [];

    private Room()
    {
    }

    /// <summary>
    /// Method to initialize Room for the first time.
    /// </summary>
    /// <param name="closedOn">Time when Room was closed.</param>
    /// <param name="invitationCode">Code for invitation link.</param>
    /// <param name="name">Name of Room.</param>
    /// <param name="description">Description of Room.</param>
    /// <param name="invitationNote">Room's invitation note (attached to invitation).</param>
    /// <param name="giftExchangeDate">Date for gifts to be exchanged.</param>
    /// <param name="giftMaximumBudget">Maximum budget of the Room.</param>
    /// <param name="users">List of Users stored in Room.</param>
    /// <param name="minUsersLimit">Minimal limit of Users in Room for draft.</param>
    /// <param name="maxUsersLimit">Maximum amount of Users in Room.</param>
    /// <param name="maxWishesLimit">Maximum amount of wishes per User.</param>
    /// <returns>Returns <see cref="Room"/> encapsulated in <see cref="Result"/>.</returns>
    public static Result<Room, ValidationResult> InitialCreate(DateTime? closedOn, string invitationCode,
        string name, string description,
        string invitationNote, DateTime giftExchangeDate, ulong giftMaximumBudget, IList<User> users,
        uint minUsersLimit, uint maxUsersLimit, uint maxWishesLimit)
    {
      var room = new Room
      {
        ClosedOn = closedOn,
        InvitationCode = invitationCode,
        Name = name,
        Description = description,
        InvitationNote = invitationNote,
        GiftExchangeDate = giftExchangeDate,
        GiftMaximumBudget = giftMaximumBudget,
        Users = users,
        MinUsersLimit = minUsersLimit,
        MaxUsersLimit = maxUsersLimit,
        MaxWishesLimit = maxWishesLimit
      };

      var roomValidator = new RoomValidator();
      var validationResult = roomValidator.Validate(room);
      return !validationResult.IsValid ? Result.Failure<Room, ValidationResult>(validationResult) : room;
    }

    /// <summary>
    /// Method to create OR restore Room.
    /// </summary>
    /// <param name="id">Room's unique identifier.</param>
    /// <param name="createdOn">Time when Room was created.</param>
    /// <param name="modifiedOn">Time when Room was modified.</param>
    /// <param name="closedOn">Time when Room was closed.</param>
    /// <param name="invitationCode">Code for invitation link.</param>
    /// <param name="name">Name of Room.</param>
    /// <param name="description">Description of Room.</param>
    /// <param name="invitationNote">Room's invitation note (attached to invitation).</param>
    /// <param name="giftExchangeDate">Date for gifts to be exchanged.</param>
    /// <param name="giftMaximumBudget">Maximum budget of the Room.</param>
    /// <param name="users">List of Users stored in Room.</param>
    /// <param name="minUsersLimit">Minimal limit of Users in Room for draft.</param>
    /// <param name="maxUsersLimit">Maximum amount of Users in Room.</param>
    /// <param name="maxWishesLimit">Maximum amount of wishes per User.</param>
    /// <returns>Returns <see cref="Room"/> encapsulated in <see cref="Result"/>.</returns>
    public static Result<Room, ValidationResult> Create(ulong id, DateTime createdOn, DateTime modifiedOn,
        DateTime? closedOn, string invitationCode, string name, string description,
        string invitationNote, DateTime giftExchangeDate, ulong giftMaximumBudget, IList<User> users,
        uint minUsersLimit, uint maxUsersLimit, uint maxWishesLimit)
    {
      var admin = users.Where(user => user.IsAdmin);
      if (admin.FirstOrDefault() is null || admin.Count() > 1)
      {
        Result.Failure("The room should contain only one admin.");
      }

      var room = new Room
      {
        Id = id,
        CreatedOn = createdOn,
        ModifiedOn = modifiedOn,
        ClosedOn = closedOn,
        InvitationCode = invitationCode,
        Name = name,
        Description = description,
        InvitationNote = invitationNote,
        GiftExchangeDate = giftExchangeDate,
        GiftMaximumBudget = giftMaximumBudget,
        Users = users,
        MinUsersLimit = minUsersLimit,
        MaxUsersLimit = maxUsersLimit,
        MaxWishesLimit = maxWishesLimit
      };

      var roomValidator = new RoomValidator();
      var validationResult = roomValidator.Validate(room);
      return !validationResult.IsValid ? Result.Failure<Room, ValidationResult>(validationResult) : room;
    }

    /// <summary>
    /// Set the name of the Room.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Returns <see cref="Room"/> encapsulated in <see cref="Result"/>.</returns>
    public Result<Room, ValidationResult> SetName(string value)
    {
      return SetProperty(nameof(Name), room => room.Name = value);
    }

    /// <summary>
    /// Set the description of the Room.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Returns <see cref="Room"/> encapsulated in <see cref="Result"/>.</returns>
    public Result<Room, ValidationResult> SetDescription(string value)
    {
      return SetProperty(nameof(Description), room => room.Description = value);
    }

    /// <summary>
    /// Set the invitation note for the Room.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Returns <see cref="Room"/> encapsulated in <see cref="Result"/>.</returns>
    public Result<Room, ValidationResult> SetInvitationNote(string value)
    {
      return SetProperty(nameof(InvitationNote), room => room.InvitationNote = value);
    }

    /// <summary>
    /// Set a giftExchangeDate of the Room.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Returns <see cref="Room"/> encapsulated in <see cref="Result"/>.</returns>
    public Result<Room, ValidationResult> SetGiftExchangeDate(DateTime value)
    {
      return SetProperty(nameof(GiftExchangeDate), room => room.GiftExchangeDate = value.ToUniversalTime().Date);
    }

    /// <summary>
    /// Set a gift maximum budget of the Room.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Returns <see cref="Room"/> encapsulated in <see cref="Result"/>.</returns>
    public Result<Room, ValidationResult> SetGiftMaximumBudget(ulong value)
    {
      return SetProperty(nameof(GiftMaximumBudget), room => room.GiftMaximumBudget = value);
    }

    /// <summary>
    /// Draw the Room.
    /// </summary>
    /// <returns>Returns <see cref="Room"/> encapsulated in <see cref="Result"/>.</returns>
    public Result<Room, ValidationResult> Draw()
    {
      // Room has MinUsersCount or more
      if (Users.Count < MinUsersLimit)
      {
        return Result.Failure<Room, ValidationResult>(new BadRequestError([
            new ValidationFailure("room.MinUsersLimit", "Not enough users to draw the room.")
        ]));
      }

      // Check Room is not closed
      var roomCanBeModifiedResult = CheckRoomCanBeModified();
      if (roomCanBeModifiedResult.IsFailure)
      {
        return Result.Failure<Room, ValidationResult>(roomCanBeModifiedResult.Error);
      }

      var shuffledIds = Users.Select(user => user.Id).ToList();
      var random = new Random();

      // Shuffle list
      for (var idIndex = 0; idIndex < shuffledIds.Count - 1; idIndex++)
      {
        var swapIndex = random.Next(idIndex + 1, shuffledIds.Count);
        (shuffledIds[idIndex], shuffledIds[swapIndex]) = (shuffledIds[swapIndex], shuffledIds[idIndex]);
      }

      // Assign each participant their gift recipient
      for (var index = 0; index < Users.Count; index++)
      {
        Users[index].GiftRecipientUserId = shuffledIds[index];
      }

      ClosedOn = DateTime.UtcNow;
      return this;
    }

    /// <summary>
    /// Method to add a new User to the Room through UserBuilder.
    /// </summary>
    /// <param name="userBuilderConfiguration">User builder delegate.</param>
    /// <returns>Returns <see cref="Room"/> encapsulated in <see cref="Result"/>.</returns>
    public Result<Room, ValidationResult> AddUser(Func<UserBuilder, UserBuilder> userBuilderConfiguration)
    {
      var roomCanBeModifiedResult = CheckRoomCanBeModified();
      if (roomCanBeModifiedResult.IsFailure)
      {
        return Result.Failure<Room, ValidationResult>(roomCanBeModifiedResult.Error);
      }

      var userBuilder = new UserBuilder();
      var user = userBuilderConfiguration(userBuilder).InitialBuild();
      Users.Add(user);

      var validationResult = new RoomValidator().Validate(this);
      if (!validationResult.IsValid)
      {
        Users.Remove(user);
        return Result.Failure<Room, ValidationResult>(validationResult);
      }

      return this;
    }

    private Result<bool, ValidationResult> CheckRoomCanBeModified()
    {
      if (ClosedOn is not null)
      {
        return Result.Failure<bool, ValidationResult>(new BadRequestError([
            new ValidationFailure("room.ClosedOn", "Room is already closed.")
        ]));
      }

      return true;
    }

    private Result<Room, ValidationResult> SetProperty(string propertyName, Action<Room> setterExpression)
    {
      if (string.IsNullOrEmpty(propertyName))
      {
        return Result.Failure<Room, ValidationResult>(new BadRequestError([
            new ValidationFailure(nameof(propertyName), "Property name cannot be null or empty.")
        ]));
      }

      // Check Room is not closed
      var roomCanBeModifiedResult = CheckRoomCanBeModified();
      if (roomCanBeModifiedResult.IsFailure)
      {
        return Result.Failure<Room, ValidationResult>(roomCanBeModifiedResult.Error);
      }

      // Invoke expression to set value
      setterExpression(this);

      // Call a RoomValidator to validate updated property
      return ValidateProperty(char.ToLowerInvariant(propertyName[0]) + propertyName[1..]);
    }

    /// <summary>
    /// Method to delete a User from the Room.
    /// </summary>
    /// <returns>Returns <see cref="Room"/> encapsulated in <see cref="Result"/>.</returns>
    public Result<Room, ValidationResult> DeleteUser(ulong? userId)
    {
      var roomCanBeModifiedResult = CheckRoomCanBeModified();
      if (roomCanBeModifiedResult.IsFailure)
      {
        return Result.Failure<Room, ValidationResult>(roomCanBeModifiedResult.Error);
      }

      var userToDelete = Users.FirstOrDefault(user => user.Id == userId);
      if (userToDelete is null)
      {
        return Result.Failure<Room, ValidationResult>(new NotFoundError([
          new ValidationFailure("user.Id", "User with such id was not found in the room.")
        ]));
      }

      Users.Remove(userToDelete);
      return this;
    }

    private Result<Room, ValidationResult> ValidateProperty(string propertyName)
    {
      var validationResult = new RoomValidator().Validate(this,
          options => options.UseCustomSelector(new MemberNameValidatorSelector([propertyName])));
      return validationResult.IsValid ? this : Result.Failure<Room, ValidationResult>(validationResult);
    }
  }
}