SELECT COUNT(*) 
FROM MEM.Member 
WHERE EmailID = @EmailID 
AND IsActive = 1;
