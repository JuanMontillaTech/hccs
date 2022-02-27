/****** Script for SelectTopNRows command from SSMS  ******/
 SELECT   
    (SELECT [nombre] FROM [dbo].[Catalogo] where numero_cuenta = ( SELECT SUBSTRING([numero_cuenta] , 1, 1)  FROM [dbo].[Catalogo]  where id =  ac.idcuenta )  ) as TIPO,
    (select  SUM( [credito] )  FROM [dbo].[asientos_contable] where idcuenta = ac.idcuenta)  as CREDITO,
    (select  SUM( [debito] )    FROM [dbo].[asientos_contable] where idcuenta = ac.idcuenta) as DEBITO,
	 CASE
	WHEN  
		(select  SUM( [credito] )  FROM [dbo].[asientos_contable] where idcuenta = ac.idcuenta) >  
		(select  SUM( [debito] )    FROM [dbo].[asientos_contable] where idcuenta = ac.idcuenta) 
	THEN
		((select  SUM( [credito] )    FROM [dbo].[asientos_contable] where idcuenta = ac.idcuenta)) -  (select  SUM( [debito] )  FROM [dbo].[asientos_contable] where idcuenta = ac.idcuenta)  
    ELSE ((select  SUM( [debito] )    FROM [dbo].[asientos_contable] where idcuenta = ac.idcuenta)) -  (select  SUM( [credito] )  FROM [dbo].[asientos_contable] where idcuenta = ac.idcuenta)  
	END AS BALANCE
, ac.IDCUENTA, (SELECT [numero_cuenta] + '-'+ [nombre] FROM [dbo].[Catalogo] WHERE  [id] =ac.idcuenta )  AS CUENTA
  FROM [dbo].[asientos_contable]   ac
  GROUP BY idcuenta 


 



   

   