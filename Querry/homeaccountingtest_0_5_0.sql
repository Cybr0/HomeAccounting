-- Database: homeaccounting
--create table categories
create table Categories
(
	c_id serial primary key,
	c_name character varying(30),
	c_date character varying(30),	
	c_category character varying(30),
	c_sum integer,
	c_comment character varying(100)
);

select * from categories;
---------------------------------------------------------------------------------------------------------



--insert function
create or replace function c_insert
(
	_name character varying(30),
	_date character varying(30),
	_category character varying(30),
	_sum integer,
	_comment character varying(100)
)
returns int as 
$$
begin
	insert into Categories(c_name, c_date, c_category, c_sum, c_comment)
	values(_name, _date, _category, _sum, _comment);
	if found then --inserted successfully
		return 1;
	else return 0; --inserted fail
	end if;
end
$$
language plpgsql
--test function insert
select * from c_insert('to phone', '20.06.2020', 'income', 3000, 'someone pay for my phone');
---------------------------------------------------------------------------------------------------------


--insert function update categories
create or replace function c_update
(
	_id int,
	_name character varying(30),
	_date character varying(30),
	_category character varying(30),
	_sum integer,
	_comment character varying(100)
)
returns int as 
$$
begin
	update Categories
	set
	c_name = _name, 
	c_date = _date, 
	c_category = _category, 
	c_sum = _sum, 
	c_comment = _comment
	where c_id = _id;
	if found then --updated successfully
		return 1;
	else return 0; --updated fail
	end if;
end
$$
language plpgsql
--test function update
select * from c_update(2, 'to smtng', '20.06.2020', 'income', 13000, 'bla-bla');
---------------------------------------------------------------------------------------------------------



--select function
create or replace function c_select()
returns table
(
	_id int,
	_name character varying(30),
	_date character varying(30),
	_category character varying(30),
	_sum integer,
	_comment character varying(100)
)as
$$
begin
	return query
	select c_id, c_name, c_date, c_category, c_sum, c_comment from Categories
	order by c_id;
end
$$
language plpgsql
--test select data function 
select * from c_select();
---------------------------------------------------------------------------------------------------------
drop function c_select();
---------------------------------------------------------------------------------------------------------