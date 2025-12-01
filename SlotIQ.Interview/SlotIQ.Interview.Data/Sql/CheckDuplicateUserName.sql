SELECT COUNT(*) 
FROM Members 
WHERE UserName = @UserName 
AND IsActive = 1;
