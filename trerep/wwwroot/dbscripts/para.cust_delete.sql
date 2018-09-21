-- FUNCTION: para.cust_delete(text)

-- DROP FUNCTION para.cust_delete(text);

CREATE OR REPLACE FUNCTION para.cust_delete(
	p_params text,
	OUT o_result json)
    RETURNS json
    LANGUAGE 'plpgsql'
    COST 100.0

AS $function$

declare
    i_cif integer;
begin
	i_cif := p_params::jsonb->'cif';
    delete from cust where cif = i_cif;
    o_result := ('{"cif":'||i_cif||',"result":"success"}')::json;
exception when others then
	o_result := ('{"cif":'||i_cif||',"result":"fail","detail":"'||sqlerrm||sqlstate||'"}')::json;
end;

$function$;

ALTER FUNCTION para.cust_delete(text)
    OWNER TO postgres;
