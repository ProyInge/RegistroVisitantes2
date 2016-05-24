-- --------------------------------------------------
-- Entity Designer DDL Script for Oracle database
-- --------------------------------------------------
-- Date Created: 23/05/2016 23:22:15
-- Generated from EDMX file: C:\Users\dsola\Source\Repos\RegistroVisitantes2\RegistroVisitantes\RegistroVisitantes\Models\registro.edmx
-- --------------------------------------------------

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

-- ALTER TABLE "REGISTRO"."INFOVISITA" DROP CONSTRAINT "FK_INFOVISITA_PERSONA_FK" CASCADE;
-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

-- DROP TABLE "REGISTRO"."INFOVISITA";

-- DROP TABLE "REGISTRO"."PERSONA";

-- DROP TABLE "REGISTRO"."USUARIO";

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'PERSONA'
CREATE TABLE "REGISTRO"."PERSONA" (
   "CEDULA" VARCHAR2(30 CHAR) NOT NULL,
   "NOMBRE" VARCHAR2(200 CHAR) ,
   "APELLIDO" VARCHAR2(200 CHAR) ,
   "EMAIL" VARCHAR2(200 CHAR) ,
   "TELEFONO" VARCHAR2(150 CHAR) ,
   "NACIONALIDAD" VARCHAR2(150 CHAR) ,
   "PAIS" VARCHAR2(150 CHAR) ,
   "ESTADO" VARCHAR2(150 CHAR) ,
   "CIUDAD" VARCHAR2(150 CHAR) ,
   "DIRECCION" VARCHAR2(400 CHAR) ,
   "POSICION" VARCHAR2(150 CHAR) ,
   "INSTITUCION" VARCHAR2(150 CHAR) ,
   "TITULO" VARCHAR2(150 CHAR) ,
   "ROL" VARCHAR2(150 CHAR) ,
   "GENERO" CHAR(1 CHAR) ,
   "COD_POSTAL" VARCHAR2(150 CHAR) 
);

-- Creating table 'USUARIO'
CREATE TABLE "REGISTRO"."USUARIO" (
   "ID" NUMBER(32) NOT NULL,
   "NOMBRE" VARCHAR2(40 CHAR) ,
   "APELLIDO" VARCHAR2(40 CHAR) ,
   "EMAIL" VARCHAR2(40 CHAR) ,
   "USUAR" VARCHAR2(40 CHAR) NOT NULL,
   "CONTRASENA" VARCHAR2(40 CHAR) NOT NULL,
   "ROL" CHAR(1 CHAR) NOT NULL,
   "IDESTACION" VARCHAR2(26 CHAR) NOT NULL,
   "SEXO" CHAR(1 CHAR) 
);

-- Creating table 'V_ESTACION'
CREATE TABLE "REGISTRO"."V_ESTACION" (
   "ID" VARCHAR2(26 CHAR) NOT NULL,
   "NOMBRE" VARCHAR2(100 CHAR) NOT NULL,
   "SIGLAS" VARCHAR2(20 CHAR) ,
   "RESERVABLE" VARCHAR2(1 CHAR) 
);

-- Creating table 'V_RESERVACION'
CREATE TABLE "REGISTRO"."V_RESERVACION" (
   "ID" VARCHAR2(255 CHAR) NOT NULL,
   "ANFITRIONA" VARCHAR2(255 CHAR) ,
   "NUMERO" VARCHAR2(255 CHAR) NOT NULL,
   "ESTADO" VARCHAR2(255 CHAR) ,
   "PRIORIDAD" NUMBER(20) ,
   "GRUPO" VARCHAR2(255 CHAR) ,
   "ENTRA" DATE ,
   "ENTRAMTN" VARCHAR2(255 CHAR) ,
   "SALE" DATE ,
   "SALEMTN" VARCHAR2(255 CHAR) ,
   "RESPONSABLE" VARCHAR2(255 CHAR) ,
   "SOLICITANTE" VARCHAR2(255 CHAR) ,
   "NOTAS" VARCHAR2(4000 CHAR) ,
   "SOLICITADAEL" DATE ,
   "FORMAPAGO" VARCHAR2(255 CHAR) ,
   "SALDO" NUMBER(20,5) ,
   "CUENTACLIENTEKEY" VARCHAR2(255 CHAR) ,
   "CUENTACLIENTEOCLASS" VARCHAR2(255 CHAR) ,
   "ESTACION" VARCHAR2(26 CHAR) ,
   "MODIFICADOR" VARCHAR2(255 CHAR) ,
   "MODIFICADO" DATE ,
   "MONTO_PREPAGO" NUMBER(15,2) ,
   "REFERENCIA_PREPAGO" VARCHAR2(1000 CHAR) ,
   "PRIMERA_COMIDA" VARCHAR2(50 CHAR) ,
   "CREACION" NUMBER(19) ,
   "ULTIMA_MODIFICACION" NUMBER(19) ,
   "MODIFICA_RACK" VARCHAR2(255 CHAR) ,
   "MODIFICADO_RACK" VARCHAR2(255 CHAR) ,
   "FLAG" NUMBER(3) ,
   "URL" VARCHAR2(2000 CHAR) ,
   "TIPO" VARCHAR2(2 CHAR) 
);

-- Creating table 'INFOVISITA'
CREATE TABLE "REGISTRO"."INFOVISITA" (
   "ID_RESERVACION" VARCHAR2(255 CHAR) NOT NULL,
   "CEDULA" VARCHAR2(30 CHAR) NOT NULL,
   "NOMBRE_EMERGENCIA" VARCHAR2(400 CHAR) ,
   "TEL_EMERGENCIA" VARCHAR2(150 CHAR) ,
   "EMAIL_EMERGENCIA" VARCHAR2(150 CHAR) ,
   "REL_EMERGENCIA" VARCHAR2(150 CHAR) ,
   "COMIDA" VARCHAR2(300 CHAR) ,
   "CARNE" NUMBER(1) ,
   "POLLO" NUMBER(1) ,
   "PESCADO" NUMBER(1) ,
   "CERDO" NUMBER(1) ,
   "DIETA" VARCHAR2(150 CHAR) ,
   "OBSERVACIONES_DIETA" VARCHAR2(150 CHAR) ,
   "ALERGIAS" VARCHAR2(150 CHAR) ,
   "PROPOSITO" VARCHAR2(150 CHAR) ,
   "OTRO_PROPOSITO" VARCHAR2(150 CHAR) ,
   "COMO_ENTERO" VARCHAR2(150 CHAR) ,
   "FECHA" DATE ,
   "NOMBRE_CURSO" VARCHAR2(50 CHAR) ,
   "NUMERO_CURSO" VARCHAR2(30 CHAR) ,
   "ROL_CURSO" VARCHAR2(20 CHAR) ,
   "NOMBRE_PROYECTO" VARCHAR2(50 CHAR) ,
   "INVERSIONES" VARCHAR2(50 CHAR) ,
   "FUENTE" VARCHAR2(50 CHAR) ,
   "RESOLUCION" VARCHAR2(30 CHAR) ,
   "PERMISO" VARCHAR2(30 CHAR) ,
   "EXPIRACION" DATE ,
   "ESTADO" NUMBER(1) 
);


-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on "CEDULA"in table 'PERSONA'
ALTER TABLE "REGISTRO"."PERSONA"
ADD CONSTRAINT "PK_PERSONA"
   PRIMARY KEY ("CEDULA" )
   ENABLE
   VALIDATE;


-- Creating primary key on "ID"in table 'USUARIO'
ALTER TABLE "REGISTRO"."USUARIO"
ADD CONSTRAINT "PK_USUARIO"
   PRIMARY KEY ("ID" )
   ENABLE
   VALIDATE;


-- Creating primary key on "ID"in table 'V_ESTACION'
ALTER TABLE "REGISTRO"."V_ESTACION"
ADD CONSTRAINT "PK_V_ESTACION"
   PRIMARY KEY ("ID" )
   ENABLE
   VALIDATE;


-- Creating primary key on "ID"in table 'V_RESERVACION'
ALTER TABLE "REGISTRO"."V_RESERVACION"
ADD CONSTRAINT "PK_V_RESERVACION"
   PRIMARY KEY ("ID" )
   ENABLE
   VALIDATE;


-- Creating primary key on "ID_RESERVACION", "CEDULA"in table 'INFOVISITA'
ALTER TABLE "REGISTRO"."INFOVISITA"
ADD CONSTRAINT "PK_INFOVISITA"
   PRIMARY KEY ("ID_RESERVACION", "CEDULA" )
   ENABLE
   VALIDATE;


-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on "IDESTACION" in table 'USUARIO'
ALTER TABLE "REGISTRO"."USUARIO"
ADD CONSTRAINT "FK_USUARIOV_ESTACION"
   FOREIGN KEY ("IDESTACION")
   REFERENCES "REGISTRO"."V_ESTACION"
       ("ID")
   ENABLE
   VALIDATE;

-- Creating index for FOREIGN KEY 'FK_USUARIOV_ESTACION'
CREATE INDEX "IX_FK_USUARIOV_ESTACION"
ON "REGISTRO"."USUARIO"
   ("IDESTACION");

-- Creating foreign key on "CEDULA" in table 'INFOVISITA'
ALTER TABLE "REGISTRO"."INFOVISITA"
ADD CONSTRAINT "FK_INFOVISITA_PERSONA_FK"
   FOREIGN KEY ("CEDULA")
   REFERENCES "REGISTRO"."PERSONA"
       ("CEDULA")
   ENABLE
   VALIDATE;

-- Creating index for FOREIGN KEY 'FK_INFOVISITA_PERSONA_FK'
CREATE INDEX "IX_FK_INFOVISITA_PERSONA_FK"
ON "REGISTRO"."INFOVISITA"
   ("CEDULA");

-- Creating foreign key on "ID_RESERVACION" in table 'INFOVISITA'
ALTER TABLE "REGISTRO"."INFOVISITA"
ADD CONSTRAINT "FK_INFOVISITAV_RESERVACION"
   FOREIGN KEY ("ID_RESERVACION")
   REFERENCES "REGISTRO"."V_RESERVACION"
       ("ID")
   ENABLE
   VALIDATE;

-- Creating index for FOREIGN KEY 'FK_INFOVISITAV_RESERVACION'
CREATE INDEX "IX_FK_INFOVISITAV_RESERVACION"
ON "REGISTRO"."INFOVISITA"
   ("ID_RESERVACION");

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
