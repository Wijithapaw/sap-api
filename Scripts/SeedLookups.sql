﻿do $$
declare 
	expenseHeader varchar(50); 
	incomeHeader varchar(50);
begin
	select "Id" into expenseHeader from "LookupHeaders" where "Code"='EXPENSE_TYPES';
	select "Id" into incomeHeader from "LookupHeaders" where "Code"='INCOME_TYPES';
		
	insert into "Lookups" 
	("Id", "HeaderId", "Code", "Name", "Inactive", "CreatedBy", "CreatedDateUtc", "LastUpdatedBy", "LastUpdatedDateUtc")
	values
	('lookup-101', expenseHeader, 'LABOUR', 'Labour', false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-102', expenseHeader, 'MATERIAL', 'Material', false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-103', expenseHeader, 'BASS_PAYMENT', 'Bass Payment', false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-104', expenseHeader, 'FERTILIZER', 'Fertilizer', false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-105', expenseHeader, 'PESTICIDE', 'Pesticide', false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-106', expenseHeader, 'SALARY', 'Salary', false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-107', expenseHeader, 'OTHER', 'Other', false, 'seed', '2022-3-14', 'seed', '2022-3-14'),

	('lookup-201', incomeHeader, 'SELLING_CROP', 'Selling Crop', false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-202', incomeHeader, 'SUBSIDY', 'Subsidy', false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-203', incomeHeader, 'OTHER', 'Other', false, 'seed', '2022-3-14', 'seed', '2022-3-14');
	
end $$;

--select * from "Lookups"

