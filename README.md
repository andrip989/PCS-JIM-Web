-- public.cashiertrans definition

-- Drop table

-- DROP TABLE public.cashiertrans;

CREATE TABLE public.cashiertrans (
	userid varchar NULL,
	tanggalopen timestamp NULL,
	saldoawal money NULL,
	status int2 NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	saldoakhir money NULL,
	tanggalclose timestamp NULL,
	CONSTRAINT cashiertrans_pk PRIMARY KEY (recid)
);


-- public.housekeepingroom definition

-- Drop table

-- DROP TABLE public.housekeepingroom;

CREATE TABLE public.housekeepingroom (
	noroom varchar NULL,
	arrival timestamp NULL,
	departure timestamp NULL,
	transid varchar NULL,
	status int2 NULL,
	createddate timestamp NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	keterangan varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	CONSTRAINT housekeepingroom_pk PRIMARY KEY (recid)
);
CREATE UNIQUE INDEX housekeepingroom_transid_idx ON public.housekeepingroom USING btree (transid);


-- public.jumlahdata definition

-- Drop table

-- DROP TABLE public.jumlahdata;

CREATE TABLE public.jumlahdata (
	count int8 NULL
);


-- public.logdaily definition

-- Drop table

-- DROP TABLE public.logdaily;

CREATE TABLE public.logdaily (
	datetime timestamp NULL,
	outletno varchar NULL,
	"current" numeric NULL,
	voltage numeric NULL,
	realpower numeric NULL,
	reactivepower numeric NULL,
	apparentpower numeric NULL,
	"5minpowerconsumption" numeric NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	deviceid varchar NULL,
	subdeviceid varchar NULL,
	createdby varchar NULL,
	CONSTRAINT logdaily_pk PRIMARY KEY (recid)
);


-- public.maintenancetype definition

-- Drop table

-- DROP TABLE public.maintenancetype;

CREATE TABLE public.maintenancetype (
	"type" varchar NULL,
	description varchar NULL,
	"period" int2 DEFAULT 30 NULL,
	createddate timestamp NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	active int2 DEFAULT 1 NULL,
	CONSTRAINT maintenancetype_pk PRIMARY KEY (recid)
);


-- public.maitenanceroom definition

-- Drop table

-- DROP TABLE public.maitenanceroom;

CREATE TABLE public.maitenanceroom (
	noroom varchar NULL,
	arrival timestamp NULL,
	departure timestamp NULL,
	transid varchar NULL,
	status int2 NULL,
	createddate timestamp NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	keterangan varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	controltype varchar NULL,
	CONSTRAINT maitenanceroom_pk PRIMARY KEY (recid)
);
CREATE INDEX maitenanceroom_transid_idx ON public.maitenanceroom USING btree (transid);


-- public.setupbusinesssources definition

-- Drop table

-- DROP TABLE public.setupbusinesssources;

CREATE TABLE public.setupbusinesssources (
	alias varchar NULL,
	companyname varchar NOT NULL,
	firstname varchar NULL,
	lastname varchar NULL,
	description varchar NULL,
	address varchar NULL,
	address2 varchar NULL,
	city varchar NULL,
	postalcode varchar NULL,
	state varchar NULL,
	country varchar NULL,
	phone varchar NULL,
	fax varchar NULL,
	email varchar NULL,
	website varchar NULL,
	creditcardtype varchar NULL,
	cardholder varchar NULL,
	creditcardno varchar NULL,
	ccexpdate timestamp NULL,
	iatano varchar NULL,
	regno varchar NULL,
	regno1 varchar NULL,
	regno2 varchar NULL,
	acnumber varchar NULL,
	definespecialseason int2 NULL,
	definespecialrate int2 NULL,
	plantype varchar NULL,
	valueplan numeric NULL,
	term int4 NULL,
	createddate timestamp DEFAULT now()::timestamp without time zone NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	marketplace varchar NULL,
	CONSTRAINT setupbusinesssources_pk PRIMARY KEY (companyname)
);


-- public.setupdevice definition

-- Drop table

-- DROP TABLE public.setupdevice;

CREATE TABLE public.setupdevice (
	deviceid varchar NOT NULL,
	description varchar NULL,
	createddate date NULL,
	recid int4 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE) NOT NULL,
	createdby varchar NULL,
	createdtime timestamp NULL,
	ipaddress varchar NULL,
	ipport varchar NULL,
	updatedatetime timestamp NULL,
	updatedby varchar NULL,
	ipaddresslan varchar NULL,
	ipaddresswifi varchar NULL,
	tipeipaddress int2 DEFAULT 0 NULL,
	CONSTRAINT setupdevice_pk PRIMARY KEY (deviceid, recid)
);


-- public.setupfloor definition

-- Drop table

-- DROP TABLE public.setupfloor;

CREATE TABLE public.setupfloor (
	floortype varchar NOT NULL,
	description varchar NULL,
	createddate timestamp NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	CONSTRAINT setupfloor_pk PRIMARY KEY (floortype),
	CONSTRAINT setupfloor_unique UNIQUE (recid)
);

-- Table Triggers

create trigger floor_changes before
update
    on
    public.setupfloor for each row execute function floortype_changed();


-- public.setupguestlist definition

-- Drop table

-- DROP TABLE public.setupguestlist;

CREATE TABLE public.setupguestlist (
	custcode varchar NOT NULL,
	firstname varchar NULL,
	lastname varchar NULL,
	identificationid varchar NULL,
	phone varchar NULL,
	email varchar NULL,
	lasttransaksiid varchar NULL,
	discountcode varchar NULL,
	discpct numeric NULL,
	npwp varchar NULL,
	telephone varchar NULL,
	createddate timestamp NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	businesscode varchar NULL,
	paymenttype varchar NULL,
	creditcardno varchar NULL,
	creditlimit money NULL,
	statusblock varchar NOT NULL,
	address varchar NULL,
	picprofile varchar NULL,
	street varchar NULL,
	city varchar NULL,
	state varchar NULL,
	zipcode varchar NULL,
	country varchar NULL,
	guesttype varchar NULL,
	regno varchar NULL,
	identificationtype varchar NULL,
	identificationexpdate timestamp NULL,
	vehiclelicenseplate varchar NULL,
	vehiclecolor varchar NULL,
	vehiclecompany varchar NULL,
	vehiclemodel varchar NULL,
	vehicleyear int4 NULL,
	maritalstatus varchar NULL,
	anniversarydate timestamp NULL,
	children int4 NULL,
	spousebirthdate timestamp NULL,
	education varchar NULL,
	occupation varchar NULL,
	income money NULL,
	nationality varchar NULL,
	language1 varchar NULL,
	language2 varchar NULL,
	allergies varchar NULL,
	bloodtype varchar NULL,
	officeadd varchar NULL,
	office varchar NULL,
	residential varchar NULL,
	fax varchar NULL,
	website varchar NULL,
	followup varchar NULL,
	heardfrom varchar NULL,
	denial varchar NULL,
	birthdate timestamp NULL,
	gender varchar NULL,
	ktpprofile varchar NULL,
	CONSTRAINT mastercustomer_pk PRIMARY KEY (custcode)
);


-- public.setupoutlet definition

-- Drop table

-- DROP TABLE public.setupoutlet;

CREATE TABLE public.setupoutlet (
	outletno varchar NOT NULL,
	deviceid varchar NOT NULL,
	subdeviceid varchar NOT NULL,
	createddate date NULL,
	createdtime timestamp NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	description varchar NULL,
	createdby varchar NULL,
	updatedatetime timestamp NULL,
	updatedby varchar NULL,
	CONSTRAINT setupoutlet_pk PRIMARY KEY (outletno, recid, subdeviceid, deviceid)
);


-- public.setuppaymenttype definition

-- Drop table

-- DROP TABLE public.setuppaymenttype;

CREATE TABLE public.setuppaymenttype (
	paymenttype varchar NULL,
	description varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	active int2 NULL,
	createddate timestamp NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	CONSTRAINT setuppaymenttype_pk PRIMARY KEY (recid)
);


-- public.setupratetype definition

-- Drop table

-- DROP TABLE public.setupratetype;

CREATE TABLE public.setupratetype (
	ratetype varchar NOT NULL,
	keterangan varchar NULL,
	"hour" int2 DEFAULT 0 NOT NULL,
	"day" int2 DEFAULT 0 NOT NULL,
	createddate timestamp NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	CONSTRAINT setupratetype_pk PRIMARY KEY (ratetype)
);


-- public.setuproom definition

-- Drop table

-- DROP TABLE public.setuproom;

CREATE TABLE public.setuproom (
	outletno varchar NULL,
	noroom varchar NULL,
	description varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	createddatetime timestamp NULL,
	createdby varchar NULL,
	updateddatetime timestamp NULL,
	updatedby varchar NULL,
	numbersequencetrans varchar NULL,
	refrecidoutlet int8 NULL,
	roomrate money NULL,
	active int2 DEFAULT 1 NULL,
	statushousekeeping int2 DEFAULT 0 NOT NULL,
	statusmaintenance int2 DEFAULT 0 NOT NULL,
	statusroom varchar DEFAULT 'off'::character varying NOT NULL,
	roomtypes varchar NULL,
	phoneext varchar NULL,
	dataext varchar NULL,
	keycard varchar NULL,
	powercode varchar NULL,
	roomamenities varchar NULL,
	housekeepingday varchar NULL,
	allowsmoking int2 NULL,
	floortype varchar NULL,
	lockprice int2 NULL,
	fixedrate int2 NULL,
	CONSTRAINT setuproom_pk PRIMARY KEY (recid)
);
CREATE INDEX setuproom_noroom_idx ON public.setuproom USING btree (noroom);


-- public.setuproomamenities definition

-- Drop table

-- DROP TABLE public.setuproomamenities;

CREATE TABLE public.setuproomamenities (
	roomamenities varchar NOT NULL,
	description varchar NULL,
	createddate timestamp DEFAULT now()::timestamp without time zone NOT NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	CONSTRAINT setuproomamenities_pk PRIMARY KEY (roomamenities)
);


-- public.setuproomtariff definition

-- Drop table

-- DROP TABLE public.setuproomtariff;

CREATE TABLE public.setuproomtariff (
	roomtypes varchar NULL,
	ratetypes varchar NULL,
	bussinesssource varchar NULL,
	seasontype varchar NULL,
	custcode varchar NULL,
	tariff money NULL,
	extraadult money NULL,
	extrachild money NULL,
	usetax int2 NULL,
	active int2 NULL,
	createddate timestamp DEFAULT now()::timestamp without time zone NOT NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	floortype varchar NULL,
	noroom varchar NULL,
	startdate timestamp NULL,
	CONSTRAINT setuproomtariff_unique UNIQUE (recid)
);
CREATE UNIQUE INDEX setuproomtariff_roomtypes_idx ON public.setuproomtariff USING btree (roomtypes, ratetypes, bussinesssource, seasontype, custcode, active, startdate);


-- public.setuproomtypes definition

-- Drop table

-- DROP TABLE public.setuproomtypes;

CREATE TABLE public.setuproomtypes (
	roomtypes varchar NOT NULL,
	keterangan varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	createddate timestamp NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	adultmax int4 NULL,
	childmax int4 NULL,
	overbooking numeric NULL,
	defaultrate numeric NULL,
	defaultadultrate numeric NULL,
	defaultchildrate numeric NULL,
	baseadult int4 NULL,
	basechild int4 NULL,
	CONSTRAINT setuproomtypes_pk PRIMARY KEY (roomtypes)
);

-- Table Triggers

create trigger roomtypes_changes before
update
    on
    public.setuproomtypes for each row execute function roomtypes_changes();


-- public.setupseasontype definition

-- Drop table

-- DROP TABLE public.setupseasontype;

CREATE TABLE public.setupseasontype (
	seasontype varchar NOT NULL,
	description varchar NULL,
	fromday int4 NULL,
	frommonth int4 NULL,
	today int4 NULL,
	tomonth int4 NULL,
	yearperiod int4 NULL,
	active int2 NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	createddate timestamp DEFAULT now()::timestamp without time zone NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	CONSTRAINT setupseasontype_pk PRIMARY KEY (seasontype),
	CONSTRAINT setupseasontype_unique UNIQUE (recid)
);


-- public.setupsubdevice definition

-- Drop table

-- DROP TABLE public.setupsubdevice;

CREATE TABLE public.setupsubdevice (
	subdeviceid varchar NOT NULL,
	description varchar NULL,
	createddate date NULL,
	createdtime timestamp NULL,
	createdby varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	deviceid varchar NULL,
	updatedatetime timestamp NULL,
	updatedby varchar NULL,
	CONSTRAINT setupsubdevice_pk PRIMARY KEY (subdeviceid, recid)
);


-- public.setuptax definition

-- Drop table

-- DROP TABLE public.setuptax;

CREATE TABLE public.setuptax (
	startdate timestamp NOT NULL,
	fromamount money NULL,
	toamount money NULL,
	tax1 numeric NULL,
	tax2 numeric NULL,
	tax3 numeric NULL,
	tax4 numeric NULL,
	tax1amount money NULL,
	tax2amount money NULL,
	tax3amount money NULL,
	tax4amount money NULL,
	createddate timestamp DEFAULT now()::timestamp without time zone NOT NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	CONSTRAINT setuptax_pk PRIMARY KEY (startdate),
	CONSTRAINT setuptax_unique UNIQUE (recid)
);


-- public.sysreportrunner definition

-- Drop table

-- DROP TABLE public.sysreportrunner;

CREATE TABLE public.sysreportrunner (
	reportname varchar NULL,
	rptcriteria varchar NULL,
	rptparameter varchar NULL,
	reporttitle varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	createddate timestamp DEFAULT now() NULL,
	CONSTRAINT sysreportrunner_pk PRIMARY KEY (recid)
);


-- public.syssession definition

-- Drop table

-- DROP TABLE public.syssession;

CREATE TABLE public.syssession (
	sessionid varchar NULL,
	username varchar NULL,
	logindatetime timestamp NULL
);


-- public.sysuser definition

-- Drop table

-- DROP TABLE public.sysuser;

CREATE TABLE public.sysuser (
	username varchar NULL,
	email varchar NULL,
	"password" varchar NULL,
	createddatetime timestamp NULL,
	lastlogindatetime timestamp NULL,
	fullname varchar NULL,
	picprofile bytea NULL,
	usergroupid varchar DEFAULT 'USER'::character varying NULL,
	opencashier int4 DEFAULT 0 NOT NULL
);
CREATE UNIQUE INDEX sysuser_username_idx ON public.sysuser USING btree (username);


-- public.transaksidetailhotspot definition

-- Drop table

-- DROP TABLE public.transaksidetailhotspot;

CREATE TABLE public.transaksidetailhotspot (
	noroom varchar NULL,
	ssid varchar NULL,
	username varchar NULL,
	"password" varchar NULL,
	checkin timestamp NULL,
	checkout timestamp NULL,
	createddate timestamp DEFAULT now() NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	status int2 DEFAULT 0 NULL,
	reftransid varchar NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	CONSTRAINT transaksidetailhotspot_pk PRIMARY KEY (recid)
);


-- public.transaksiroom definition

-- Drop table

-- DROP TABLE public.transaksiroom;

CREATE TABLE public.transaksiroom (
	noroom varchar NOT NULL,
	refrecidroom int8 NULL,
	arrival timestamp NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	createddatetime timestamp NULL,
	createdby varchar NULL,
	updatedatetime timestamp NULL,
	updatedby varchar NULL,
	jumlahadult int4 NULL,
	status int2 NULL,
	departure timestamp NULL,
	transaksiid varchar NOT NULL,
	keterangantambahan varchar NULL,
	custcode varchar NULL,
	refbookingcode varchar NULL,
	actualcheckin timestamp NULL,
	actualcheckout timestamp NULL,
	checkoutby varchar NULL,
	seasontype varchar NULL,
	ratetype varchar NULL,
	usetax1 int2 NULL,
	usetax2 int2 NULL,
	usetax3 int2 NULL,
	usetax4 int2 NULL,
	totalcharges money NULL,
	discount money NULL,
	totaltax money NULL,
	total money NULL,
	flatdiscount money NULL,
	amountpaid money NULL,
	deposit money NULL,
	roundoff money NULL,
	balance money NULL,
	extracharges money NULL,
	totalrate money NULL,
	jumlahchild int4 NULL,
	paymenttype varchar NULL,
	manualcheckin int2 DEFAULT 0 NULL,
	tipetrans varchar NULL,
	settlepaydeposit money DEFAULT 0 NULL,
	closebalance money DEFAULT 0 NULL,
	closedate timestamp NULL,
	closeby varchar NULL,
	exportstatus int2 DEFAULT 0 NULL,
	journalidax varchar NULL,
	settletxt varchar NULL,
	bookingsource varchar NULL,
	CONSTRAINT transaksiroom_unique UNIQUE (transaksiid)
);


-- public.transaksiroomratedetail definition

-- Drop table

-- DROP TABLE public.transaksiroomratedetail;

CREATE TABLE public.transaksiroomratedetail (
	transaksiid varchar NULL,
	tanggal timestamp NULL,
	totalcharges money NULL,
	discount money NULL,
	totaltax money NULL,
	total money NULL,
	ratetypes varchar NULL,
	seasontype varchar NULL,
	extraadult money NULL,
	extrachild money NULL,
	recid int8 GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 9223372036854775807 START 1 CACHE 1 NO CYCLE) NOT NULL,
	createddate timestamp NULL,
	createdby varchar NULL,
	updateddate timestamp NULL,
	updatedby varchar NULL,
	CONSTRAINT transaksiroomratedetail_pk PRIMARY KEY (recid)
);
