-- Collars / Harnesses (CategoryId = 10)
UPDATE Products
SET Size = CASE 
              WHEN Id % 2 = 0 THEN '38cm'
              ELSE '50cm'
           END
WHERE CategoryId = 10;

-- Dry Food Dogs (CategoryId = 11)
UPDATE Products
SET Size = CASE 
              WHEN Id % 2 = 0 THEN '1.5kg'
              ELSE '3kg'
           END
WHERE CategoryId = 11;

-- Dry Food Cats (CategoryId = 12)
UPDATE Products
SET Size = CASE 
              WHEN Id % 2 = 0 THEN '400g'
              ELSE '2kg'
           END
WHERE CategoryId = 12;

-- Toys Cats (CategoryId = 13)
UPDATE Products
SET Size = CASE 
              WHEN Id % 2 = 0 THEN '1 piece'
              ELSE '2 pieces'
           END
WHERE CategoryId = 13;

-- Wet Food Cats (CategoryId = 14)
UPDATE Products
SET Size = CASE 
              WHEN Id % 2 = 0 THEN '200g'
              ELSE '415g'
           END
WHERE CategoryId = 14;

-- Pharmacy (CategoryId = 8)
UPDATE Products
SET Size = CASE 
              WHEN Id % 2 = 0 THEN '120ml'
              ELSE '1 Tablet'
           END
WHERE CategoryId = 8;
