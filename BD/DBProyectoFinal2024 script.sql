USE master
Go

if exists(Select * FROM sys.databases WHERE name='ProyectoFinal2024')
BEGIN
	 DROP  DATABASE ProyectoFinal2024
END
GO

CREATE DATABASE ProyectoFinal2024
GO

USE ProyectoFinal2024
GO

CREATE LOGIN [IIS APPPOOL\DefaultAppPool] FROM WINDOWS 
GO

CREATE USER [IIS APPPOOL\DefaultAppPool] FOR LOGIN [IIS APPPOOL\DefaultAppPool]
GO

exec sys.sp_addrolemember 'db_owner', [IIS APPPOOL\DefaultAppPool]
GO

create login juaperez with password ='pass1234'
create user juaperez from login juaperez
grant exec to juaperez
GO

create login marrodri with password ='pass2345'
create user marrodri from login marrodri
grant exec to marrodri
GO

create login marrodri with password ='pass2345'
create user marrodri from login marrodri
grant exec to marrodri
GO

create login cargomez with password='pass3456'
create user cargomez from login cargomez
grant exec to cargomez
GO

create login anafern with password='pass4567'
create user anafern from login anafern
grant exec to anafern
GO

create login luigonza with password='pass5678'
create user luigonza from login luigonza
grant exec to luigonza
GO

create login pabmende with password='pass6789'
create user pabmende from login pabmende
grant exec to pabmende
GO

create login claserra with password='pass7890'
create user claserra from login claserra
grant exec to claserra
GO

create login migdiaz1 with password='pass8901'
create user migdiaz1 from login migdiaz1
grant exec to migdiaz1
GO

create login laumarti with password='pass9012'
create user laumarti from login laumarti
grant exec to laumarti
GO

create login soflopez with password='pass0123'
create user soflopez from login soflopez
grant exec to soflopez
Go

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--- TABLAS -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
create table Empleado 
(
    EmpUsu varchar (25) primary key,
    EmpNom varchar(50) not null ,
    EmpPass varchar(25) not null,
)
go

create table Cliente 
(
    CliCI varchar(8) Check (CliCI like '[1-6][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or CliCI like '[1-9][0-9][0-9][0-9][0-9][0-9][0-9]') Primary Key,
    CliNom varchar(50) not null,
    CliNumTar varchar(16) not null  check(CliNumTar not like '%[^0-9]%' and len(CliNumTar) = 16) unique,
    CliTel varchar(9) check (CliTel like '[0][9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' or CliTel like '[2,4][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
	
)
go

create table Categoria 
(
    CatCod varchar(6) primary key  check(CatCod not like '%[^A-Za-z0-9]%' and len(CatCod) = 6), 
    CatNom varchar(30) not null ,
	CatActivo bit not null default 1
)
go

create table Articulo 
(
    ArtCod varchar(10) primary key check(ArtCod not like '%[^A-Za-z0-9]%' and len(ArtCod) = 10), 
    ArtNom varchar(50) not null,
    ArtTipo varchar(7) check (ArtTipo in ('Unidad', 'Blíster', 'Sobre', 'Frasco')) not null,
    ArtTam int  not null check (Arttam > 0),
	ArtFechVen datetime not null check(ArtFechVen > getdate()), 
	ArtPre decimal (20,2) check (ArtPre > 0),
    ArtCatCod varchar(6) not null foreign key references Categoria(CatCod),
	ArtActivo bit not null default 1
)
go

create table Estado 
(
    EstCod int primary key identity(1,1),
    EstNom varchar(9),
)
go

create table Venta 
(
    VenNum int primary key identity(1,1),
    VenFec datetime not null default getdate(),
    VenMon decimal(10,2) not null check( VenMon > 0),
	VenDir varchar(200) not null,
    VenCliCI varchar(8) not null foreign key references Cliente (CliCI),
    VenEmpUsu varchar(25) not null foreign key references Empleado(EmpUsu),
     
)
go

create table DetalleVenta 
(
	DetVenNum int not null foreign key references Venta (VenNum),
	DetVenArtCod varchar(10) not null foreign key references Articulo(ArtCod),
	DetVenCant int not null check (DetVenCant > 0),
	primary key(DetVenNum, DetVenArtCod)
)
go

create table HistoricoEstado
(
	VenNum int not null foreign key references Venta (VenNum),
	VenEst int not null foreign key references Estado(EstCod),
	Fecha datetime not null default getdate()
	primary key (VenNum, VenEst)
)
go



----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--- EMPLEADO ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE LogueoEmpleado 
@usuario varchar(25), 
@contraseña varchar(25)
AS
BEGIN
    SELECT * 
    FROM Empleado
    WHERE EmpUsu = @usuario AND EmpPAss = @contraseña
END
GO

create proc BuscarEmpleado 
@empleado varchar(25)
as
begin
	select * from Empleado where EmpUsu = @empleado
end
go


----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--- CATEGORIA --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
create proc AltaCategoria 
@CatCod varchar(6), 
@CatNom varchar(30)
as
begin
    	if exists (select * from Categoria where CatCod = @CatCod and CatActivo = 1)
			begin 
				return -1
			end 
		
		if exists (select * from Categoria where CatCod = @CatCod and CatActivo = 0)
			begin
				update Categoria set CatNom = @CatNom, CatActivo = 1 where CatCod = @CatCod
				return 1
			end

	    insert into  Categoria (catCod, catNom)
						values (@catCod, @catNom)
    
		if (@@error <> 0)
			begin 
				return -2
			end
			else 
			    return 1
end 
go

create  proc BajaCategoria 
@CatCod varchar(6)
as
begin
    	if not exists (select * from Categoria where CatCod = @CatCod)
			begin 
				return -1
			end 
		
		if exists (select * from Articulo where ArtCatCod = @CatCod)
			begin 
				update Categoria set  CatActivo = 0 where CatCod = @CatCod
				return 1
			end 
		
		delete Categoria where CatCod = @catCod
						
		if (@@error <> 0)
			begin 
				return -2
			end
			else 
			    return 1
end 
GO

create proc ModificarCategoria 
@CatCod varchar(6), 
@CatNom varchar(30)
as
begin
    	if not exists (select * from Categoria where CatCod = @CatCod and CatActivo = 1)
			begin 
				return -1
			end 
		else 
			begin 
				update Categoria set CatNom = @CatNom where CatCod = @CatCod
				return 1
			end
		
		
		if (@@error <> 0)
			begin 
				return -2
			end
			else 
			    return 1
end 
go

create proc ConsultaCategoria @CatCod varchar(6)
as
begin
	select *
    FROM Categoria
    WHERE catCod = @catCod and CatActivo = 1
end
go

create proc ConsultaTodasCategorias
@CatCod varchar(6)
as
begin
	select *
    FROM Categoria
	where CatCod = @CatCod
end
go

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--- CLIENTE ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
create proc AltaCliente 
@cliCi varchar(8), 
@nombre varchar(50), 
@clinumTar varchar(16), 
@telefono varchar(9)
as
begin 
    if  exists (select * from Cliente where CliCI = @cliCi)
    begin
        return -1
    end

    insert into  Cliente (CliCI, CliNom, CliNumTar, CliTel)
    values (@cliCi, @nombre, @clinumTar, @telefono)

    if(@@ERROR <> 0)
    begin
        return -2
    end

    return 1
end 
go

create proc ModificarCliente @cliCi varchar(8), @nombre varchar(50), @clinumTar varchar(16), @telefono varchar(9)
as
begin 
    if not exists (select * from Cliente where CliCI = @cliCi)
    begin
        return -1
    end

    update  Cliente set  CliNom=@nombre, CliNumTar = @clinumTar, CliTel = @telefono where clici=@CliCi

    if(@@ERROR <> 0)
    begin
        return -2
    end

    return 1
end 
go

create proc ConsultaCliente (@cliCi varchar(8))
as
begin
    select *
    from Cliente
    where CliCI = @cliCi
end 
go

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--- ARTICULO ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
create proc AltaArticulo 
@artCod varchar(10),
@artNom varchar(50),
@artTipo varchar(7),
@artTam int,
@artFechaVen datetime,
@precio decimal(20,2), 
@catCod varchar(6)
as
begin
    	if exists (select * from Articulo where ArtCod = @artCod and ArtActivo = 1)
			begin 
				return -1
			end 
		if not exists (select * from Categoria where CatCod = @catCod and CatActivo = 1)
			begin 
				return -2
			end 
		
		if exists (select * from Articulo where ArtCod = @artCod and ArtActivo = 0)
			begin
				update Articulo 
					set  ArtNom =@artNom, 
						 ArtTipo = @artTipo, 
						 ArtTam = @ArtTam, 
						 ArtFechVen = @ArtFechaVen, 
						 ArtPre = @precio, 
						 ArtCatCod = @catCod, 
						 ArtActivo = 1
				 where ArtCod = @artCod
				return 1
			end

	   insert into Articulo (ArtCod, ArtNom, ArtTipo, ArtTam, ArtFechVen, ArtPre, ArtCatCod)
				values (@artCod, @artNom, @artTipo, @artTam, @artFechaVen, @precio, @catCod)
    
		if (@@error <> 0)
			begin 
				return -3
			end
			else 
			    return 1
end 
GO

create proc ModificarArticulo  
@artCod varchar(10), 
@artNom varchar(50),
@artTipo varchar(7),    
@artTam int,
@artFechaVen datetime,
@precio decimal(20,2),
@catCod varchar(6)
as
begin
    	if not exists (select * from Articulo where ArtCod = @artCod and ArtActivo = 1)
			begin 
				return -1
			end 
		if not exists (select * from Categoria where CatCod = @catCod and CatActivo = 1)
			begin 
				return -2
			end 

		update Articulo 
			set  ArtNom =@artNom, 
				 ArtTipo = @artTipo, 
				 ArtTam = @ArtTam, 
				 ArtFechVen = @ArtFechaVen, 
				 ArtPre = @precio, 
				 ArtCatCod = @catCod, 
				 ArtActivo = 1
			where ArtCod = @artCod
			 
		if (@@error <> 0)
			begin 
				return -3
			end
			else 
			    return 1
end 
go

create proc EliminarArticulo  @artCod varchar(10)
as
begin
    	if not exists (select * from Articulo where ArtCod = @artCod)
		return -1
	
		if exists (select * from DetalleVenta where DetVenArtCod = @artCod)
			begin
				update Articulo set ArtActivo = 0 where ArtCod = @artCod 
					return 1
			end
		
		delete Articulo
				 where ArtCod = @artCod 
			 
    
		if (@@error <> 0)
			begin 
				return -2
			end
			else 
			    return 1
end 
go

create proc ConsultaArticulo @artCod varchar(10)
as
begin
    select *
    from  Articulo
    where  ArtCod = @artCod and ArtActivo = 1
end 
go

create proc ConsultaTodosArticulos 
@artCod varchar(10)
as
begin
    select *
    from  Articulo	
	where ArtCod = @artCod
end 
go

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--- VENTA ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create proc AltaVenta
  @VenMon decimal(10,2),
  @VenDir varchar(200),
  @VenCliCI varchar(8),
  @VenEmpUsu varchar(25),
  @NumVen int output
as
Begin
		if not exists(Select * From Cliente where CliCI = @VenCliCI)
			return -1
		
		if not exists(Select * From Empleado where EmpUsu = @VenEmpUsu)
			return -2
	
		insert into Venta (VenMon, VenDir, VenCliCI,VenEmpUsu)
					values (
						@VenMon, 
						@VenDir, 
						@VenCliCI, 
    					@VenEmpUsu)

		if (@@error <> 0)
			begin 
				return -3
			end
			else 
			begin
				set @NumVen = SCOPE_IDENTITY()
				return 1
			end
				
			  
End
go

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--- DETALLE VENTA ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
create proc AltaDetalleVenta @DetVenNum int, @DetVenArtCod varchar(10), @DetVenCant int As
Begin
		
		if not exists(Select * From Venta where VenNum = @DetVenNum)
			return -1

		if exists (select * from DetalleVenta where DetVenNum = @DetVenNum and DetVenArtCod = @DetVenArtCod)
			return -2

				
		if not exists(Select * From Articulo where ArtCod = @DetVenArtCod and ArtActivo = 1)
			return -3

		Insert DetalleVenta(DetVenNum, DetVenArtCod, DetVenCant) Values (@DetVenNum, @DetVenArtCod, @DetVenCant)

		if (@@error <> 0)
			begin 
				return -4
			end
			else 
			    return 1
End
go

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--- ESTADO Y HISTORICO VENTA -----------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
create proc AltaEstadoVenta @VenNum int
as
Begin
		if not exists(Select * From Venta where VenNum = @VenNum)
			return -1
			
		Insert HistoricoEstado(VenNum, VenEst) Values (@VenNum, 1)

		if (@@error <> 0)
			begin 
				return -2
			end
			else 
			    return 1
End
go

create proc CambiarEstado @VenNum int, @EstCod int
as
Begin
		Declare @EstadoActual int
		
		if not exists (select * from Estado where EstCod = @EstCod)
			return -1
		
		if not exists (select * from Venta where VenNum = @VenNum)
			return -2
		
		select @EstadoActual = max(VenEst) from HistoricoEstado where VenNum = @VenNum

				
		if (@EstCod <=  @EstadoActual)
			return -3
		else if (@EstCod > @EstadoActual + 1)
			return -4 
		
		insert into  HistoricoEstado (VenNum, VenEst)
				values (@VenNum, @EstCod)
		if (@@ERROR <> 0)
			Begin
				return -5
			End	
	
		return 1	
End
go

create proc BuscarEstado
@estado int
as
begin
	select *
	from Estado
	where EstCod = @estado
end
go

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--- LISTADOS ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Create proc ListadoClientes
as 
begin 
select * from cliente
end
go

Create proc ListadoArticulos
as 
begin 
select * from Articulo where ArtActivo=1
end
go

Create proc ListadoVentas
as 
begin 
select  * from 
venta
end
go

Create proc ListadoCategorias
as 
begin 
select * from Categoria where CatActivo=1
end
go
Create proc ListadoEstado
as 
begin 
select * from Estado
end
go

Create proc DetalleDeVenta 
@VenNum int
as
begin 
	Select * from DetalleVenta where DetVenNum = @VenNum
end
go

Create proc DetalleEstadoxVenta
@VenNum int
as
begin 
	Select * from HistoricoEstado where VenNum = @VenNum
end
go

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--- DATOS PRUEBA -----------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
use ProyectoFinal2024
go

INSERT INTO Empleado (EmpUsu, EmpNom, EmpPass) 
VALUES 
('juaperez', 'Juan Pérez', 'pass1234'),
('marrodri', 'María Rodríguez', 'pass2345'),
('cargomez', 'Carlos Gómez', 'pass3456'),
('anafern', 'Ana Fernández', 'pass4567'),
('luigonza', 'Luis González', 'pass5678'),
('pabmende', 'Pablo Méndez', 'pass6789'),
('claserra', 'Claudia Serrano', 'pass7890'),
('migdiaz1', 'Miguel Díaz', 'pass8901'),
('laumarti', 'Laura Martínez', 'pass9012'),
('soflopez', 'Sofía López', 'pass0123')
go


--- Estado -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
insert into Estado values ('Armado')
insert into Estado values ('Envío')
insert into Estado values ('Entregado')
insert into Estado values ('Devuelto')

--- Categoria --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
exec AltaCategoria  'ANG001','Analgésicos'
exec AltaCategoria  'ATB002','Antibióticos'
exec AltaCategoria  'AIF003','Antiinflamatorios'
exec AltaCategoria  'VIT004','Vitaminas'
exec AltaCategoria  'ACD005','Antiácidos'
exec AltaCategoria  'ALG006','Antialérgicos'
exec AltaCategoria  'DRM007','Dermatológicos'
exec AltaCategoria  'DIG008','Digestivos'
exec AltaCategoria  'OFT009','Oftálmicos'
exec AltaCategoria  'RSP010','Respiratorios'

--- Articulo ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
exec AltaArticulo 'IBUP100001', 'Ibuprofeno 400mg', 'Blíster', 10,'01/12/2025', 250.50, 'ANG001'
exec AltaArticulo 'PARA100002', 'Paracetamol 500mg', 'Blíster', 8, '01/12/2025' , 180.00, 'ANG001'
exec AltaArticulo 'ASPI100003', 'Aspirina 500mg', 'Blíster', 12,'01/02/2026', 220.00, 'ANG001'
exec AltaArticulo 'KETO100004', 'Ketoprofeno 50mg', 'Sobre', 2,'02/02/2026', 150.75, 'ANG001'
exec AltaArticulo 'DIPI100005', 'Dipirona 500mg', 'Blíster', 10,'03/02/2026', 195.25, 'ANG001'
exec AltaArticulo 'TRAM100006', 'Tramadol 100mg', 'Frasco', 30,'04/02/2026', 450.00, 'ANG001'
exec AltaArticulo 'NAPR100007', 'Naproxeno 550mg', 'Blíster', 10,'05/02/2026', 280.50, 'ANG001'
exec AltaArticulo 'DICL100008', 'Diclofenac 75mg', 'Blíster', 15,'06/02/2026', 320.25, 'ANG001'
exec AltaArticulo 'AMOX200001', 'Amoxicilina 500mg', 'Blíster', 14,'01/01/2026', 420.00, 'ATB002'
exec AltaArticulo 'AZIT200002', 'Azitromicina 500mg', 'Blíster', 6,'01/01/2026', 580.50, 'ATB002'
exec AltaArticulo 'CEFL200003', 'Cefalexina 500mg', 'Blíster', 12,'01/01/2026', 495.75, 'ATB002'
exec AltaArticulo 'CLAR200004', 'Claritromicina 250mg', 'Blíster', 10,'01/01/2026', 650.25, 'ATB002'
exec AltaArticulo 'CIPR200005', 'Ciprofloxacina 500mg', 'Blíster', 14,'01/01/2026', 520.00, 'ATB002'
exec AltaArticulo 'DOXL200006', 'Doxiciclina 100mg', 'Blíster', 10,'01/01/2026', 380.50, 'ATB002'
exec AltaArticulo 'METR200007', 'Metronidazol 500mg', 'Blíster', 15,'01/01/2026', 425.75, 'ATB002'
exec AltaArticulo 'NITR200008', 'Nitrofurantoína 100mg', 'Blíster', 20,'01/01/2026', 290.25, 'ATB002'
exec AltaArticulo 'DEXA300001', 'Dexametasona 4mg', 'Blíster', 10,'01/01/2026', 320.50, 'AIF003'
exec AltaArticulo 'PRED300002', 'Prednisona 20mg', 'Blíster', 15,'01/01/2026', 280.75, 'AIF003'
exec AltaArticulo 'MELO300003', 'Meloxicam 15mg', 'Blíster', 10,'01/01/2026', 390.25, 'AIF003'
exec AltaArticulo 'INDO300004', 'Indometacina 25mg', 'Blíster', 20,'01/01/2026', 245.00, 'AIF003'
exec AltaArticulo 'ETOD300005', 'Etodolaco 400mg', 'Blíster', 14,'01/01/2026', 420.50, 'AIF003'
exec AltaArticulo 'CELE300006', 'Celecoxib 200mg', 'Blíster', 10,'01/01/2026', 580.25, 'AIF003'
exec AltaArticulo 'NABU300007', 'Nabumetona 500mg', 'Blíster', 15,'01/01/2026', 340.75, 'AIF003'
exec AltaArticulo 'PIRO300008', 'Piroxicam 20mg', 'Blíster', 12,'01/01/2026', 295.50, 'AIF003'
exec AltaArticulo 'VITC400001', 'Vitamina C 1000mg', 'Blíster', 30,'01/01/2026', 280.00, 'VIT004'
exec AltaArticulo 'VITD400002', 'Vitamina D3 2000UI', 'Frasco', 60,'01/01/2026', 450.50, 'VIT004'
exec AltaArticulo 'VITB400003', 'Complejo B', 'Blíster', 20,'01/01/2026', 320.25, 'VIT004'
exec AltaArticulo 'VITA400004', 'Vitamina A 5000UI', 'Frasco', 30,'01/01/2026', 380.75, 'VIT004'
exec AltaArticulo 'VITE400005', 'Vitamina E 400UI', 'Blíster', 15,'01/01/2026', 290.50, 'VIT004'
exec AltaArticulo 'ZINK400006', 'Zinc 50mg', 'Blíster', 25,'01/01/2026', 180.25, 'VIT004'
exec AltaArticulo 'CALC400007', 'Calcio + D3', 'Blíster', 30,'01/01/2026', 420.75, 'VIT004'
exec AltaArticulo 'MAGN400008', 'Magnesio 500mg', 'Blíster', 20,'01/01/2026', 250.50, 'VIT004'
exec AltaArticulo 'OMEP500001', 'Omeprazol 20mg', 'Blíster', 14,'01/01/2026', 380.50, 'ACD005'
exec AltaArticulo 'RANI500002', 'Ranitidina 150mg', 'Blíster', 20,'01/01/2026', 290.25, 'ACD005'
exec AltaArticulo 'ESOP500003', 'Esomeprazol 40mg', 'Blíster', 15,'01/01/2026', 450.75, 'ACD005'
exec AltaArticulo 'MAAL500004', 'Magaldrato Susp', 'Frasco', 150,'01/01/2026', 220.50, 'ACD005'
exec AltaArticulo 'FAMO500005', 'Famotidina 40mg', 'Blíster', 10,'01/01/2026', 320.25, 'ACD005'
exec AltaArticulo 'PANT500006', 'Pantoprazol 20mg', 'Blíster', 14,'01/01/2026', 410.75, 'ACD005'
exec AltaArticulo 'BICAR50007', 'Bicarbonato Sodio', 'Sobre', 5,'01/01/2026', 150.50, 'ACD005'
exec AltaArticulo 'HIDR500008', 'Hidróxido Aluminio', 'Frasco', 180,'01/01/2026', 280.25, 'ACD005'
exec AltaArticulo 'LORA600001', 'Loratadina 10mg', 'Blíster', 10,'01/01/2026', 180.50, 'ALG006'
exec AltaArticulo 'CETI600002', 'Cetirizina 10mg', 'Blíster', 15,'01/01/2026', 220.25, 'ALG006'
exec AltaArticulo 'FEXO600003', 'Fexofenadina 120mg', 'Blíster', 10,'01/01/2026', 350.75, 'ALG006'
exec AltaArticulo 'DESL600004', 'Desloratadina 5mg', 'Blíster', 20,'01/01/2026', 280.50, 'ALG006'
exec AltaArticulo 'EBAS600005', 'Ebastina 10mg', 'Blíster', 15,'01/01/2026', 420.25, 'ALG006'
exec AltaArticulo 'HIDR600006', 'Hidroxizina 25mg', 'Blíster', 12,'01/01/2026', 310.75, 'ALG006'
exec AltaArticulo 'CLOR600007', 'Clorfeniramina 4mg', 'Blíster', 20,'01/01/2026', 150.50, 'ALG006'
exec AltaArticulo 'RUPA600008', 'Rupatadina 10mg', 'Blíster', 15,'01/01/2026', 380.25, 'ALG006'
exec AltaArticulo 'BETA700001', 'Betametasona Crema', 'Frasco', 30,'01/01/2026', 450.75, 'DRM007'
exec AltaArticulo 'CLOT700002', 'Clotrimazol 1%', 'Frasco', 20,'01/01/2026', 320.50, 'DRM007'
exec AltaArticulo 'MICO700003', 'Miconazol Gel', 'Frasco', 15,'01/01/2026', 280.25, 'DRM007'
exec AltaArticulo 'KETO700004', 'Ketoconazol Shampoo', 'Frasco', 120,'01/01/2026', 580.50, 'DRM007'
exec AltaArticulo 'HIDRO70005', 'Hidrocortisona 1%', 'Frasco', 25,'01/01/2026', 420.75, 'DRM007'
exec AltaArticulo 'MOME700006', 'Mometasona Crema', 'Frasco', 15,'01/01/2026', 490.25, 'DRM007'
exec AltaArticulo 'TERB700007', 'Terbinafina Crema', 'Frasco', 20,'01/01/2026', 350.50, 'DRM007'
exec AltaArticulo 'ACIC700008', 'Aciclovir Crema', 'Frasco', 10,'01/01/2026', 280.75, 'DRM007'
exec AltaArticulo 'METO800001', 'Metoclopramida 10mg', 'Blíster', 30,'01/01/2026', 220.50, 'DIG008'
exec AltaArticulo 'DIME800002', 'Dimeticona 40mg', 'Blíster', 20,'01/01/2026', 180.25, 'DIG008'
exec AltaArticulo 'BISAC80003', 'Bisacodilo 5mg', 'Blíster', 10,'01/01/2026', 150.75, 'DIG008'
exec AltaArticulo 'LACTU80004', 'Lactulosa Solución', 'Frasco', 120,'01/01/2026', 320.50, 'DIG008'
exec AltaArticulo 'DOMA800005', 'Domperidona 10mg', 'Blíster', 15,'01/01/2026', 280.25, 'DIG008'
exec AltaArticulo 'SIME800006', 'Simeticona 40mg', 'Blíster', 20,'01/01/2026', 190.75, 'DIG008'
exec AltaArticulo 'CARB800007', 'Carbón Activado', 'Sobre', 5,'01/01/2026', 120.50, 'DIG008'
exec AltaArticulo 'PLAN800008', 'Plantago Psyllium', 'Sobre', 10,'01/01/2026', 220.25, 'DIG008'
exec AltaArticulo 'ARTF900001', 'Lágrimas Artificiales', 'Frasco', 15,'01/01/2026', 280.50, 'OFT009'
exec AltaArticulo 'ANTI900002', 'Antialérgico Ocular', 'Frasco', 10,'01/01/2026', 320.25, 'OFT009'
exec AltaArticulo 'TIMOL90003', 'Timolol 0.5%', 'Frasco', 5,'01/01/2026', 420.75, 'OFT009'
exec AltaArticulo 'TETR900004', 'Tetracaína Gotas', 'Frasco', 10,'01/01/2026', 250.50, 'OFT009'
exec AltaArticulo 'TROPI90005', 'Tropicamida 1%', 'Frasco', 15,'01/01/2026', 380.25, 'OFT009'
exec AltaArticulo 'CICLO90006', 'Ciclosporina 0.05%', 'Frasco', 5,'01/01/2026', 520.75, 'OFT009'
exec AltaArticulo 'TOBRA90007', 'Tobramicina Gotas', 'Frasco', 10,'01/01/2026', 420.50, 'OFT009'
exec AltaArticulo 'DORZO90008', 'Dorzolamida 2%', 'Frasco', 5,'01/01/2026', 480.25, 'OFT009'
exec AltaArticulo 'SALB000001', 'Salbutamol Inhalador', 'Frasco', 200,'01/01/2026', 380.50, 'RSP010'
exec AltaArticulo 'FLUT000002', 'Fluticasona Spray', 'Frasco', 120,'01/01/2026', 520.25, 'RSP010'
exec AltaArticulo 'IPRA000003', 'Ipratropio Inh', 'Frasco', 200,'01/01/2026', 450.75, 'RSP010'
exec AltaArticulo 'BUDE000004', 'Budesonida Inh', 'Frasco', 200,'01/01/2026', 480.50, 'RSP010'
exec AltaArticulo 'MONT000005', 'Montelukast 10mg', 'Blíster', 30,'01/01/2026', 620.25, 'RSP010'
exec AltaArticulo 'TEOF000006', 'Teofilina 200mg', 'Blíster', 20,'01/01/2026', 280.75, 'RSP010'
exec AltaArticulo 'AMBO000007', 'Ambroxol Jarabe', 'Frasco', 120,'01/01/2026', 220.50, 'RSP010'
exec AltaArticulo 'BROM000008', 'Bromhexina Jarabe', 'Frasco', 100,'01/01/2026', 190.25, 'RSP010'

--- Cliente ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
exec AltaCliente '12345678', 'Juan Pérez', '1234567812345678', '22001234'
exec AltaCliente '22345678', 'María Rodríguez', '2234567822345678', '091234567'
exec AltaCliente '32345678', 'Carlos Gómez', '3234567832345678', '24005678'
exec AltaCliente '42345678', 'Ana Fernández', '4234567842345678', '42345678'
exec AltaCliente '52345678', 'Luis González', '5234567852345678', '091987654'
exec AltaCliente '42345678', 'Sofía López', '6234567862345678', '26004567'
exec AltaCliente '32345678', 'Miguel Díaz', '7234567872345678', '42987654'
exec AltaCliente '22345678', 'Laura Martínez', '8234567882345678', '091654321'
exec AltaCliente '12345679', 'Diego Silva', '1234567912345679', '22019876'
exec AltaCliente '22345699', 'Valeria Torres', '2234567922345679', '091112233'
exec AltaCliente '32345639', 'José Ramírez', '3234567932345679', '24001234'
exec AltaCliente '42345619', 'Camila Castillo', '4234567942345679', '43345678'
exec AltaCliente '45434157', 'Pablo Méndez', '5234567952345679', '091998877'
exec AltaCliente '45446232', 'Lucía Vega', '6234567962345679', '25004567'
exec AltaCliente '42345679', 'Andrés Acosta', '7234567972345679', '47123456'
exec AltaCliente '2345679', 'Florencia Sosa', '8234567982345679', '091223344'
exec AltaCliente '12345670', 'Santiago Pereira', '1234567012345670', '24019876'
exec AltaCliente '22345670', 'Carolina Rivero', '2234567022345670', '091334455'
exec AltaCliente '32345670', 'Martín Suárez', '3234567032345670', '27005678'
exec AltaCliente '42345670', 'Verónica Cáceres', '4234567042345670', '44234567'
exec AltaCliente '52345670', 'Fernando Paredes', '5234567052345670', '091556677'
exec AltaCliente '62345670', 'Natalia Castro', '6234567062345670', '26012345'
exec AltaCliente '52345670', 'Jorge Morales', '7234567072345670', '48345678'
exec AltaCliente '12345670', 'Alejandra Vargas', '8234567082345670', '091667788'
exec AltaCliente '12345671', 'Agustín Cabrera', '1234567112345671', '25019876'
exec AltaCliente '22345671', 'Claudia Serrano', '2234567122345671', '091778899'
exec AltaCliente '32345671', 'Sebastián Varela', '3234567132345671', '23004567'
exec AltaCliente '42345671', 'Andrea Batista', '4234567142345671', '41123456'
exec AltaCliente '2345671', 'Tomás Correa', '5234567152345671', '091889900' 
go



exec AltaVenta 1111.50, 'Av. Gonzalo Ramírez 1234', '12345679', 'juaperez',''
exec AltaDetalleVenta 1, 'IBUP100001', 3
exec AltaDetalleVenta 1, 'PARA100002', 2
exec AltaEstadoVenta 1
go

exec AltaVenta 1030.75, 'Av. San Martín 2345', '22345678', 'marrodri',''
exec AltaDetalleVenta 2, 'ASPI100003', 4
exec AltaDetalleVenta 2, 'KETO100004', 1
exec AltaEstadoVenta 2
go

exec AltaVenta 1876.25, 'Av. Brasil 3456', '32345678', 'cargomez',''
exec AltaDetalleVenta 3, 'DIPI100005', 5
exec AltaDetalleVenta 3, 'TRAM100006', 2
exec AltaEstadoVenta 3
go

exec AltaVenta 1521.75, 'Av. Rondeau 4567', '42345678', 'anafern',''
exec AltaDetalleVenta 4, 'NAPR100007', 2
exec AltaDetalleVenta 4, 'DICL100008', 3
exec AltaEstadoVenta 4
go

exec AltaVenta 2742.00, 'Av. Libertador 5678', '52345678', 'luigonza',''
exec AltaDetalleVenta 5, 'AMOX200001', 1
exec AltaDetalleVenta 5, 'AZIT200002', 4
exec AltaEstadoVenta 5
go

exec AltaVenta 2787.75, 'Av. Artigas 6789', '42345678', 'pabmende',''
exec AltaDetalleVenta 6, 'CEFL200003', 3
exec AltaDetalleVenta 6, 'CLAR200004', 2
exec AltaEstadoVenta 6
go

exec AltaVenta 2942.50, 'Av. Ellauri 7890', '32345678', 'claserra',''
exec AltaDetalleVenta 7, 'CIPR200005', 2
exec AltaDetalleVenta 7, 'DOXL200006', 5
exec AltaEstadoVenta 7
go

exec AltaVenta 3444.50, 'Av. Sarmiento 8901', '45434157', 'migdiaz1',''
exec AltaDetalleVenta 8, 'METR200007', 4
exec AltaDetalleVenta 8, 'NITR200008', 6
exec AltaEstadoVenta 8
go

exec AltaVenta 3085.75, 'Av. Burgues 9012', '12345670', 'laumarti',''
exec AltaDetalleVenta 9, 'DEXA300001', 7
exec AltaDetalleVenta 9, 'PRED300002', 3
exec AltaEstadoVenta 9
go

exec AltaVenta 2005.50, 'Av. Joaquín Suárez 0123', '22345670', 'soflopez',''
exec AltaDetalleVenta 10, 'MELO300003', 2
exec AltaDetalleVenta 10, 'INDO300004', 5
exec AltaEstadoVenta 10
go

exec AltaVenta 1841.75, 'Av. Uruguay 1234', '12345671', 'juaperez',''
exec AltaDetalleVenta 11, 'ETOD300005', 3
exec AltaDetalleVenta 11, 'CELE300006', 1
exec AltaEstadoVenta 11
exec CambiarEstado 11, 2
go

exec AltaVenta 1954.00, 'Av. Paraguay 2345', '22345671', 'marrodri',''
exec AltaDetalleVenta 12, 'NABU300007', 4
exec AltaDetalleVenta 12, 'PIRO300008', 2
exec AltaEstadoVenta 12
exec CambiarEstado 12, 2
go

exec AltaVenta 2301.00, 'Av. Lezica 1010', '32345671', 'cargomez',''
exec AltaDetalleVenta 13, 'VITC400001', 5
exec AltaDetalleVenta 13, 'VITD400002', 2
exec AltaEstadoVenta 13
exec CambiarEstado 13, 2
go

exec AltaVenta 2483.75, 'Av. Rivera 2020', '42345671', 'anafern',''
exec AltaDetalleVenta 14, 'VITB400003', 3
exec AltaDetalleVenta 14, 'VITA400004', 4
exec AltaEstadoVenta 14
exec CambiarEstado 14, 2
go

exec AltaVenta 1482.25, 'Av. Italia 3030', '2345671', 'luigonza',''
exec AltaDetalleVenta 15, 'VITE400005', 2
exec AltaDetalleVenta 15, 'ZINK400006', 5
exec AltaEstadoVenta 15
exec CambiarEstado 15, 2
go

exec AltaVenta 3276.00, 'Av. Garzón 4040', '45434157', 'pabmende',''
exec AltaDetalleVenta 16, 'CALC400007', 6
exec AltaDetalleVenta 16, 'MAGN400008', 3
exec AltaEstadoVenta 16
exec CambiarEstado 16, 2
go

exec AltaVenta 2483.00, 'Av. Agraciada 5050', '32345671', 'claserra',''
exec AltaDetalleVenta 17, 'OMEP500001', 5
exec AltaDetalleVenta 17, 'RANI500002', 2
exec AltaEstadoVenta 17
exec CambiarEstado 17, 2
go

exec AltaVenta 2234.25, 'Av. 8 de Octubre 6060', '22345678', 'migdiaz1',''
exec AltaDetalleVenta 18, 'ESOP500003', 3
exec AltaDetalleVenta 18, 'MAAL500004', 4
exec AltaEstadoVenta 18
exec CambiarEstado 18, 2
go

exec AltaVenta 3474.00, 'Av. Luis Alberto de Herrera 7070', '12345670', 'laumarti',''
exec AltaDetalleVenta 19, 'FAMO500005', 7
exec AltaDetalleVenta 19, 'PANT500006', 3
exec AltaEstadoVenta 19
exec CambiarEstado 19, 2
go

exec AltaVenta 1032.75, 'Av. Millán 8080', '22345670', 'soflopez',''
exec AltaDetalleVenta 20, 'BICAR50007', 5
exec AltaDetalleVenta 20, 'HIDR500008', 1
exec AltaEstadoVenta 20
exec CambiarEstado 20, 2
go

exec AltaVenta 1823.25, 'Av. Burgues 9090', '32345671', 'juaperez',''
exec AltaDetalleVenta 21, 'LORA600001', 4
exec AltaDetalleVenta 21, 'CETI600002', 5
exec AltaEstadoVenta 21
exec CambiarEstado 21, 2
exec CambiarEstado 21, 3
go

exec AltaVenta 2735.25, 'Av. Joaquín Suárez 0123', '42345671', 'marrodri',''
exec AltaDetalleVenta 22, 'FEXO600003', 3
exec AltaDetalleVenta 22, 'DESL600004', 6
exec AltaEstadoVenta 22
exec CambiarEstado 22, 2
exec CambiarEstado 22, 3
go

exec AltaVenta 3563.25, 'Av. Gonzalo Ramírez 1234', '2345671', 'cargomez',''
exec AltaDetalleVenta 23, 'EBAS600005', 7
exec AltaDetalleVenta 23, 'HIDR600006', 2
exec AltaEstadoVenta 23
exec CambiarEstado 23, 2
exec CambiarEstado 23, 3
go

exec AltaVenta 1893.25, 'Av. San Martín 2345', '45434157', 'anafern',''
exec AltaDetalleVenta 24, 'CLOR600007', 5
exec AltaDetalleVenta 24, 'RUPA600008', 3
exec AltaEstadoVenta 24
exec CambiarEstado 24, 2
exec CambiarEstado 24, 3
go

exec AltaVenta 3535.75, 'Av. Brasil 3456', '32345671', 'luigonza',''
exec AltaDetalleVenta 25, 'BETA700001', 5
exec AltaDetalleVenta 25, 'CLOT700002', 4
exec AltaEstadoVenta 25
exec CambiarEstado 25, 2
exec CambiarEstado 25, 3
go

exec AltaVenta 3423.00, 'Av. Rondeau 4567', '22345678', 'pabmende',''
exec AltaDetalleVenta 26, 'MICO700003', 6
exec AltaDetalleVenta 26, 'KETO700004', 3
exec AltaEstadoVenta 26
exec CambiarEstado 26, 2
exec CambiarEstado 26, 3
go

exec AltaVenta 4064.75, 'Av. Libertador 5678', '12345670', 'claserra',''
exec AltaDetalleVenta 27, 'HIDRO70005', 5
exec AltaDetalleVenta 27, 'MOME700006', 4
exec AltaEstadoVenta 27
exec CambiarEstado 27, 2
exec CambiarEstado 27, 3
go

exec AltaVenta 2594.75, 'Av. Artigas 6789', '22345670', 'migdiaz1',''
exec AltaDetalleVenta 28, 'TERB700007', 5
exec AltaDetalleVenta 28, 'ACIC700008', 3
exec AltaEstadoVenta 28
exec CambiarEstado 28, 2
exec CambiarEstado 28, 3
go

exec AltaVenta 1683.50, 'Av. Ellauri 7890', '32345671', 'laumarti',''
exec AltaDetalleVenta 29, 'METO800001', 6
exec AltaDetalleVenta 29, 'DIME800002', 2
exec AltaEstadoVenta 29
exec CambiarEstado 29, 2
exec CambiarEstado 29, 3
go

exec AltaVenta 2205.50, 'Av. Sarmiento 8901', '42345671', 'soflopez',''
exec AltaDetalleVenta 30, 'BISAC80003', 4
exec AltaDetalleVenta 30, 'LACTU80004', 5
exec AltaEstadoVenta 30
exec CambiarEstado 30, 2
exec CambiarEstado 30, 3
go
exec AltaVenta 981.25, 'Av. Gonzalo Ramírez 1234', '12345679', 'juaperez',''
exec AltaDetalleVenta 31, 'IBUP100001', 1
exec AltaDetalleVenta 31, 'PARA100002', 2
exec AltaDetalleVenta 31, 'ASPI100003', 1
exec AltaDetalleVenta 31, 'KETO100004', 1
exec AltaEstadoVenta 31
go

exec AltaVenta 1526.50, 'Av. San Martín 2345', '22345678', 'marrodri',''
exec AltaDetalleVenta 32, 'DIPI100005', 1
exec AltaDetalleVenta 32, 'TRAM100006', 1
exec AltaDetalleVenta 32, 'NAPR100007', 2
exec AltaDetalleVenta 32, 'DICL100008', 1
exec AltaEstadoVenta 32
go

exec AltaVenta 2727.00, 'Av. Brasil 3456', '32345678', 'cargomez',''
exec AltaDetalleVenta 33, 'AMOX200001', 1
exec AltaDetalleVenta 33, 'AZIT200002', 2
exec AltaDetalleVenta 33, 'CEFL200003', 1
exec AltaDetalleVenta 33, 'CLAR200004', 1
exec AltaEstadoVenta 33
go

exec AltaVenta 1997.00, 'Av. Rondeau 4567', '42345678', 'anafern',''
exec AltaDetalleVenta 34, 'CIPR200005', 1
exec AltaDetalleVenta 34, 'DOXL200006', 2
exec AltaDetalleVenta 34, 'METR200007', 1
exec AltaDetalleVenta 34, 'NITR200008', 1
exec AltaEstadoVenta 34
go

exec AltaVenta 1517.25, 'Av. Libertador 5678', '52345678', 'luigonza',''
exec AltaDetalleVenta 35, 'DEXA300001', 1
exec AltaDetalleVenta 35, 'PRED300002', 2
exec AltaDetalleVenta 35, 'MELO300003', 1
exec AltaDetalleVenta 35, 'INDO300004', 1
exec AltaEstadoVenta 35
go

exec AltaVenta 2217.25, 'Av. Artigas 6789', '42345678', 'pabmende',''
exec AltaDetalleVenta 36, 'ETOD300005', 1
exec AltaDetalleVenta 36, 'CELE300006', 2
exec AltaDetalleVenta 36, 'NABU300007', 1
exec AltaDetalleVenta 36, 'PIRO300008', 1
exec AltaEstadoVenta 36
go

exec AltaVenta 1882.00, 'Av. Ellauri 7890', '32345678', 'claserra',''
exec AltaDetalleVenta 37, 'VITC400001', 1
exec AltaDetalleVenta 37, 'VITD400002', 2
exec AltaDetalleVenta 37, 'VITB400003', 1
exec AltaDetalleVenta 37, 'VITA400004', 1
exec AltaEstadoVenta 37
go

exec AltaVenta 1322.25, 'Av. Sarmiento 8901', '45434157', 'migdiaz1',''
exec AltaDetalleVenta 38, 'VITE400005', 1
exec AltaDetalleVenta 38, 'ZINK400006', 2
exec AltaDetalleVenta 38, 'CALC400007', 1
exec AltaDetalleVenta 38, 'MAGN400008', 1
exec AltaEstadoVenta 38
go

exec AltaVenta 1632.25, 'Av. Burgues 9012', '12345670', 'laumarti',''
exec AltaDetalleVenta 39, 'OMEP500001', 1
exec AltaDetalleVenta 39, 'RANI500002', 2
exec AltaDetalleVenta 39, 'ESOP500003', 1
exec AltaDetalleVenta 39, 'MAAL500004', 1
exec AltaEstadoVenta 39
go

exec AltaVenta 1572.50, 'Av. Joaquín Suárez 0123', '22345670', 'soflopez',''
exec AltaDetalleVenta 40, 'FAMO500005', 1
exec AltaDetalleVenta 40, 'PANT500006', 2
exec AltaDetalleVenta 40, 'BICAR50007', 1
exec AltaDetalleVenta 40, 'HIDR500008', 1
exec AltaEstadoVenta 40
go



exec AltaVenta 250.50, 'Av. 18 de Julio 1000', '12345679', 'juaperez',''
exec AltaDetalleVenta 41, 'IBUP100001', 1
exec AltaEstadoVenta 41


go
exec AltaVenta 360.00, 'Av. Rivera 2020', '22345678', 'marrodri',''
exec AltaDetalleVenta 42, 'PARA100002', 2
exec AltaEstadoVenta 42

go
exec AltaVenta 440.00, 'Av. Italia 3030', '32345678', 'cargomez',''
exec AltaDetalleVenta 43, 'ASPI100003', 2
exec AltaEstadoVenta 43

go
exec AltaVenta 301.50, 'Av. Garzón 4040', '42345678', 'anafern',''
exec AltaDetalleVenta 44, 'KETO100004', 2
exec AltaEstadoVenta 44

go
exec AltaVenta 390.50, 'Av. Agraciada 5050', '52345678', 'luigonza',''
exec AltaDetalleVenta 45, 'DIPI100005', 2
exec AltaEstadoVenta 45

go
exec AltaVenta 900.00, 'Av. 8 de Octubre 6060', '42345678', 'pabmende',''
exec AltaDetalleVenta 46, 'TRAM100006', 2
exec AltaEstadoVenta 46

go
exec AltaVenta 561.00, 'Av. Luis Alberto de Herrera 7070', '32345678', 'claserra',''
exec AltaDetalleVenta 47, 'NAPR100007', 2
exec AltaEstadoVenta 47

go
exec AltaVenta 640.50, 'Av. Millán 8080', '45434157', 'migdiaz1',''
exec AltaDetalleVenta 48, 'DICL100008', 2
exec AltaEstadoVenta 48

go
exec AltaVenta 840.00, 'Av. Burgues 9090', '12345670', 'laumarti',''
exec AltaDetalleVenta 49, 'AMOX200001', 2
exec AltaEstadoVenta 49

go
exec AltaVenta 1161.00, 'Av. Lezica 1010', '22345670', 'soflopez',''
exec AltaDetalleVenta 50, 'AZIT200002', 2
exec AltaEstadoVenta 50
go

exec AltaVenta 280.00, 'Av. Gonzalo Ramírez 1234', '12345671', 'laumarti',''
exec AltaDetalleVenta 51, 'VITC400001', 1
exec AltaEstadoVenta 51
exec CambiarEstado 51, 2

go
exec AltaVenta 450.50, 'Av. San Martín 2345', '22345671', 'soflopez',''
exec AltaDetalleVenta 52, 'VITD400002', 1
exec AltaEstadoVenta 52
exec CambiarEstado 52, 2

go
exec AltaVenta 320.25, 'Av. Brasil 3456', '32345671', 'juaperez',''
exec AltaDetalleVenta 53, 'VITB400003', 1
exec AltaEstadoVenta 53
exec CambiarEstado 53, 2

go
exec AltaVenta 380.75, 'Av. Rondeau 4567', '42345671', 'marrodri',''
exec AltaDetalleVenta 54, 'VITA400004', 1
exec AltaEstadoVenta 54
exec CambiarEstado 54, 2

go
exec AltaVenta 290.50, 'Av. Libertador 5678', '2345671', 'cargomez',''
exec AltaDetalleVenta 55, 'VITE400005', 1
exec AltaEstadoVenta 55
exec CambiarEstado 55, 2

go
exec AltaVenta 180.25, 'Av. Artigas 6789', '45434157', 'anafern',''
exec AltaDetalleVenta 56, 'ZINK400006', 1
exec AltaEstadoVenta 56
exec CambiarEstado 56, 2

go
exec AltaVenta 420.75, 'Av. Ellauri 7890', '32345671', 'luigonza',''
exec AltaDetalleVenta 57, 'CALC400007', 1
exec AltaEstadoVenta 57
exec CambiarEstado 57, 2

go
exec AltaVenta 250.50, 'Av. Sarmiento 8901', '22345678', 'pabmende',''
exec AltaDetalleVenta 58, 'MAGN400008', 1
exec AltaEstadoVenta 58
exec CambiarEstado 58, 2

go
exec AltaVenta 380.50, 'Av. Vaz Ferreira 9012', '12345670', 'claserra',''
exec AltaDetalleVenta 59, 'OMEP500001', 1
exec AltaEstadoVenta 59
exec CambiarEstado 59, 2

go
exec AltaVenta 290.25, 'Av. Joaquín Suárez 0123', '22345670', 'migdiaz1',''
exec AltaDetalleVenta 60, 'RANI500002', 1
exec AltaEstadoVenta 60
exec CambiarEstado 60, 2

go
exec AltaVenta 320.50, 'Av. Uruguay 1234', '12345671', 'laumarti',''
exec AltaDetalleVenta 61, 'DEXA300001', 1
exec AltaEstadoVenta 61
exec CambiarEstado 61, 2  
exec CambiarEstado 61, 3  

go
exec AltaVenta 280.75, 'Av. Paraguay 2345', '22345671', 'soflopez',''
exec AltaDetalleVenta 62, 'PRED300002', 1
exec AltaEstadoVenta 62
exec CambiarEstado 62, 2  
exec CambiarEstado 62, 3 

go
exec AltaVenta 390.25, 'Av. Argentina 3456', '32345671', 'juaperez',''
exec AltaDetalleVenta 63, 'MELO300003', 1
exec AltaEstadoVenta 63
exec CambiarEstado 63, 2  
exec CambiarEstado 63, 3  

go
exec AltaVenta 245.00, 'Av. Bolivia 4567', '42345671', 'marrodri',''
exec AltaDetalleVenta 64, 'INDO300004', 1
exec AltaEstadoVenta 64
exec CambiarEstado 64, 2  
exec CambiarEstado 64, 3  


exec AltaVenta 420.50, 'Av. Chile 5678', '2345671', 'cargomez',''
exec AltaDetalleVenta 65, 'ETOD300005', 1
exec AltaEstadoVenta 65
exec CambiarEstado 65, 2  
exec CambiarEstado 65, 3  

go
exec AltaVenta 580.25, 'Av. Perú 6789', '45434157', 'anafern',''
exec AltaDetalleVenta 66, 'CELE300006', 1
exec AltaEstadoVenta 66
exec CambiarEstado 66, 2  
exec CambiarEstado 66, 3 

go
exec AltaVenta 340.75, 'Av. Colombia 7890', '32345671', 'luigonza',''
exec AltaDetalleVenta 67, 'NABU300007', 1
exec AltaEstadoVenta 67
exec CambiarEstado 67, 2  
exec CambiarEstado 67, 3 

go
exec AltaVenta 295.50, 'Av. Venezuela 8901', '22345678', 'pabmende',''
exec AltaDetalleVenta 68, 'PIRO300008', 1
exec AltaEstadoVenta 68
exec CambiarEstado 68, 2  
exec CambiarEstado 68, 3  

go
exec AltaVenta 380.50, 'Av. Ecuador 9012', '12345670', 'claserra',''
exec AltaDetalleVenta 69, 'OMEP500001', 1
exec AltaEstadoVenta 69
exec CambiarEstado 69, 2 
exec CambiarEstado 69, 3  

go
exec AltaVenta 290.25, 'Av. Guyana 0123', '22345670', 'migdiaz1',''
exec AltaDetalleVenta 70, 'RANI500002', 1
exec AltaEstadoVenta 70
exec CambiarEstado 70, 2  
exec CambiarEstado 70, 3 

go

exec AltaVenta 830.50, 'Av. Gonzalo Ramírez 1234', '12345679', 'juaperez',''
exec AltaDetalleVenta 71, 'IBUP100001', 1
exec AltaDetalleVenta 71, 'PARA100002', 2
exec AltaDetalleVenta 71, 'ASPI100003', 1
exec AltaEstadoVenta 71
go

exec AltaVenta 991.25, 'Av. San Martín 2345', '22345678', 'marrodri',''
exec AltaDetalleVenta 72, 'KETO100004', 1
exec AltaDetalleVenta 72, 'DIPI100005', 2
exec AltaDetalleVenta 72, 'TRAM100006', 1
exec AltaEstadoVenta 72
go

exec AltaVenta 1301.25, 'Av. Brasil 3456', '32345678', 'cargomez',''
exec AltaDetalleVenta 73, 'NAPR100007', 2
exec AltaDetalleVenta 73, 'DICL100008', 1
exec AltaDetalleVenta 73, 'AMOX200001', 1
exec AltaEstadoVenta 73
go

exec AltaVenta 2307.00, 'Av. Rondeau 4567', '42345678', 'anafern',''
exec AltaDetalleVenta 74, 'AZIT200002', 2
exec AltaDetalleVenta 74, 'CEFL200003', 1
exec AltaDetalleVenta 74, 'CLAR200004', 1
exec AltaEstadoVenta 74
go

exec AltaVenta 1706.75, 'Av. Libertador 5678', '52345678', 'luigonza',''
exec AltaDetalleVenta 75, 'CIPR200005', 1
exec AltaDetalleVenta 75, 'DOXL200006', 2
exec AltaDetalleVenta 75, 'METR200007', 1
exec AltaEstadoVenta 75
go

exec AltaVenta 1472.00, 'Av. Artigas 6789', '42345678', 'pabmende',''
exec AltaDetalleVenta 76, 'NITR200008', 3
exec AltaDetalleVenta 76, 'DEXA300001', 1
exec AltaDetalleVenta 76, 'PRED300002', 1
exec AltaEstadoVenta 76
go

exec AltaVenta 1446.00, 'Av. Ellauri 7890', '32345678', 'claserra',''
exec AltaDetalleVenta 77, 'MELO300003', 2
exec AltaDetalleVenta 77, 'INDO300004', 1
exec AltaDetalleVenta 77, 'ETOD300005', 1
exec AltaEstadoVenta 77
go

exec AltaVenta 2193.50, 'Av. Sarmiento 8901', '45434157', 'migdiaz1',''
exec AltaDetalleVenta 78, 'CELE300006', 1
exec AltaDetalleVenta 78, 'NABU300007', 3
exec AltaDetalleVenta 78, 'PIRO300008', 2
exec AltaEstadoVenta 78
go

exec AltaVenta 2621.25, 'Av. Burgues 9012', '12345670', 'laumarti',''
exec AltaDetalleVenta 79, 'VITC400001', 5
exec AltaDetalleVenta 79, 'VITD400002', 2
exec AltaDetalleVenta 79, 'VITB400003', 1
exec AltaEstadoVenta 79
go

exec AltaVenta 2444.25, 'Av. Joaquín Suárez 0123', '22345670', 'soflopez',''
exec AltaDetalleVenta 80, 'VITA400004', 3
exec AltaDetalleVenta 80, 'VITE400005', 2
exec AltaDetalleVenta 80, 'ZINK400006', 4
exec AltaEstadoVenta 80
go

exec AltaVenta 830.50, 'Av. Gonzalo Ramírez 1234', '12345679', 'juaperez',''
exec AltaDetalleVenta 81, 'IBUP100001', 1
exec AltaDetalleVenta 81, 'PARA100002', 2
exec AltaDetalleVenta 81, 'ASPI100003', 1
exec AltaEstadoVenta 81
exec CambiarEstado 81, 2
go

exec AltaVenta 991.25, 'Av. San Martín 2345', '22345678', 'marrodri',''
exec AltaDetalleVenta 82, 'KETO100004', 1
exec AltaDetalleVenta 82, 'DIPI100005', 2
exec AltaDetalleVenta 82, 'TRAM100006', 1
exec AltaEstadoVenta 82
exec CambiarEstado 82, 2
go

exec AltaVenta 1301.25, 'Av. Brasil 3456', '32345678', 'cargomez',''
exec AltaDetalleVenta 83, 'NAPR100007', 2
exec AltaDetalleVenta 83, 'DICL100008', 1
exec AltaDetalleVenta 83, 'AMOX200001', 1
exec AltaEstadoVenta 83
exec CambiarEstado 83, 2
go

exec AltaVenta 2307, 'Av. Rondeau 4567', '42345678', 'anafern',''
exec AltaDetalleVenta 84, 'AZIT200002', 2
exec AltaDetalleVenta 84, 'CEFL200003', 1
exec AltaDetalleVenta 84, 'CLAR200004', 1
exec AltaEstadoVenta 84
exec CambiarEstado 84, 2
go

exec AltaVenta 1706.75, 'Av. Libertador 5678', '52345678', 'luigonza',''
exec AltaDetalleVenta 85, 'CIPR200005', 1
exec AltaDetalleVenta 85, 'DOXL200006', 2
exec AltaDetalleVenta 85, 'METR200007', 1
exec AltaEstadoVenta 85
exec CambiarEstado 85, 2
go

exec AltaVenta 1472.00, 'Av. Artigas 6789', '42345678', 'pabmende',''
exec AltaDetalleVenta 86, 'NITR200008', 3
exec AltaDetalleVenta 86, 'DEXA300001', 1
exec AltaDetalleVenta 86, 'PRED300002', 1
exec AltaEstadoVenta 86
exec CambiarEstado 86, 2
go

exec AltaVenta 1446.00, 'Av. Ellauri 7890', '32345678', 'claserra',''
exec AltaDetalleVenta 87, 'MELO300003', 2
exec AltaDetalleVenta 87, 'INDO300004', 1
exec AltaDetalleVenta 87, 'ETOD300005', 1
exec AltaEstadoVenta 87
exec CambiarEstado 87, 2
go

exec AltaVenta 2193.50, 'Av. Sarmiento 8901', '45434157', 'migdiaz1',''
exec AltaDetalleVenta 88, 'CELE300006', 1
exec AltaDetalleVenta 88, 'NABU300007', 3
exec AltaDetalleVenta 88, 'PIRO300008', 2
exec AltaEstadoVenta 88
exec CambiarEstado 88, 2
go

exec AltaVenta 2621.25, 'Av. Burgues 9012', '12345670', 'laumarti',''
exec AltaDetalleVenta 89, 'VITC400001', 5
exec AltaDetalleVenta 89, 'VITD400002', 2
exec AltaDetalleVenta 89, 'VITB400003', 1
exec AltaEstadoVenta 89
exec CambiarEstado 89, 2
go

exec AltaVenta 2444.25, 'Av. Joaquín Suárez 0123', '22345670', 'soflopez',''
exec AltaDetalleVenta 90, 'VITA400004', 3
exec AltaDetalleVenta 90, 'VITE400005', 2
exec AltaDetalleVenta 90, 'ZINK400006', 4
exec AltaEstadoVenta 90
exec CambiarEstado 90, 2
go

exec AltaVenta 830.50, 'Av. Gonzalo Ramírez 1234', '12345679', 'juaperez',''
exec AltaDetalleVenta 91, 'IBUP100001', 1
exec AltaDetalleVenta 91, 'PARA100002', 2
exec AltaDetalleVenta 91, 'ASPI100003', 1
exec AltaEstadoVenta 91
exec CambiarEstado 91, 2
exec CambiarEstado 91, 3
go

exec AltaVenta 991.25, 'Av. San Martín 2345', '22345678', 'marrodri',''
exec AltaDetalleVenta 92, 'KETO100004', 1
exec AltaDetalleVenta 92, 'DIPI100005', 2
exec AltaDetalleVenta 92, 'TRAM100006', 1
exec AltaEstadoVenta 92
exec CambiarEstado 92, 2
exec CambiarEstado 92, 3
go

exec AltaVenta 1301.25, 'Av. Brasil 3456', '32345678', 'cargomez',''
exec AltaDetalleVenta 93, 'NAPR100007', 2
exec AltaDetalleVenta 93, 'DICL100008', 1
exec AltaDetalleVenta 93, 'AMOX200001', 1
exec AltaEstadoVenta 93
exec CambiarEstado 93, 2
exec CambiarEstado 93, 3
go

exec AltaVenta 2307.00, 'Av. Rondeau 4567', '42345678', 'anafern',''
exec AltaDetalleVenta 94, 'AZIT200002', 2
exec AltaDetalleVenta 94, 'CEFL200003', 1
exec AltaDetalleVenta 94, 'CLAR200004', 1
exec AltaEstadoVenta 94
exec CambiarEstado 94, 2
exec CambiarEstado 94, 3
go

exec AltaVenta 1706.75, 'Av. Libertador 5678', '52345678', 'luigonza',''
exec AltaDetalleVenta 95, 'CIPR200005', 1
exec AltaDetalleVenta 95, 'DOXL200006', 2
exec AltaDetalleVenta 95, 'METR200007', 1
exec AltaEstadoVenta 95
exec CambiarEstado 95, 2
exec CambiarEstado 95, 3
go

exec AltaVenta 1472.00, 'Av. Artigas 6789', '42345678', 'pabmende',''
exec AltaDetalleVenta 96, 'NITR200008', 3
exec AltaDetalleVenta 96, 'DEXA300001', 1
exec AltaDetalleVenta 96, 'PRED300002', 1
exec AltaEstadoVenta 96
exec CambiarEstado 96, 2
exec CambiarEstado 96, 3
go

exec AltaVenta 1446.00, 'Av. Ellauri 7890', '32345678', 'claserra',''
exec AltaDetalleVenta 97, 'MELO300003', 2
exec AltaDetalleVenta 97, 'INDO300004', 1
exec AltaDetalleVenta 97, 'ETOD300005', 1
exec AltaEstadoVenta 97
exec CambiarEstado 97, 2
exec CambiarEstado 97, 3
go

exec AltaVenta 2193.50, 'Av. Sarmiento 8901', '45434157', 'migdiaz1',''
exec AltaDetalleVenta 98, 'CELE300006', 1
exec AltaDetalleVenta 98, 'NABU300007', 3
exec AltaDetalleVenta 98, 'PIRO300008', 2
exec AltaEstadoVenta 98
exec CambiarEstado 98, 2
exec CambiarEstado 98, 3
go

exec AltaVenta 2621.25, 'Av. Burgues 9012', '12345670', 'laumarti',''
exec AltaDetalleVenta 99, 'VITC400001', 5
exec AltaDetalleVenta 99, 'VITD400002', 2
exec AltaDetalleVenta 99, 'VITB400003', 1
exec AltaEstadoVenta 99
exec CambiarEstado 99, 2
exec CambiarEstado 99, 3
go

exec AltaVenta 2444.25, 'Av. Joaquín Suárez 0123', '22345670', 'soflopez',''
exec AltaDetalleVenta 100, 'VITA400004', 3
exec AltaDetalleVenta 100, 'VITE400005', 2
exec AltaDetalleVenta 100, 'ZINK400006', 4
exec AltaEstadoVenta 100
exec CambiarEstado 100, 2
exec CambiarEstado 100, 3
go