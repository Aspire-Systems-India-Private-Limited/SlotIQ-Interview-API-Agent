SELECT COUNT(*) 
FROM MEM.Member 
WHERE PhoneNumber = @PhoneNumber 
AND IsActive = 1;
