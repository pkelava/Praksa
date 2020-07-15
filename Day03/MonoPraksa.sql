DROP TABLE kupovina;
DROP TABLE Kupac;
DROP TABLE proizvod;

CREATE TABLE Kupac (
kupac_id INT CONSTRAINT kupac_pk PRIMARY KEY,
ime VARCHAR(20) NOT NULL,
prezime VARCHAR(20) NOT NULL
);

CREATE TABLE proizvod (
proizvod_id INT CONSTRAINT proizvod_pk PRIMARY KEY,
naziv_proizvoda VARCHAR(20) NOT NULL,
cijena_proizvoda INT NOT NULL
);

CREATE TABLE Kupovina (
kupac_id INT
CONSTRAINT kupovina_fk_kupac
REFERENCES kupac(kupac_id)
ON DELETE CASCADE,
proizvod_id INT 
CONSTRAINT kupovina_fk_proizvod
REFERENCES proizvod(proizvod_id)
ON DELETE CASCADE,
nacin_placanja VARCHAR(10) NOT NULL,
datum_kupovine DATE NOT NULL
);


INSERT INTO proizvod( proizvod_id, naziv_proizvoda, cijena_proizvoda)
VALUES (1,'Biljeznica',5);
INSERT INTO proizvod( proizvod_id, naziv_proizvoda, cijena_proizvoda)
VALUES (2,'Pribor za pisanje',30);
INSERT INTO proizvod( proizvod_id, naziv_proizvoda, cijena_proizvoda)
VALUES (3,'Geometrijski pribor',50);
INSERT INTO proizvod( proizvod_id, naziv_proizvoda, cijena_proizvoda)
VALUES (4,'Kalkulator',180);
INSERT INTO proizvod( proizvod_id, naziv_proizvoda, cijena_proizvoda)
VALUES (5,'Pernica',40);
INSERT INTO proizvod( proizvod_id, naziv_proizvoda, cijena_proizvoda)
VALUES (6,'Torba',200);

INSERT INTO kupac(kupac_id, ime, prezime)
VALUES (1, 'Petar', 'Kelava');
INSERT INTO kupac(kupac_id, ime, prezime)
VALUES (2, 'Goran', 'Kelava');
INSERT INTO kupac(kupac_id, ime, prezime)
VALUES (3, 'Igor', 'Kelava');

INSERT INTO kupovina(kupac_id, proizvod_id, nacin_placanja, datum_kupovine)
VALUES (1, 1, 'Gotovina', GETDATE());
INSERT INTO kupovina(kupac_id, proizvod_id, nacin_placanja, datum_kupovine)
VALUES (1, 2, 'Gotovina', GETDATE());
INSERT INTO kupovina(kupac_id, proizvod_id, nacin_placanja, datum_kupovine)
VALUES (1, 5, 'Gotovina', GETDATE());
INSERT INTO kupovina(kupac_id, proizvod_id, nacin_placanja, datum_kupovine)
VALUES (2, 3, 'Kartica', GETDATE());
INSERT INTO kupovina(kupac_id, proizvod_id, nacin_placanja, datum_kupovine)
VALUES (3, 6, 'Kartica', GETDATE());
