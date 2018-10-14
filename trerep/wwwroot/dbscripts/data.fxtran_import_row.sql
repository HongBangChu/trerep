CREATE OR REPLACE FUNCTION data.fxtran_import_row(
	p_params text,
	OUT o_result json)
    RETURNS json
    LANGUAGE 'plpgsql'
    COST 100.0

AS $function$

declare
	json_row json;
    i_month integer;
    i_year integer;
    i_tranid integer;
    i_cif integer;
	json_result jsonb;
begin
	json_result := '{}';
	i_tranid := 0;
    -- i_month := to_number(p_params::json->>'month', '99');
    -- i_year := to_number(p_params::json->>'year', '9999');
    json_row := p_params::json->>'row';
    i_month := to_number(to_char(to_date(json_row->>1, 'dd/mm/yyyy'), 'mm'), '99');
    i_year := to_number(to_char(to_date(json_row->>1, 'dd/mm/yyyy'), 'yyyy'), '9999');
	-- 0: stt, 7: id
    i_tranid := json_row->>7;
    i_cif := to_number(json_row->>8, '9999999999');
	INSERT INTO public.fxtran (
		tranid
		, ngaygd
		, ngaygt
		, bdscha
		, cnthien
		, gdv
		, ksv
		, cif
		, doitac
		, loaigd
		, ntegd
		, slmua
		, tgmua
		, slban
		, tgban
		, mdnb, gchu, loaikh, dtien2
		, kluqd, kluqdm, kluqdb, tgdu, ln2, lnqv
		, mdnb2, gchu2, mpa, lhkt, nnghe
		, thang, nam
	)
	VALUES (
		i_tranid
		, to_date(json_row->>1, 'dd/mm/yyyy')
		, to_date(json_row->>2, 'dd/mm/yyyy')
		, json_row->>3
		, json_row->>4
		, json_row->>5
		, json_row->>6
		, i_cif
		, json_row->>9
		, json_row->>10
		, json_row->>11
		, to_number(json_row->>12, '9999999999d99999999')
		, to_number(json_row->>13, '9999999999d99999999')
		, to_number(json_row->>14, '9999999999d99999999')
		, to_number(json_row->>15, '9999999999d99999999')
		, json_row->>16, json_row->>17, json_row->>18, json_row->>19
		, to_number(json_row->>20, '9999999999d99'), to_number(json_row->>21, '9999999999d99'), to_number(json_row->>22, '9999999999d99'), to_number(json_row->>23, '9999999999d99'), to_number(json_row->>24, '9999999999d99'), to_number(json_row->>25, '9999999999d99')
		, json_row->>26, json_row->>27, json_row->>28, json_row->>29, json_row->>30
		, i_month, i_year
	)
	on conflict (tranid)
    do update SET 
		ngaygd = to_date(json_row->>1, 'dd/mm/yyyy')
		, ngaygt = to_date(json_row->>2, 'dd/mm/yyyy')
		, bdscha=json_row->>3
		, cnthien=json_row->>4
		, gdv=json_row->>5
		, ksv=json_row->>6
		, cif=i_cif
		, doitac=json_row->>9
		, loaigd=json_row->>10
		, ntegd=json_row->>11
		, slmua=to_number(json_row->>12, '9999999999d99999999')
		, tgmua=to_number(json_row->>13, '9999999999d99999999')
		, slban=to_number(json_row->>14, '9999999999d99999999')
		, tgban=to_number(json_row->>15, '9999999999d99999999')
		, mdnb=json_row->>16, gchu=json_row->>17, loaikh=json_row->>18, dtien2=json_row->>19
		, kluqd=to_number(json_row->>20, '9999999999d99'), kluqdm=to_number(json_row->>21, '9999999999d99'), kluqdb=to_number(json_row->>22, '9999999999d99'), tgdu=to_number(json_row->>23, '9999999999d99'), ln2=to_number(json_row->>24, '9999999999d99'), lnqv=to_number(json_row->>25, '9999999999d99')
		, mdnb2=json_row->>26, gchu2=json_row->>27, mpa=json_row->>28, lhkt=json_row->>29, nnghe=json_row->>30
		, thang=i_month, nam=i_year
	;
    select ('{"tranid":'||i_tranid||',"result":"success"}')::jsonb into json_result;
    perform report.fxreport_upsert(i_cif, i_month, i_year);
	o_result := json_result::json;
exception when others then
	select ('{"tranid":'||i_tranid||',"result":"fail","detail":"'||sqlerrm||sqlstate||'"}')::jsonb into json_result;
    o_result := json_result::json;
end;

$function$;

ALTER FUNCTION data.fxtran_import_row(text)
    OWNER TO postgres;