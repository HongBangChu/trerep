
CREATE OR REPLACE FUNCTION data.fxtran_get(
	p_params text,
	OUT o_rows json,
	OUT o_total integer)
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
AS $BODY$

declare
	j_params json;
    i_page integer;
	i_take integer;
begin
	i_page := p_params::json->>'page';
	i_take := p_params::json->>'take';
	select count(*) into o_total from fxtran;
	select json_agg(row_arr) into o_rows from (
		select array[tranid::text, ngaygd::text, ngaygt::text, bdscha::text, cnthien::text, gdv::text, ksv::text, cif::text, doitac::text, loaigd::text, ntegd::text, slmua::text, tgmua::text, slban::text, tgban::text, mdnb::text, gchu::text, loaikh::text, dtien2::text, kluqd::text, kluqdm::text, kluqdb::text, tgdu::text, ln2::text, lnqv::text, mdnb2::text, gchu2::text, mpa::text, lhkt::text, nnghe::text, thang::text, nam::text] as row_arr from fxtran
		order by tranid
		limit i_take offset (i_page-1)*i_take
	) tbl_arr;
end;

$BODY$;

ALTER FUNCTION data.fxtran_get(text)
    OWNER TO postgres;
