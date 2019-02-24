CREATE DATABASE dbtimebreak;

USE dbtimebreak;

CREATE TABLE tab_Klasse
(
	K_Name varchar(10) NOT NULL,
    K_Richtung varchar(60) NOT NULL,
    CONSTRAINT PK PRIMARY KEY (K_Name)
)ENGINE=INNODB;

CREATE TABLE tab_Menschen
(
	M_Matrikelnummer varchar(15) NOT NULL,
    M_Passwort varchar(32) NOT NULL,
    M_Vorname varchar(15) NOT NULL,
    M_Nachname varchar(15) NOT NULL,
    M_istLehrer boolean NOT NULL,
    M_Klasse varchar(10),
    CONSTRAINT PK PRIMARY KEY (M_Matrikelnummer),
    CONSTRAINT FK_Klasse FOREIGN KEY (M_Klasse) REFERENCES tab_Klasse(K_Name)
)ENGINE=INNODB;

CREATE TABLE tab_Anwesenheit
(
	A_ID tinyint NOT NULL AUTO_INCREMENT,
    A_Klassenraum varchar(4),
    A_Datum date not null,
    M_Matrikelnummer varchar(15) not null,
    A_Anfangszeit time,
    A_Endzeit time,
    A_Gesamtzeit int,
    CONSTRAINT FK_Matrikelnummer_Anwesenheit FOREIGN KEY (M_Matrikelnummer) REFERENCES tab_Menschen(M_Matrikelnummer),
    CONSTRAINT PK PRIMARY KEY (A_ID, A_Datum, M_Matrikelnummer)
)ENGINE=INNODB;

CREATE TABLE tab_Pause
(
	P_ID tinyint NOT NULL AUTO_INCREMENT,
    A_ID tinyint NOT NULL,
    A_Datum date not null,
    M_Matrikelnummer varchar(15) NOT NULL,
    P_Anfangszeit time NOT NULL,
    P_Endzeit time,
    P_Aufenthaltsort varchar(40) NOT NULL,
    P_Gesamtzeit int,
    CONSTRAINT FK_Matrikelnummer_Pause FOREIGN KEY (A_ID, A_Datum, M_Matrikelnummer) REFERENCES tab_Anwesenheit(A_ID, A_Datum, M_Matrikelnummer),
    CONSTRAINT PK PRIMARY KEY (P_ID, A_Datum, M_Matrikelnummer)
)ENGINE=INNODB;

/* Testdatensätze */

INSERT INTO tab_klasse 
VALUES
('AIT21v', 'Informationstechnische Assistente'),
('AIT12v', 'Informationstechnische Assistente'),
('AIT22v', 'Informationstechnische Assistente'),
('GTA21v', 'Gestaltunstechnische Assistente');

INSERT INTO tab_menschen
VALUES
('123456789', 'toor', 'German', 'Firsov', 0, 'AIT22v'),
('987654321', 'root', 'Daniil', 'Politaev', 0, 'GTA21v');

INSERT INTO tab_menschen(M_Matrikelnummer, M_Passwort, M_Vorname, M_Nachname, M_istLehrer)
VALUES
('mmm', 'hello', 'Max', 'Mustermann', 1);