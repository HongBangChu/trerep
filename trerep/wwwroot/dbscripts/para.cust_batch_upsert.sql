-- FUNCTION: para.cust_batch_upsert(text)

-- DROP FUNCTION para.cust_batch_upsert(text);

CREATE OR REPLACE FUNCTION para.cust_batch_upsert(
	p_params text,
	OUT o_result json)
    RETURNS json
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
AS $BODY$

declare
	json_row json;
    i_cif integer;
	json_row_result json;
	json_result jsonb;
begin
	json_result := '[]';
	for json_row in select * from json_array_elements(p_params::json)
    loop
		i_cif := json_row->>0::integer;
		begin
			-- https://www.postgresql.org/docs/9.3/static/functions-json.html
			-- test function select * from para.cust_batch_upsert('[["1","","","","","","","","","","",""],["2","","","","","","","","","","",""]]')
			INSERT INTO public.cust(cif, cusseg, cfsic8, name, cifbrn, incrra, busine, addres, distri, state, rm, cro)
			VALUES (i_cif, json_row->>1, json_row->>2, json_row->>3, json_row->>4, json_row->>5, json_row->>6, json_row->>7, json_row->>8, json_row->>9, json_row->>10, json_row->>11)
			on conflict (cif)
			do update
				SET cusseg=json_row->>1, cfsic8=json_row->>2, name=json_row->>3, cifbrn=json_row->>4, incrra=json_row->>5, busine=json_row->>6, addres=json_row->>7, distri=json_row->>8, state=json_row->>9, rm=json_row->>10, cro=json_row->>11;
			select json_result||('{"cif":'||i_cif||',"result":"success"}')::jsonb into json_result;
		exception when others then
			select json_result||('{"cif":'||i_cif||',"result":"fail","detail":"'||sqlerrm||sqlstate||'"}')::jsonb into json_result;
		end;
    end loop;
	o_result := json_result::json;
end;

$BODY$;

ALTER FUNCTION para.cust_batch_upsert(text)
    OWNER TO postgres;
