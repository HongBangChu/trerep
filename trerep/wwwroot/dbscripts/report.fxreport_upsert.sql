CREATE OR REPLACE FUNCTION report.fxreport_upsert(
	p_cif integer,
	p_month integer,
	p_year integer)
    RETURNS void
    LANGUAGE 'plpgsql'
    COST 100.0

AS $function$

declare
	d_doanhso numeric;
    d_loinhuan numeric;
begin
	-- hoi lai nghiep vu sum theo truong nao de lay doanh so???
	select sum(kluqd), sum(lnqv) into d_doanhso, d_loinhuan from fxtran t where t.cif = p_cif and t.thang = p_month and t.nam = p_year;
    insert into fxreport (cif, doanhso, loinhuan, thang, nam)
    values (p_cif, d_doanhso, d_loinhuan, p_month, p_year)
    on conflict (cif, thang, nam)
	do update
    	set doanhso = d_doanhso
        , loinhuan = d_loinhuan;
end;

$function$;

ALTER FUNCTION report.fxreport_upsert(integer, integer, integer)
    OWNER TO postgres;