INSERT INTO Member (
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
)
VALUES (
    @MemberID,
    @UserName,
    @FirstName,
    @LastName,
    @Password,
    @EmailID,
    @PhoneNumber,
    @RoleID,
    @PracticeID,
    @IsActive,
    @CreatedDate,
    @ModifiedDate,
    @CreatedBy,
    @ModifiedBy,
    @Source
);

SELECT * FROM Member WHERE MemberID = @MemberID;
