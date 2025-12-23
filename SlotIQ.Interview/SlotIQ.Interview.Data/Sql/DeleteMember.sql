UPDATE MEM.Member
SET IsActive = 0,
    ModifiedDate = GETUTCDATE()
WHERE MemberID = @MemberID;
