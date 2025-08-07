using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;

// ==================================================================================
// EPIC ID: CPD-LC-002 – Learning Component Core Refactors
// USER STORY: CPD-LC-002-007 – Refactor mapping logic for learning components using polymorphism
//
// TASKS:
// - T1: Design generic base mapper class for LearningComponent<T, TDto>
// - T2: Implement ProjectorDtoMapper and WhiteboardDtoMapper, ProjectorNoIdDtoMapper and WhiteboardNoIdDtoMapper
// - T3: Implement GlobalMapper for dynamic resolution
// - T4: Add support for DTOs without IDs and their mappers
// - T5: Refactor handler to use GlobalMapper
// - T6: Manual testing and debugging
// - T7: Technical validation with assistants
// - T8: Final review and cleanup
//
// PARTICIPANTS:
// - Ericka Araya Hidalgo – C20553
// - Luis Fonseca Chinchilla – C03035 
//
// COMMENT:
// This file is part of the centralized refactor of learning component mappers using
// polymorphism, generic base classes, and dynamic type-based resolution.
// ==================================================================================

/// <summary>
/// Abstract base class for mapping between a <see cref="LearningComponent"/> entity
/// and its corresponding DTO that includes an identifier.
/// </summary>
/// <typeparam name="T">The specific type of <see cref="LearningComponent"/> to map from.</typeparam>
/// <typeparam name="TDto">The DTO type that includes an ID, mapped to/from the component.</typeparam>
public abstract class LearningComponentDtoMapper<T, TDto>
    where T : LearningComponent
    where TDto : LearningComponentDto
{
    /// <summary>
    /// Converts a domain entity of type <typeparamref name="T"/> to its corresponding DTO.
    /// </summary>
    /// <param name="component">The entity instance to convert.</param>
    /// <returns>A <typeparamref name="TDto"/> representing the entity.</returns>
    public abstract TDto ToDto(T component);

    /// <summary>
    /// Converts a DTO of type <typeparamref name="TDto"/> to its corresponding domain entity.
    /// </summary>
    /// <param name="dto">The DTO to convert.</param>
    /// <returns>A <typeparamref name="T"/> entity constructed from the DTO.</returns>
    public abstract T ToEntity(TDto dto);
}





