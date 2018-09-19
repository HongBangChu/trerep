create or replace function para.batch_upsert_cust(
    p_jsonstr json,
    o_norows out integer)
as $$
declare
	v_a integer;
begin
	o_norows := 0;
end;
$$ LANGUAGE plpgsql;