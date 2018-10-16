-- FUNCTION: report.fxreport_get(text)

-- DROP FUNCTION report.fxreport_get(text);

CREATE OR REPLACE FUNCTION report.customer_get(
	p_params text,
	OUT o_rows refcursor,
	OUT o_total integer)
    RETURNS record
    LANGUAGE 'plpgsql'
    COST 100.0

AS $function$

declare
	j_params json;
    i_month integer;
    i_year integer;
begin
	i_month := p_params::json->>'month';
    i_year := p_params::json->>'year';
	-- select to_number(to_char(ngaygd, 'mm'), '99') from fxtran;
	select count(*) into o_total from cusrpt;
	open o_rows for 
    	select 
        	r.cif
            , c.name
            -- fx
            , r.fxdoanhso fxdoanhsokynay
            , (select fxdoanhso from cusrpt r1 where r1.cif = r.cif and r1.month = r.month and r1.year = (r.year-1)) fxdoanhsocungky
            , (select fxdoanhso from cusrpt r1 where r1.cif = r.cif and r1.month = (r.month-1) and r1.year = r.year) fxdoanhsokytruoc
            , (select sum(fxdoanhso) from cusrpt r1 where r1.cif = r.cif and r1.month <= r.month and r1.year = (r.year-1)) fxdoanhsocungkyluyke
            , (select sum(fxdoanhso) from cusrpt r1 where r1.cif = r.cif and r1.month < r.month and r1.year = r.year) fxdoanhsokynayluyke
            , r.fxloinhuan
        from cusrpt r
        left join cust c on r.cif = c.cif;
end;

$function$;

ALTER FUNCTION report.customer_get(text)
    OWNER TO postgres;
