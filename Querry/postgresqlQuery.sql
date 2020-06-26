-- Database: homeaccountingtest
create schema homeAccounting;

create table homeAccounting.MainCategory
(
	id serial primary key,
	name character varying(50) not null
);

create table homeAccounting.NameCategory
(
	id serial primary key,
	main_category int not null,
	name character varying(50) not null,
	foreign key(main_category) references homeAccounting.MainCategory(id)
);

create table homeAccounting.Entry
(
	id serial primary key,
	main_category int not null,
	name_category int not null,
	date date default CURRENT_DATE,	
	cost decimal default 0,
	comment character varying(100),
	foreign key(main_category) references homeAccounting.MainCategory(id),
	foreign key(name_category) references homeAccounting.NameCategory(id)
	
);

-----------------------------------------------------------------------------------------------------------------------------------------------------------------
select * from homeAccounting.MainCategory;

select * from homeAccounting.NameCategory;

select * from homeAccounting.Entry;

-----------------------------------------------------------------------------------------------------------------------------------------------------------------
insert into homeAccounting.MainCategory(name)
	values
	('Доход'),('Расход');

insert into homeAccounting.NameCategory(main_category, name)
	values
	(2, 'Другое'),
	(1, 'Иные доходы'),
	(1, 'Заработная плата'),
	(1, 'Сдача в аренду недвижимость'),
	(2, 'Продукты питaния'),
	(2, 'Транспорт'),
	(2, 'Мобильная связь'),
	(2, 'Интернет'),
	(2, 'Развлечения');


insert into homeAccounting.Entry(main_category, name_category, date, cost, comment)
	values
	(1, 3,'2020-05-16', 530, 'получил зп!'),
	(1, 4,'2020-05-21', 400, 'за аренду!'),
	(2, 5,'2020-06-23', 25, 'кг мясо'),
	(2, 8,'2020-06-21', 70, ''),
	(2, 5,'2020-06-21', 70, 'молоко'),
	(2, 5,'2020-06-21', 70, 'хлеб'),
	(2, 5,'2020-06-21', 70, 'хлеб'),
	(2, 8,'2020-05-21', 70, 'за нэт'),
	(2, 8,'2020-04-21', 70, 'нэт'),
	(2, 9,'2020-04-15', 300, 'клуб'),
	(1, 3,'2020-04-21', 540, 'получил зп!!'),
	(1, 3,'2020-06-05', 530, 'получил зп!!');

-- insert into homeAccounting.Entry(main_category, name_category, cost, comment)
-- 	values
-- 	(2, 8, 100, 'за нэт');
	

-----------------------------------------------------------------------------------------------------------------------------------------------------------------
--select * from homeAccounting.MainCategory;
--select * from homeAccounting.NameCategory;

select e.id, m.name, n.name, e.date, e.cost, e.comment from homeAccounting.Entry as e
left join homeAccounting.NameCategory as n
on e.name_category = n.id
left join homeAccounting.MainCategory as m
on e.main_category = m.id
order by e.id;


-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-- update homeAccounting.Entry
-- 	set
-- 	main_category = 2
-- 	where id = 8;
	
-----------------------------------------------------------------------------------------------------------------------------------------------------------------


---select *, Extract(month from e.date::date ) as month_col_name from homeAccounting.Entry as e
select 'Январь', 0 

union all

select distinct(n.name) as Месяц, sum(e.cost) as "Итоговая стоимость" from homeAccounting.Entry as e
left join homeAccounting.NameCategory as n
on e.name_category = n.id
where Extract(month from e.date::date ) = 4 and (Extract(year from e.date::date ) = 2020)
group by n.name

union all

select 'Февраль', 0 

union all

select distinct(n.name) as Февраль, sum(e.cost) as "Итоговая стоимость" from homeAccounting.Entry as e
left join homeAccounting.NameCategory as n
on e.name_category = n.id
where Extract(month from e.date::date ) = 4 and (Extract(year from e.date::date ) = 2020)
group by n.name


select * from homeAccounting.NameCategory as n
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-- delete from homeAccounting.Entry;
-----------------------------------------------------------------------------------------------------------------------------------------------------------------

-- -- crosstab
-- create extension tablefunc;



-- select * from crosstab (
-- $$select distinct(e.name_category), n.name as Namess, sum(e.cost) Total from homeAccounting.Entry as e
-- left join homeAccounting.NameCategory as n
-- on e.name_category = n.id
-- where Extract(month from e.date::date ) = 4 and (Extract(year from e.date::date ) = 2020)
-- group by n.name, e.name_category; $$
-- ) as ct(Square int, "Заработная плата" decimal, "Интернет" decimal, "Развлечения" decimal)

-----------------------------------------------------------------------------------------------------------------------------------------------------------------
--select_func_for--выподающий список

DROP FUNCTION test_select(integer,integer)

create or replace function test_select
(
	_month int,
	_year int
)
returns table
(
	c_entry character varying(50),
	c_total decimal
)
as
$$
begin
	return query
	select distinct(n.name) as Месяц, sum(e.cost) as "Итоговая стоимость" from homeAccounting.Entry as e
	left join homeAccounting.NameCategory as n
	on e.name_category = n.id
	where Extract(month from e.date::date ) = _month and (Extract(year from e.date::date ) = _year)
	group by n.name;
end
$$
language plpgsql

select * from test_select(4,2020)

select distinct(n.name) as Месяц, sum(e.cost) as "Итоговая стоимость" from homeAccounting.Entry as e
left join homeAccounting.NameCategory as n
on e.name_category = n.id
where Extract(month from e.date::date ) = 4 and (Extract(year from e.date::date ) = 2020)
group by n.name

-----------------------------------------------------------------------------------------------------------------------------------------------------------------

DROP FUNCTION mainSelect()

create or replace function mainSelect()
returns table
(
	id int,
	"Основная категория" character varying(50),
	Категория character varying(50),
	Дата date,
	Стоимость decimal,
	Комментарий character varying(100)
)
as
$$
begin
	return query
	select e.id, m.name, n.name, e.date, e.cost, e.comment from homeAccounting.Entry as e
	left join homeAccounting.NameCategory as n
	on e.name_category = n.id
	left join homeAccounting.MainCategory as m
	on e.main_category = m.id
	order by e.date;
end
$$
language plpgsql


select id, "Основная категория", Категория, to_char(Дата,'dd-mm-yyyy') as "Дата", Стоимость, Комментарий from mainSelect()


-----------------------------------------------------------------------------------------------------------------------------------------------------------------

--insert function
create or replace function adding_new_date
(
	_main_category int,
	_name_category int,
	_date date,	
	_cost decimal,
	_comment character varying(100)
)
returns int as 
$$
begin
	insert into homeAccounting.Entry(main_category, name_category, date, cost, comment)
	values(_main_category, _name_category, _date, _cost, _comment);
	if found then --inserted successfully
		return 1;
	else return 0; --inserted fail
	end if;
end
$$
language plpgsql
--test function insert

select * from adding_new_date(1, 10, to_date('29-05-2020','dd-mm-yyyy'), 3000, 'someone pay for my phone');

select * from homeAccounting.Entry
delete from homeAccounting.Entry
where id = 37


select * from homeAccounting.NameCategory
delete from homeAccounting.NameCategory
where id = 22

-----------------------------------------------------------------------------------------------------------------------------------------------------------------



CREATE OR REPLACE FUNCTION public.namecategory_insert(
	_main_category integer,
	_name character varying)
    RETURNS integer
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    
AS $BODY$
begin
	insert into homeAccounting.NameCategory(main_category, name) 
	values(_main_category, _name);
	if found then --inserted successfully
		return 1;
	else return 0; --inserted fail
	end if;
end
$BODY$;

ALTER FUNCTION public.namecategory_insert(integer, character varying)
    OWNER TO postgres;
	
	
	
-----------------------------------------------------------------------------------------------------------------------------------------------------------------

update homeAccounting.Entry
	set
	main_category = 1, 
	name_category = 8, 
	date = to_date('01-05-2020','dd-mm-yyyy'), 
	cost = 500, 
	comment = 'Чипсы'
	where id = 36;