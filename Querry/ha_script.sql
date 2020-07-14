-- Database: homeaccountingtest
drop table if exists homeAccounting.Entry;
drop table if exists homeAccounting.NameCategory;
drop table if exists homeAccounting.MainCategory;
drop function if exists test_select;
drop function if exists mainSelect;
drop function if exists adding_new_date;
drop function if exists public.namecategory_insert;
drop schema if exists homeAccounting;

-----------------------------------------------------------------------------------------------------------------------------------------------------------------
create schema homeAccounting;

create table homeAccounting.MainCategory
(
	id serial primary key,
	name character varying not null
);

create table homeAccounting.NameCategory
(
	id serial primary key,
	main_category int not null,
	name character varying not null,
	foreign key(main_category) references homeAccounting.MainCategory(id)
);

create table homeAccounting.Entry
(
	id serial primary key,
	main_category int not null,
	name_category int not null,
	date date default CURRENT_DATE,	
	cost decimal default 0,
	comment character varying,
	foreign key(main_category) references homeAccounting.MainCategory(id),
	foreign key(name_category) references homeAccounting.NameCategory(id)
	
);

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


-----------------------------------------------------------------------------------------------------------------------------------------------------------------
--select_func_for--выподающий список

create or replace function test_select
(
	_month int,
	_year int
)
returns table
(
	c_entry character varying,
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
language plpgsql;


-----------------------------------------------------------------------------------------------------------------------------------------------------------------



create or replace function mainSelect()
returns table
(
	id int,
	"Основная категория" character varying,
	Категория character varying,
	Дата date,
	Стоимость decimal,
	Комментарий character varying
)
as
$$
begin
	return query
	select e.id, m.name, n.name, e.date, ABS(e.cost), e.comment from homeAccounting.Entry as e
	left join homeAccounting.NameCategory as n
	on e.name_category = n.id
	left join homeAccounting.MainCategory as m
	on e.main_category = m.id
	order by e.date;
end
$$
language plpgsql;



-----------------------------------------------------------------------------------------------------------------------------------------------------------------

--insert function
create or replace function adding_new_date
(
	_main_category int,
	_name_category int,
	_date date,	
	_cost decimal,
	_comment character varying
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
language plpgsql;
--test function insert




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