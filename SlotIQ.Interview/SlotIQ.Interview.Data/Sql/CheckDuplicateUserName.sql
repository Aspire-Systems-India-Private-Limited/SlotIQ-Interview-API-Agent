SELECT COUNT(*) 
FROM MEM.Member 
WHERE UserName = @UserName 
AND IsActive = 1;
