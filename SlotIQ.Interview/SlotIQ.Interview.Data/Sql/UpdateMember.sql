UPDATE MEM.Member
SET 
    FirstName = @FirstName,
    LastName = @LastName,
    EmailID = @EmailID,
    PhoneNumber = @PhoneNumber,
    RoleID = @RoleID,
    PracticeID = @PracticeID,
    ModifiedDate = @ModifiedDate,
    ModifiedBy = @ModifiedBy,
    Source = @Source
WHERE MemberID = @MemberID;
