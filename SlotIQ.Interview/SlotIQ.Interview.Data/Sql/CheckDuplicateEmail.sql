SELECT COUNT(*) 
FROM Members 
WHERE EmailID = @EmailID 
AND IsActive = 1;
