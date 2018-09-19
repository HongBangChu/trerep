CREATE TABLE public.cust
(
    cif integer NOT NULL,
    cusseg character varying(10),
    cfsic8 character varying(10),
    name character varying(250),
    cifbrn character varying(5),
    incrra character varying(10),
    busine character varying(250),
    addres character varying(250),
    distri character varying(250),
    state character varying(100),
    rm character(20),
    cro character varying(100),
    PRIMARY KEY (cif)
)
WITH (
    OIDS = FALSE
);

ALTER TABLE public.cust
    OWNER to postgres;
COMMENT ON TABLE public.cust
    IS 'customer';

COMMENT ON COLUMN public.cust.cusseg
    IS 'customer segment';

COMMENT ON COLUMN public.cust.cifbrn
    IS 'chi nhanh mo cif';

COMMENT ON COLUMN public.cust.incrra
    IS 'internal credit rating - xep hang tin dung noi bo';

COMMENT ON COLUMN public.cust.busine
    IS 'business - nganh nghe kinh doanh';

COMMENT ON COLUMN public.cust.addres
    IS 'address - dia chi';

COMMENT ON COLUMN public.cust.distri
    IS 'district';

COMMENT ON COLUMN public.cust.cro
    IS 'customer relationship officer - can bo quan he khach hang';