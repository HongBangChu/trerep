CREATE OR REPLACE FUNCTION data.fxtran_import_row(
	p_params text,
	OUT o_result json)
    LANGUAGE 'plpgsql'
    COST 100.0

AS $function$

declare
	json_row json;
    i_month integer;
    i_year integer;
    i_id integer;
	json_result jsonb;
begin
	json_result := '[]';
    i_month := p_params::json->>'month';
    i_year := p_params::json->>'year';
    json_row := p_params::json->>'row';
    i_id := json_row->>0;
    INSERT INTO public.fxtran(
	id, ngaygd, ngaygt, bdscha, cnthien, gdv, ksv, cif, doitac, loaigd, ntegd, slmua, tgmua, slban, tgban, mdnb, gchu, loaikh, dtien2, kluqd, kluqdm, kluqdb, tgdu, ln2, lnqv, mdnb2, gchu2, mpa, lhkt, nnghe, thang, nam)
	VALUES (json_row->>0, json_row->>1, json_row->>2, json_row->>3, json_row->>4, json_row->>5, json_row->>6, json_row->>7, json_row->>8, json_row->>9, json_row->>10, json_row->>11, json_row->>12, json_row->>13, json_row->>14, json_row->>15, json_row->>16, json_row->>17, json_row->>18, json_row->>19, json_row->>20, json_row->>21, json_row->>22, json_row->>23, json_row->>24, json_row->>25, json_row->>26, json_row->>27, json_row->>28, json_row->>29, i_month, i_year)
	on conflict (id)
    do update
    	SET ngaygd=json_row->>1, ngaygt=json_row->>2, bdscha=json_row->>3, cnthien=json_row->>4, gdv=json_row->>5, ksv=json_row->>6, cif=json_row->>7, doitac=json_row->>8, loaigd=json_row->>9, ntegd=json_row->>10, slmua=json_row->>11, tgmua=json_row->>12, slban=json_row->>13, tgban=json_row->>14, mdnb=json_row->>15, gchu=json_row->>16, loaikh=json_row->>17, dtien2=json_row->>18, kluqd=json_row->>19, kluqdm=json_row->>20, kluqdb=json_row->>21, tgdu=json_row->>22, ln2=json_row->>23, lnqv=json_row->>24, mdnb2=json_row->>25, gchu2=json_row->>26, mpa=json_row->>27, lhkt=json_row->>28, nnghe=json_row->>29, thang=i_month, nam=i_year;
    select json_result||('{"id":'||i_id||',"result":"success"}')::jsonb into json_result;
    
	o_result := json_result::json;
exception when others then
	select json_result||('{"cif":'||i_id||',"result":"fail","detail":"'||sqlerrm||sqlstate||'"}')::jsonb into json_result;
    o_result := json_result::json;
end;

$function$;

ALTER FUNCTION data.fxtran_import_row(text)
    OWNER TO postgres;