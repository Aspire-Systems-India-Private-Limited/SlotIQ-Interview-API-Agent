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
WHERE MemberID = @MemberID
AND IsActive = 1;
