CREATE FUNCTION [Infrastructure].[GetComponentDisplayId](@ComponentId INT)
RETURNS VARCHAR(20)
WITH SCHEMABINDING
AS
BEGIN
    DECLARE @DisplayId VARCHAR(20)
    
    IF EXISTS(SELECT 1 FROM Infrastructure.Whiteboard WHERE ComponentId = @ComponentId)
        SET @DisplayId = 'WB-' + CAST(@ComponentId AS VARCHAR(10))
    ELSE IF EXISTS(SELECT 1 FROM Infrastructure.Projector WHERE ComponentId = @ComponentId)
        SET @DisplayId = 'PROJ-' + CAST(@ComponentId AS VARCHAR(10))
    ELSE
        SET @DisplayId = CAST(@ComponentId AS VARCHAR(10))
    
    RETURN @DisplayId
END