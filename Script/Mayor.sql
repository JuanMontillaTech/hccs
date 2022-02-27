/****** Script for SelectTopNRows command from SSMS  ******/

  
  SELECT   
  
    (select  SUM( [credito] )  FROM [dbo].[asientos_contable] where idcuenta = ac.idcuenta)  as CREDITO,
    (select  SUM( [debito] )    FROM [dbo].[asientos_contable] where idcuenta = ac.idcuenta) as DEBITO,	 
 ac.IDCUENTA, (SELECT [numero_cuenta] + '-'+ [nombre] FROM [dbo].[Catalogo] WHERE  [id] =ac.idcuenta )  AS CUENTA
  FROM [dbo].[asientos_contable]   ac
  GROUP BY idcuenta 


