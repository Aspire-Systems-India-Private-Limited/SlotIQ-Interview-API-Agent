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
FROM Members
WHERE IsActive = 1
ORDER BY CreatedDate DESC
OFFSET @Offset ROWS
FETCH NEXT @PageSize ROWS ONLY;
