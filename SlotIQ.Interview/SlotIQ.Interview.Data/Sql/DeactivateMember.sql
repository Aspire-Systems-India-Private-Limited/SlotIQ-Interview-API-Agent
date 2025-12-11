UPDATE MEM.Member
SET 
    IsActive = 0,
    ModifiedDate = @ModifiedDate,
    ModifiedBy = @ModifiedBy,
    Source = @Source
WHERE MemberID = @MemberID
  AND IsActive = 1;
