
CREATE OR REPLACE FUNCTION para.cust_get(
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
	select count(*) into o_total from cust;
	select json_agg(row_arr) into o_rows from (
		select array[cif::text, cusseg::text, cfsic8::text, name::text, cifbrn::text, incrra::text, busine::text, addres::text, distri::text, state::text, rm::text, cro::text] as row_arr from cust
		order by cif
		limit i_take offset (i_page-1)*i_take
	) tbl_arr;
end;

$BODY$;

ALTER FUNCTION para.cust_get(text)
    OWNER TO postgres;
