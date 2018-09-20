create or replace function para.cust_batch_upsert(
    p_jsonstr json,
    o_norows out integer)
as $$
declare
	v_a integer;
begin
	o_norows := 0;
end;
$$ LANGUAGE plpgsql;