UPDATE Members
SET IsActive = 0,
    ModifiedDate = GETUTCDATE()
WHERE MemberID = @MemberID;
