-- FUNCTION: data.fxtran_delete(text)

-- DROP FUNCTION data.fxtran_delete(text);

CREATE OR REPLACE FUNCTION data.fxtran_delete(
	p_params text,
	OUT o_result json)
    RETURNS json
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
AS $BODY$

declare
    i_tranid integer;
begin
	i_tranid := 0;
	i_tranid := p_params::jsonb->'tranid';
    delete from fxtran where tranid = i_tranid;
    o_result := ('{"tranid":'||i_tranid||',"result":"success"}')::json;
exception when others then
	o_result := ('{"tranid":'||i_tranid||',"result":"fail","detail":"'||sqlerrm||sqlstate||'"}')::json;
end;

$BODY$;

ALTER FUNCTION data.fxtran_delete(text)
    OWNER TO postgres;
