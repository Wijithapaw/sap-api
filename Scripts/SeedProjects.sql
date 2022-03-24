--select * from Projects

insert into Projects 
(Id, Name, State, StartDate, EndDate, CreatedBy, CreatedDateUtc, LastUpdatedBy, LastUpdatedDateUtc)
values
('project-1', 'Wire Fence and Gate', 2, '2020-3-1', '2020-7-1', 'seed', '2022-3-14', 'seed', '2022-3-14'),
('project-2', 'Bridge', 2, '2020-9-1', '2020-9-10', 'seed', '2022-3-14', 'seed', '2022-3-14'),
('project-3', 'Clearing Canal', 2, '2020-2-1', '2020-2-28', 'seed', '2022-3-14', 'seed', '2022-3-14'),
('project-4', 'Papow', 1, '2021-11-1', null, 'seed', '2022-3-14', 'seed', '2022-3-14'),
('project-5', 'Peanut', 1, '2021-12-1', null, 'seed', '2022-3-14', 'seed', '2022-3-14'),
('project-6', 'Vegitable', 1, '2022-1-1', null, 'seed', '2022-3-14', 'seed', '2022-3-14'),
('project-7', 'Common Infrastructure', 1, '2020-1-1', null, 'seed', '2022-3-14', 'seed', '2022-3-14')