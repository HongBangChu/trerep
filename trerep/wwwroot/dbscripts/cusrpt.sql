-- Table: public.cusrpt

-- DROP TABLE public.cusrpt;

CREATE TABLE public.cusrpt
(
    cif integer,
    fxdoanhso double precision,
    fxloinhuan double precision,
    thang smallint,
    nam integer,
    CONSTRAINT cusrpt_uq UNIQUE (nam, thang, cif)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.cusrpt
    OWNER to postgres;
COMMENT ON TABLE public.cusrpt
    IS 'bao cao khach hang (sheet thong ke chi nhanh)';