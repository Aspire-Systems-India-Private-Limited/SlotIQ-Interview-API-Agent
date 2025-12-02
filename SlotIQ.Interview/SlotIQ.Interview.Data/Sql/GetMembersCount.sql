SELECT COUNT(*) 
FROM MEM.Member 
WHERE (@IsActive IS NULL OR IsActive = @IsActive)
  AND (@RoleID IS NULL OR RoleID = @RoleID)
  AND (@PracticeID IS NULL OR PracticeID = @PracticeID);
