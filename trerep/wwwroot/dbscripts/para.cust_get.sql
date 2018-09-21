-- FUNCTION: para.cust_get(text)

-- DROP FUNCTION para.cust_get(text);

CREATE OR REPLACE FUNCTION para.cust_get(
	p_params text,
	OUT o_result json)
    RETURNS json
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
AS $BODY$

declare
    i_cif integer;
begin
	select json_agg(row_arr) into o_result from (
		select array[cif::text, cusseg::text, cfsic8::text, name::text, cifbrn::text, incrra::text, busine::text, addres::text, distri::text, state::text, rm::text, cro::text] as row_arr from cust
	) tbl_arr;
end;

$BODY$;

ALTER FUNCTION para.cust_get(text)
    OWNER TO postgres;
