declare @expenseHeader varchar(50), @incomeHeader varchar(50)

select @expenseHeader = Id from LookupHeaders where Code='EXPENSE_TYPES'
select @incomeHeader = Id from LookupHeaders where Code='INCOME_TYPES'

insert into Lookups 
(Id, HeaderId, Code, Name, Inactive, CreatedBy, CreatedDateUtc, LastUpdatedBy, LastUpdatedDateUtc)
values
('lookup-101', @expenseHeader, 'LABOUR', 'Labour', 0, 'seed', '2022-3-14', 'seed', '2022-3-14'),
('lookup-102', @expenseHeader, 'MATERIAL', 'Material', 0, 'seed', '2022-3-14', 'seed', '2022-3-14'),
('lookup-103', @expenseHeader, 'BASS_PAYMENT', 'Bass Payment', 0, 'seed', '2022-3-14', 'seed', '2022-3-14'),
('lookup-104', @expenseHeader, 'FERTILIZER', 'Fertilizer', 0, 'seed', '2022-3-14', 'seed', '2022-3-14'),
('lookup-105', @expenseHeader, 'PESTICIDE', 'Pesticide', 0, 'seed', '2022-3-14', 'seed', '2022-3-14'),
('lookup-106', @expenseHeader, 'SALARY', 'Salary', 0, 'seed', '2022-3-14', 'seed', '2022-3-14'),
('lookup-107', @expenseHeader, 'OTHER', 'Other', 0, 'seed', '2022-3-14', 'seed', '2022-3-14'),

('lookup-201', @incomeHeader, 'SELLING_CROP', 'Selling Crop', 0, 'seed', '2022-3-14', 'seed', '2022-3-14'),
('lookup-202', @incomeHeader, 'SUBSIDY', 'Subsidy', 0, 'seed', '2022-3-14', 'seed', '2022-3-14'),
('lookup-203', @incomeHeader, 'OTHER', 'Other', 0, 'seed', '2022-3-14', 'seed', '2022-3-14')