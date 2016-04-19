drop  table preregistro
/
create table preregistro(
  numpreregistro number(10,0),
  idcontacto number(10,0),
  idreservacion varchar2(255 byte),
  proposito char,
  idgrupo varchar2(30 byte),
  comoentero varchar2(50 byte),
  fecha date,
  
  --estudiantes y profes
  nomcurso varchar2(50 byte),
  numcurso varchar2(30 byte),
  rolcurso char,
  
  --investigadores
  nomproyecto varchar2(50 byte),
  inversiones varchar2(50 byte),
  fuente varchar2(50 byte),
  resolucion varchar2(30 byte),
  permiso varchar2(30 byte),
  expiracion date   
  
  
);
/
ALTER TABLE PREREGISTRO ADD CONSTRAINT preregistro_pk PRIMARY KEY (numpreregistro);
/
ALTER TABLE PREREGISTRO ADD CONSTRAINT preregistro_contacto_fk FOREIGN KEY (idcontacto) REFERENCES contacto(contacto);
/

drop SEQUENCE PREREGISTRO_SEQ
/
CREATE SEQUENCE PREREGISTRO_SEQ
 START WITH     1
 INCREMENT BY   1
 NOCACHE
 NOCYCLE;
/
CREATE OR REPLACE TRIGGER "PREREGISTRO_TRN" before
INSERT ON "PREREGISTRO" FOR EACH row BEGIN
IF inserting THEN
  IF :NEW."NUMPREREGISTRO" IS NULL THEN
    SELECT PREREGISTRO_SEQ.nextval INTO :NEW."NUMPREREGISTRO" FROM dual;
  END IF;
END IF;
END;
/
ALTER TRIGGER "PREREGISTRO_TRN" ENABLE;