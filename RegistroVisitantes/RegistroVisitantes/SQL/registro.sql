--ESQUEMA: REGISTRO

-----------------------------------------------------------------------------------------------------------------DROPS
--DROP SEQUENCE USUARIO_SEQ;
--DROP TABLE USUARIO;
--DROP TABLE INFOVISITA;
--DROP TABLE PERSONA;
--DROP VIEW V_RESERVACION;

-----------------------------------------------------------------------------------------------------------------USUARIO
CREATE TABLE "USUARIO" (	
	"ID"					NUMBER(32,0) NOT NULL ENABLE, 
	"NOMBRE"				VARCHAR2(40 BYTE), 
	"APELLIDO"				VARCHAR2(40 BYTE), 
	"EMAIL"					VARCHAR2(40 BYTE), 
	"USUAR"					VARCHAR2(40 BYTE) NOT NULL ENABLE, 
	"CONTRASENA"			VARCHAR2(40 BYTE) NOT NULL ENABLE, 
	"ROL"					CHAR NOT NULL ENABLE,
	"IDESTACION"			VARCHAR2(26 BYTE),
	CONSTRAINT "USUARIO_PK" PRIMARY KEY ("ID"),
	CONSTRAINT "USUARIO_ESTACION_FK" FOREIGN KEY (IDESTACION) REFERENCES RESERVAS.ESTACION(ID)
);
/
CREATE SEQUENCE USUARIO_SEQ
  MINVALUE 0 
  START WITH     0
  INCREMENT BY   1
  NOCACHE
  NOCYCLE;
/ 
ALTER SESSION SET PLSCOPE_SETTINGS = 'IDENTIFIERS:NONE';
/
CREATE OR REPLACE TRIGGER "USUARIO_T" 
BEFORE INSERT ON USUARIO FOR EACH ROW
BEGIN
  SELECT USUARIO_SEQ.NEXTVAL INTO :NEW."ID" FROM DUAL;
END;
/
ALTER TRIGGER "USUARIO_T" ENABLE;
/ 
--R=Root, A=Administrador, S=Secretaria
--SII014192819200.2788020223=PaloVerde, SII014192819200.7082987519=LaSelva, SII014548761600.7018216447=CR Office
INSERT INTO USUARIO (NOMBRE,APELLIDO,EMAIL,USUAR,CONTRASENA,ROL,IDESTACION,SEXO) VALUES ('David','Solano','desolanom@gmail.com','dave','dave','R','SII014192819200.7082987519','M');
INSERT INTO USUARIO (NOMBRE,APELLIDO,EMAIL,USUAR,CONTRASENA,ROL,IDESTACION,SEXO) VALUES ('Emmanuel','Arias','emma@gmail.com','emma','emma','R','SII014192819200.7082987519','M');
INSERT INTO USUARIO (NOMBRE,APELLIDO,EMAIL,USUAR,CONTRASENA,ROL,IDESTACION,SEXO) VALUES ('Ang�lica','Fallas','ange@gmail.com','ange','ange','S','SII014192819200.7082987519','F');
INSERT INTO USUARIO (NOMBRE,APELLIDO,EMAIL,USUAR,CONTRASENA,ROL,IDESTACION,SEXO) VALUES ('Daniel','Arias','daniel@gmail.com','daniel','daniel','S','SII014192819200.7082987519','M');
INSERT INTO USUARIO (NOMBRE,APELLIDO,EMAIL,USUAR,CONTRASENA,ROL,IDESTACION,SEXO) VALUES ('Jeffry','Venegas','jeffry@gmail.com','jeffry','jeffry','A','SII014192819200.7082987519','M');
INSERT INTO USUARIO (NOMBRE,APELLIDO,EMAIL,USUAR,CONTRASENA,ROL,IDESTACION,SEXO) VALUES ('Gabriel','Berm�dez','gabriel@g.com','gabriel','gabriel','R','SII014192819200.7082987519','M');
INSERT INTO USUARIO (NOMBRE,APELLIDO,EMAIL,USUAR,CONTRASENA,ROL,IDESTACION,SEXO) VALUES ('Admin','Istrador','admin@gmail.com','admin','admin','A','SII014192819200.2788020223','M');
INSERT INTO USUARIO (NOMBRE,APELLIDO,EMAIL,USUAR,CONTRASENA,ROL,IDESTACION,SEXO) VALUES ('Secre','Taria','secre@gmail.com','secre','secre','S','SII014192819200.7082987519','F');
INSERT INTO USUARIO (NOMBRE,APELLIDO,EMAIL,USUAR,CONTRASENA,ROL,IDESTACION,SEXO) VALUES ('Root','Superusuario','root@gmail.com','root','root','R','SII014548761600.7018216447','M');
/


-----------------------------------------------------------------------------------------------------------------PERSONA
CREATE TABLE "PERSONA" (
  "CEDULA"					VARCHAR2(30 BYTE),
  "NOMBRE"					VARCHAR2(200 BYTE),
  "APELLIDO"				VARCHAR2(200 BYTE),
  "EMAIL"					VARCHAR2(200 BYTE),
  "TELEFONO"				VARCHAR2(150 BYTE),
  "NACIONALIDAD"			VARCHAR2(150 BYTE),
  "PAIS"					VARCHAR2(150 BYTE),
  "ESTADO"					VARCHAR2(150 BYTE),
  "CIUDAD"					VARCHAR2(150 BYTE),
  "DIRECCION"				VARCHAR2(400 BYTE),
  "POSICION"				VARCHAR2(150 BYTE),
  "INSTITUCION"				VARCHAR2(150 BYTE),
  "TITULO"					VARCHAR2(150 BYTE),
  "ROL"						VARCHAR2(150 BYTE),
  "GENERO"					CHAR,
  "COD_POSTAL"				VARCHAR2(150 BYTE),
  CONSTRAINT "PERSONA_PK" PRIMARY KEY ("CEDULA")
);
/
--Tipos de Contacto: 1=Admin, 2=Researcher, 3=Students, 4=Donor, 5=Volunteer, 6=Academic Proffesional, 7=Station Admin, 8=Contactos, 9=Students Staff, 10=Faculty
CREATE OR REPLACE PROCEDURE import_contacto AS
	ced		VARCHAR2(30 BYTE);
	nom		VARCHAR2(200 BYTE);
	ape		VARCHAR2(200 BYTE);
	email	VARCHAR2(200 BYTE);
	tel		VARCHAR2(150 BYTE);
	nacio	VARCHAR2(150 BYTE);
	pais	VARCHAR2(150 BYTE);
	est		VARCHAR2(150 BYTE);
	ciud	VARCHAR2(150 BYTE);
	dir		VARCHAR2(400 BYTE);
	pos		VARCHAR2(150 BYTE);
	inst	VARCHAR2(150 BYTE);
	tit		VARCHAR2(150 BYTE);
	rol		VARCHAR2(150 BYTE);
	gen		CHAR;
	codpos	VARCHAR2(150 BYTE);
	tipo	INTEGER;
	numrep	INTEGER;

	CURSOR completecontacts IS
	SELECT DISTINCT REPLACE(REPLACE(REPLACE("IDENTIFICATION", ' ', ''), '.', ''), '-', '') AS IDENT, "FIRST_NAME", "LAST_NAME", "E_MAIL", "PHONE", "NACIONALITY", ADDRESS_COUNTRY, "STATE", "CITY", CONCAT("ADDRESS_L1", CONCAT(' ',"ADDRESS_L2")) AS "ADDRESS", "POSITION", "INSTITUTION", "ACADEMIC_DEGREE", "TYPE", "GENDER", "ZIP_CODE"
		FROM CONTACTOS.CONTACTO
		WHERE IDENTIFICATION IS NOT NULL AND LENGTH(IDENTIFICATION) <= 30 
		AND UPPER(IDENTIFICATION) NOT LIKE CONCAT('%',CONCAT(TRANSLATE(UPPER(FIRST_NAME), '����������', 'aeiouAEIOU'),'%')) 
		AND UPPER(IDENTIFICATION) NOT LIKE CONCAT('%',CONCAT(TRANSLATE(UPPER(LAST_NAME), '����������', 'aeiouAEIOU'),'%'))
		AND UPPER(IDENTIFICATION) NOT LIKE '%PASSPORT%' AND UPPER(IDENTIFICATION) NOT LIKE '%LICENSE%' AND UPPER(IDENTIFICATION) NOT LIKE '%PASAPORTE%'
		AND UPPER(IDENTIFICATION) NOT LIKE '%LICENCIA%' AND UPPER(IDENTIFICATION) NOT LIKE '%STUDENT%' AND UPPER(IDENTIFICATION) NOT LIKE '%DRIVER%';
        
	BEGIN
		OPEN completecontacts;
		LOOP
			FETCH completecontacts INTO ced,nom,ape,email,tel,nacio,pais,est,ciud,dir,pos,inst,tit,tipo,gen,codpos;
			EXIT WHEN completecontacts%NOTFOUND;

			CASE
				WHEN tipo = 1 THEN rol := 'Staff';
				WHEN tipo = 2 THEN rol := 'Investigador';
				WHEN tipo = 3 THEN rol := 'Acad�mico';
				WHEN tipo = 4 THEN rol := 'Otro';
				WHEN tipo = 5 THEN rol := 'Otro';
				WHEN tipo = 6 THEN rol := 'Acad�mico';
				WHEN tipo = 7 THEN rol := 'Staff';
				WHEN tipo = 8 THEN rol := 'Otro';
				WHEN tipo = 9 THEN rol := 'Acad�mico';
				WHEN tipo = 10 THEN rol := 'Acad�mico';
				ELSE rol := 'Otro';
			END CASE;
      
			SELECT COUNT(*) INTO numrep FROM PERSONA WHERE CEDULA=ced;
			IF numrep>0 THEN
				numrep:=0;
			ELSE
				INSERT INTO PERSONA ("CEDULA", "NOMBRE", "APELLIDO", "EMAIL", "TELEFONO", "NACIONALIDAD", "PAIS", "ESTADO", "CIUDAD", "DIRECCION", "POSICION", "INSTITUCION", "TITULO", "ROL", "GENERO", "COD_POSTAL")
					VALUES			(ced,      nom,      ape,        email,   tel,        nacio,          pais,   est,      ciud,     dir,         pos,        inst,          tit,      rol,   gen,      codpos);
			END IF;
      
		END LOOP;
	END;
/
--Todos son acad�micos
CREATE OR REPLACE PROCEDURE import_student AS
	ced		VARCHAR2(30 BYTE);
	nom		VARCHAR2(200 BYTE);
	ape		VARCHAR2(200 BYTE);
	email	VARCHAR2(200 BYTE);
	tel		VARCHAR2(150 BYTE);
	nacio	VARCHAR2(150 BYTE);
	pais	VARCHAR2(150 BYTE);
	est		VARCHAR2(150 BYTE);
	ciud	VARCHAR2(150 BYTE);
	dir		VARCHAR2(400 BYTE);
	pos		VARCHAR2(150 BYTE);
	inst	VARCHAR2(150 BYTE);
	tit		VARCHAR2(150 BYTE);
	rol		VARCHAR2(150 BYTE);
	gen		CHAR;
	codpos	VARCHAR2(150 BYTE);
	tipo	INTEGER;
	numrep	INTEGER;

	CURSOR completestudents IS
	SELECT DISTINCT REPLACE(REPLACE(REPLACE("S"."IDENTIFICATION", ' ', ''), '.', ''), '-', '') AS IDENT, "S"."FIRST_NAME", "LAST_NAME", "E_MAIL", "PHONE_NUMBER", "NACIONALITY", "ADDRESS_COUNTRY", "ADDRESS_STATE", "ADDRESS_CITY", CONCAT("ADDRESS_L1", CONCAT(' ',"ADDRESS_L2")) AS "ADDRESS", "I"."DESCRIPCION" AS "INSTITUTION", "D"."MAJOR_FIELD" AS "ACADEMIC_DEGREE", "GENDER", "ADDRESS_ZIP"
		FROM CONTACTOS.STUDENT "S", CONTACTOS.STUDENT_ACADEMIC_DEGREE "D", CONTACTOS.CAT_INSTITUCION "I"
		WHERE "S"."STUDENT" = "D"."STUDENT" AND "S"."CAT_INSTITUCION" = "I"."CAT_INSTITUCION" AND
    IDENTIFICATION IS NOT NULL AND LENGTH(IDENTIFICATION) <= 30 
		AND UPPER(IDENTIFICATION) NOT LIKE CONCAT('%',CONCAT(TRANSLATE(UPPER(FIRST_NAME), '����������', 'aeiouAEIOU'),'%')) 
		AND UPPER(IDENTIFICATION) NOT LIKE CONCAT('%',CONCAT(TRANSLATE(UPPER(LAST_NAME), '����������', 'aeiouAEIOU'),'%'))
		AND UPPER(IDENTIFICATION) NOT LIKE '%PASSPORT%' AND UPPER(IDENTIFICATION) NOT LIKE '%LICENSE%' AND UPPER(IDENTIFICATION) NOT LIKE '%PASAPORTE%'
		AND UPPER(IDENTIFICATION) NOT LIKE '%LICENCIA%' AND UPPER(IDENTIFICATION) NOT LIKE '%STUDENT%' AND UPPER(IDENTIFICATION) NOT LIKE '%DRIVER%';
        
	BEGIN
		OPEN completestudents;
		LOOP
			FETCH completestudents INTO ced,nom,ape,email,tel,nacio,pais,est,ciud,dir,inst,tit,gen,codpos;
			EXIT WHEN completestudents%NOTFOUND;

			rol := 'Acad�mico';
      
			SELECT COUNT(*) INTO numrep FROM PERSONA WHERE CEDULA=ced;
			IF numrep>0 THEN
				numrep:=0;
			ELSE
				INSERT INTO PERSONA ("CEDULA", "NOMBRE", "APELLIDO", "EMAIL", "TELEFONO", "NACIONALIDAD", "PAIS", "ESTADO", "CIUDAD", "DIRECCION", "POSICION", "INSTITUCION", "TITULO", "ROL", "GENERO", "COD_POSTAL")
					VALUES			(ced,      nom,      ape,        email,   tel,        nacio,          pais,   est,      ciud,     dir,         'Student',        inst,          tit,      rol,   gen,      codpos);
			END IF;
      
		END LOOP;
	END;
/
BEGIN
  import_contacto();
  import_student();
END;
/


-----------------------------------------------------------------------------------------------------------------INFOVISITA
CREATE OR REPLACE FORCE VIEW "REGISTRO"."V_RESERVACION" ("ID", "ANFITRIONA", "NUMERO", "ESTADO", "PRIORIDAD", "GRUPO", "ENTRA", 
  "ENTRAMTN", "SALE", "SALEMTN", "RESPONSABLE", "SOLICITANTE", "NOTAS", "SOLICITADAEL", "FORMAPAGO", "SALDO", "CUENTACLIENTEKEY", 
  "CUENTACLIENTEOCLASS", "ESTACION", "MODIFICADOR", "MODIFICADO", "MONTO_PREPAGO", "REFERENCIA_PREPAGO", "PRIMERA_COMIDA", "CREACION", 
  "ULTIMA_MODIFICACION", "MODIFICA_RACK", "MODIFICADO_RACK", "FLAG", "URL", "TIPO",
  CONSTRAINT "V_RESERVACION_PK" PRIMARY KEY ("ID") DISABLE) AS 
  SELECT
    "ID","ANFITRIONA","NUMERO","ESTADO","PRIORIDAD","GRUPO","ENTRA","ENTRAMTN","SALE","SALEMTN","RESPONSABLE","SOLICITANTE","NOTAS","SOLICITADAEL","FORMAPAGO","SALDO","CUENTACLIENTEKEY","CUENTACLIENTEOCLASS","ESTACION","MODIFICADOR","MODIFICADO","MONTO_PREPAGO","REFERENCIA_PREPAGO","PRIMERA_COMIDA","CREACION","ULTIMA_MODIFICACION","MODIFICA_RACK","MODIFICADO_RACK","FLAG","URL","TIPO"
  FROM
    RESERVAS.RESERVACION;
    
CREATE TABLE "INFOVISITA" (
----DATOS DEL CONTACTO
	"ID_RESERVACION"		VARCHAR2(255 BYTE),
	"CEDULA"				VARCHAR2(30 BYTE),
	"NOMBRE_EMERGENCIA"		VARCHAR2(400 BYTE), 
	"TEL_EMERGENCIA"		VARCHAR2(150 BYTE), 
	"EMAIL_EMERGENCIA"		VARCHAR2(150 BYTE), 
	"REL_EMERGENCIA"		VARCHAR2(150 BYTE), 
	"COMIDA"				VARCHAR2(300 BYTE), 
	"CARNE"					NUMBER(1,0),
	"POLLO"					NUMBER(1,0),
	"PESCADO"				NUMBER(1,0),
	"CERDO"					NUMBER(1,0),
	"DIETA"					VARCHAR2(150 BYTE),
	"OBSERVACIONES_DIETA"	VARCHAR2(150 BYTE),
	"ALERGIAS"				VARCHAR2(150 BYTE),
	"PROPOSITO"				VARCHAR2(150 BYTE), 
	"OTRO_PROPOSITO"		VARCHAR2(150 BYTE), 
	"COMO_ENTERO"			VARCHAR2(150 BYTE),
	"FECHA"					DATE,
	"ESTADO"				NUMBER(1,0) DEFAULT 1,
----ESTUDIANTES Y PROFES
	"NOMBRE_CURSO"			VARCHAR2(50 BYTE),
	"NUMERO_CURSO"			VARCHAR2(30 BYTE),
	"ROL_CURSO"				VARCHAR2(20 BYTE) ,
----"INVESTIGADORES
	"NOMBRE_PROYECTO"		VARCHAR2(50 BYTE),
	"INVERSIONES"			VARCHAR2(50 BYTE),
	"FUENTE"				VARCHAR2(50 BYTE),
	"RESOLUCION"			VARCHAR2(30 BYTE),
	"PERMISO"				VARCHAR2(30 BYTE),
	"EXPIRACION"			DATE,
	CONSTRAINT "INFOVISITA_PK" PRIMARY KEY ("CEDULA","ID_RESERVACION"),
	CONSTRAINT "INFOVISITA_RESERVA_FK" FOREIGN KEY ("ID_RESERVACION") REFERENCES RESERVAS.RESERVACION(ID),
	CONSTRAINT "INFOVISITA_PERSONA_FK" FOREIGN KEY ("CEDULA") REFERENCES PERSONA(CEDULA)
);
/


-----------------------------------------------------------------------------------------------------------------SELECTS
--SELECT * FROM "REGISTRO"."USUARIO";
--SELECT * FROM PERSONA;
--SELECT * FROM INFOVISITA;
--SELECT * FROM RESERVACION;
--SELECT * FROM V_ANFITRIONA;
--SELECT * FROM V_RESERVACION;
--SELECT * FROM V_ESTACION;
--SELECT * FROM USUARIO;
--SELECT DATA_DEFAULT FROM ALL_TAB_COLUMNS WHERE TABLE_NAME='INFOVISITA' AND COLUMN_NAME='ESTADO';