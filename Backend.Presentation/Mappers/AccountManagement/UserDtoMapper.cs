using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

/// <summary>
/// Provides mapping logic between <see cref="User"/> domain entities and DTO representations.
/// </summary>
internal static class UserDtoMapper
{
    /// <summary>
    /// Converts a <see cref="Users"/> entity to a <see cref="UsersDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Users"/> domain entity to convert.</param>
    /// <returns>A <see cref="UsersDto"/> containing the mapped data from the entity.</returns>
    internal static UserDto ToDto(User entity)
    {
        // Check that no attribute can be null
        if (entity.UserName == null)
        {
            throw new ValidationException("The Users entity cannot have null attributes.");
        }

        return new UserDto(
          entity.Id,
          entity.UserName.Value,
          entity.PersonId
        );
    }

    /// <summary>
    /// Converts a <see cref="CreateUserDto"/> to a <see cref="User"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="CreateUserDto"/> to convert.</param>
    /// <returns>A <see cref="User"/> entity constructed from the DTO values.</returns>
    internal static User ToEntity(CreateUserDto dto)
    {
        return new User(
          UserName.Create(dto.UserName), 
          dto.PersonId
        );
    }

    /// <summary>
    /// Converts a collection of <see cref="User"/> entities to a list of <see cref="UserDto"/>.
    /// </summary>
    /// <param name="people">The collection of <see cref="User"/> entities to convert.</param>
    /// <returns>A list of <see cref="UserDto"/> objects mapped from the input entities.</returns>
    internal static List<UserDto> ToDtoList(IEnumerable<User> user)
    {
        // Creates an empty list of PersonDto
        return [.. user.Select(ToDto)];
    }

    /// <summary>
    /// Updates an existing <see cref="User"/> entity using data from a <see cref="ModifyUserDto"/>.
    /// </summary>
    /// <param name="entity">The existing <see cref="User"/> to update.</param>
    /// <param name="dto">The <see cref="ModifyUserDto"/> containing the new values.</param>
    internal static void UpdateEntity(User entity, ModifyUserDto dto)
    {
        if (dto.UserName != default)
        {
            entity.UserName = UserName.Create(dto.UserName);
        }
    }

    }
