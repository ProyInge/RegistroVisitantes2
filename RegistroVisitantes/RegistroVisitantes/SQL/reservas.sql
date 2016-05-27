--ESQUEMA: RESERVAS

-----------------------------------------------------------------------------------------------------------------GRANTS
GRANT REFERENCES ON RESERVACION TO REGISTRO;
GRANT REFERENCES ON ESTACION TO REGISTRO;
GRANT SELECT ON CONTACTO TO REGISTRO;
GRANT SELECT ON GRUPOANON TO REGISTRO;
GRANT SELECT ON GRUPORSRV TO REGISTRO;

-----------------------------------------------------------------------------------------------------------------FOREIGN KEYS
DELETE FROM RESERVACION WHERE RESPONSABLE NOT IN (SELECT ID FROM CONTACTO);
DELETE FROM RESERVACION WHERE SOLICITANTE NOT IN (SELECT ID FROM CONTACTO);
ALTER TABLE RESERVACION
	ADD CONSTRAINT RESPONSABLE_CONTACTO_FK
		FOREIGN KEY (RESPONSABLE) REFERENCES CONTACTO(ID);
ALTER TABLE RESERVACION
	ADD CONSTRAINT SOLICITANTE_CONTACTO_FK
		FOREIGN KEY (SOLICITANTE) REFERENCES CONTACTO(ID);

DELETE FROM RESERVACION WHERE GRUPO NOT IN (SELECT ID FROM GRUPORSRV);
DELETE FROM RESERVACION WHERE GRUPO NOT IN (SELECT ID FROM GRUPOANON);
ALTER TABLE RESERVACION
	ADD CONSTRAINT GRUPO_GRUPORSRV_FK
		FOREIGN KEY (GRUPO) REFERENCES GRUPORSRV(ID);
ALTER TABLE RESERVACION
	ADD CONSTRAINT GRUPO_GRUPOANON_FK
		FOREIGN KEY (GRUPO) REFERENCES GRUPOANON(ID);

COMMIT;