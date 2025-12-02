SELECT 
    MemberID,
    UserName,
    FirstName,
    LastName,
    Password,
    EmailID,
    PhoneNumber,
    RoleID,
    PracticeID,
    IsActive,
    CreatedDate,
    ModifiedDate,
    CreatedBy,
    ModifiedBy,
    Source
FROM MEM.Member
WHERE (@IsActive IS NULL OR IsActive = @IsActive)
  AND (@RoleID IS NULL OR RoleID = @RoleID)
  AND (@PracticeID IS NULL OR PracticeID = @PracticeID)
ORDER BY 
    CASE WHEN @SortBy = 'UserName' AND @SortOrder = 'ASC' THEN UserName END ASC,
    CASE WHEN @SortBy = 'UserName' AND @SortOrder = 'DESC' THEN UserName END DESC,
    CASE WHEN @SortBy = 'Firstname' AND @SortOrder = 'ASC' THEN FirstName END ASC,
    CASE WHEN @SortBy = 'Firstname' AND @SortOrder = 'DESC' THEN FirstName END DESC,
    CASE WHEN @SortBy = 'Lastname' AND @SortOrder = 'ASC' THEN LastName END ASC,
    CASE WHEN @SortBy = 'Lastname' AND @SortOrder = 'DESC' THEN LastName END DESC,
    CASE WHEN @SortBy = 'EmailAddress' AND @SortOrder = 'ASC' THEN EmailID END ASC,
    CASE WHEN @SortBy = 'EmailAddress' AND @SortOrder = 'DESC' THEN EmailID END DESC,
    CASE WHEN @SortBy = 'RoleName' AND @SortOrder = 'ASC' THEN RoleID END ASC,
    CASE WHEN @SortBy = 'RoleName' AND @SortOrder = 'DESC' THEN RoleID END DESC,
    CASE WHEN @SortBy = 'PracticeName' AND @SortOrder = 'ASC' THEN PracticeID END ASC,
    CASE WHEN @SortBy = 'PracticeName' AND @SortOrder = 'DESC' THEN PracticeID END DESC,
    CASE WHEN @SortBy = 'CreatedDate' AND @SortOrder = 'ASC' THEN CreatedDate END ASC,
    CASE WHEN @SortBy = 'CreatedDate' AND @SortOrder = 'DESC' THEN CreatedDate END DESC,
    CreatedDate DESC
OFFSET @Offset ROWS
FETCH NEXT @PageSize ROWS ONLY;
