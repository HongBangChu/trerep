create or replace function para.cust_batch_upsert(
    p_json in json,
    o_norows out integer)
as $$
declare
	json_row json;
    c_cif integer;
begin
	for json_row in select * from json_array_elements(p_json)
    loop
    	c_cif := to_number(json_row->>0, '999D99S');
		-- continue: upsert all field
		-- https://www.postgresql.org/docs/9.3/static/functions-json.html
		-- test function select * from para.cust_batch_upsert('[["1","","","","","","","","","","",""],["2","","","","","","","","","","",""]]')
		-- upsert: http://www.postgresqltutorial.com/postgresql-upsert/
        insert into cust (cif) values (c_cif);
    end loop;
	o_norows := 0;
end;
$$ LANGUAGE plpgsql;