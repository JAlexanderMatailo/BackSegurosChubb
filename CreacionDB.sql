--create database PruebaSegurosChubb
use PruebaSegurosChubb;

create table Seguros (
IdSeguros int primary key identity not null,
NombreSeguro varchar(50) not null,
Codigo varchar(50) not null,
SumaAsegurada float not null,
Prima float not null,
Estado char not null
);

create table Persona (
IdAsegurados int primary Key identity not null,
Cedula varchar(10) not null,
NombreCliente varchar(50) not null,
Telefono varchar(50) not null,
Edad int not null,
Estado char not null
);

create table Polizas (
IdPoliza int primary key identity not null,
Estado char not null,
IdSeguros int not null FOREIGN KEY (IdSeguros) REFERENCES Seguros(IdSeguros),
IdAsegurados int not null FOREIGN KEY (IdAsegurados) REFERENCES Persona(IdAsegurados)
);