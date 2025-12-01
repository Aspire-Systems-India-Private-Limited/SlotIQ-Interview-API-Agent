UPDATE MEM.Member
SET 
    UserName = @UserName,
    FirstName = @FirstName,
    LastName = @LastName,
    EmailID = @EmailID,
    PhoneNumber = @PhoneNumber,
    RoleID = @RoleID,
    PracticeID = @PracticeID,
    IsActive = @IsActive,
    ModifiedDate = @ModifiedDate,
    ModifiedBy = @ModifiedBy,
    Source = @Source
WHERE MemberID = @MemberID;
