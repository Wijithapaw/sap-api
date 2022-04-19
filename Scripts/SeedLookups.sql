do $$
declare 
	expenseHeader varchar(50); 
	incomeHeader varchar(50);
begin
	select "Id" into expenseHeader from "LookupHeaders" where "Code"='EXPENSE_TYPES';
	select "Id" into incomeHeader from "LookupHeaders" where "Code"='INCOME_TYPES';
		
	insert into "Lookups" 
	("Id", "HeaderId", "Code", "Name", "Inactive", "Protected", "CreatedBy", "CreatedDateUtc", "LastUpdatedBy", "LastUpdatedDateUtc")
	values
	('lookup-100', expenseHeader, 'SHARE_DIVIDEND', 'Share Dividend', false, true, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-101', expenseHeader, 'LABOUR', 'Labour', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-102', expenseHeader, 'MATERIAL', 'Material', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-103', expenseHeader, 'BASS_PAYMENT', 'Bass Payment', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-104', expenseHeader, 'FERTILIZER', 'Fertilizer', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-105', expenseHeader, 'PESTICIDE', 'Pesticide', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-106', expenseHeader, 'SALARY', 'Salary', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-107', expenseHeader, 'OTHER', 'Other', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14'),	
	('lookup-108', expenseHeader, 'LANDSCAPE', 'Landscaping', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-109', expenseHeader, 'SEEDS_PLANTS', 'Seends & Plants', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14'),

	('lookup-201', incomeHeader, 'SELLING_CROP', 'Selling Crop', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-202', incomeHeader, 'SUBSIDY', 'Subsidy', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14'),
	('lookup-203', incomeHeader, 'OTHER', 'Other', false, false, 'seed', '2022-3-14', 'seed', '2022-3-14');
	
end $$;

--select * from "Lookups"

