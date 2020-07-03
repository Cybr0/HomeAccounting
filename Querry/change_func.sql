-- FUNCTION: public.mainselect()

DROP FUNCTION public.mainselect();

CREATE OR REPLACE FUNCTION public.mainselect(
	)
    RETURNS TABLE(id integer, "Основная категория" character varying, "Категория" character varying, "Дата" date, "Стоимость" numeric, "Комментарий" character varying) 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
    
AS $BODY$
begin
	return query
	select e.id, m.name, n.name, e.date, ABS(e.cost), e.comment from homeAccounting.Entry as e
	left join homeAccounting.NameCategory as n
	on e.name_category = n.id
	left join homeAccounting.MainCategory as m
	on e.main_category = m.id
	order by e.date;
end
$BODY$;

ALTER FUNCTION public.mainselect()
    OWNER TO postgres;
