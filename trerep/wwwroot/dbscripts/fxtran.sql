-- Table: public.fxtran

-- DROP TABLE public.fxtran;

CREATE TABLE public.fxtran
(
    id bigint NOT NULL,
    ngaygd date,
    ngaygt date,
    bdscha character varying(5) COLLATE "default".pg_catalog,
    cnthien character varying(200) COLLATE "default".pg_catalog,
    gdv character varying(20) COLLATE "default".pg_catalog,
    ksv character varying(20) COLLATE "default".pg_catalog,
    cif integer,
    doitac character varying(500) COLLATE "default".pg_catalog,
    loaigd character varying(20) COLLATE "default".pg_catalog,
    ntegd character varying(3) COLLATE "default".pg_catalog,
    slmua double precision,
    tgmua double precision,
    slban double precision,
    tgban double precision,
    mdnb character varying(10) COLLATE "default".pg_catalog,
    gchu character varying(500) COLLATE "default".pg_catalog,
    loaikh character varying(100) COLLATE "default".pg_catalog,
    dtien2 character varying(3) COLLATE "default".pg_catalog,
    kluqd double precision,
    kluqdm double precision,
    kluqdb double precision,
    tgdu double precision,
    ln2 double precision,
    lnqv double precision,
    mdnb2 character varying(100) COLLATE "default".pg_catalog,
    gchu2 character varying(500) COLLATE "default".pg_catalog,
    mpa character varying(100) COLLATE "default".pg_catalog,
    lhkt character varying(100) COLLATE "default".pg_catalog,
    nnghe character varying(200) COLLATE "default".pg_catalog,
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