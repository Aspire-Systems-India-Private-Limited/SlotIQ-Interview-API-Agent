SELECT COUNT(*) 
FROM Members 
WHERE PhoneNumber = @PhoneNumber 
AND IsActive = 1;
