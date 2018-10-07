-- Table: public.fxtran

-- DROP TABLE public.fxtran;

CREATE TABLE public.fxtran
(
    id bigint NOT NULL,
    ngaygd date,
    ngaygt date,
    bdscha character varying(5),
    cnthien character varying(200),
    gdv character varying(20),
    ksv character varying(20),
    cif integer,
    doitac character varying(500),
    loaigd character varying(20),
    ntegd character varying(3),
    slmua double precision,
    tgmua double precision,
    slban double precision,
    tgban double precision,
    mdnb character varying(10),
    gchu character varying(500),
    loaikh character varying(100),
    dtien2 character varying(3),
    kluqd double precision,
    kluqdm double precision,
    kluqdb double precision,
    tgdu double precision,
    ln2 double precision,
    lnqv double precision,
    mdnb2 character varying(100),
    gchu2 character varying(500),
    mpa character varying(100),
    lhkt character varying(100),
    nnghe character varying(200),
    thang smallint,
    nam integer,
    CONSTRAINT fxtran_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.fxtran
    OWNER to postgres;
COMMENT ON TABLE public.fxtran
    IS 'giao dich fx';

-- comments
COMMENT ON COLUMN public.fxtran.ngaygd
    IS 'ngay giao dich';

COMMENT ON COLUMN public.fxtran.ngaygt
    IS 'ngay gia tri';

COMMENT ON COLUMN public.fxtran.cnthien
    IS 'chi nhanh thuc hien';

COMMENT ON COLUMN public.fxtran.ntegd
    IS 'ngoai te giao dich';

COMMENT ON COLUMN public.fxtran.slmua
    IS 'so luong mua';

COMMENT ON COLUMN public.fxtran.tgmua
    IS 'ty gia mua';

COMMENT ON COLUMN public.fxtran.slban
    IS 'so luong ban';

COMMENT ON COLUMN public.fxtran.tgban
    IS 'ty gia ban';

COMMENT ON COLUMN public.fxtran.mdnb
    IS 'muc dich/ nguon ban';

COMMENT ON COLUMN public.fxtran.gchu
    IS 'ghi chu';

COMMENT ON COLUMN public.fxtran.dtien2
    IS 'dong tien 2';

COMMENT ON COLUMN public.fxtran.kluqd
    IS 'khoi luong usd quy doi';

COMMENT ON COLUMN public.fxtran.kluqdm
    IS 'khoi luong usd quy doi mua';

COMMENT ON COLUMN public.fxtran.kluqdb
    IS 'khoi luong usd quy doi ban';

COMMENT ON COLUMN public.fxtran.tgdu
    IS 'ty gia doi ung';

COMMENT ON COLUMN public.fxtran.ln2
    IS 'loi nhuan theo dong tien 2';

COMMENT ON COLUMN public.fxtran.lnqv
    IS 'loi nhuan quy vnd';

COMMENT ON COLUMN public.fxtran.mdnb2
    IS 'muc dich/ nguon ban 2';

COMMENT ON COLUMN public.fxtran.gchu2
    IS 'ghi chu 2';

COMMENT ON COLUMN public.fxtran.lhkt
    IS 'loai hinh kinh te';

COMMENT ON COLUMN public.fxtran.nnghe
    IS 'nganh nghe';