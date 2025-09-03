UPDATE Products
SET rate = CAST((RAND(CHECKSUM(NEWID())) * 2 + 3) AS DECIMAL(2,1));


select * from Products