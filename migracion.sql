IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AccessRequests] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [Entity] varchar(60) NOT NULL,
    [Name] varchar(60) NOT NULL,
    [Email] varchar(100) NOT NULL,
    [Telephone] varchar(16) NULL,
    CONSTRAINT [PK_AccessRequests] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [account] (
    [Id] int NOT NULL IDENTITY,
    [idTipoCuenta] int NOT NULL,
    [Nombre_Cuenta] nvarchar(max) NOT NULL,
    [Numero_Cuenta] nvarchar(max) NOT NULL,
    [Saldo_Deuda] decimal(18,2) NULL,
    [Fecha] datetime2 NULL,
    [Decripcion] nvarchar(max) NULL,
    [idEntidad] int NOT NULL,
    [idCuentacontable] int NULL,
    CONSTRAINT [PK_account] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [asientos_contable] (
    [id] int NOT NULL IDENTITY,
    [aciento] nvarchar(max) NULL,
    [fecha] datetime2 NOT NULL,
    [tercero] int NOT NULL,
    [codigo] nvarchar(max) NULL,
    [cuenta_contable] nvarchar(max) NULL,
    [debito] decimal(18,2) NOT NULL,
    [credito] decimal(18,2) NOT NULL,
    [idcuenta] int NOT NULL,
    [idContacto] int NOT NULL,
    [descripcion] nvarchar(max) NULL,
    [identidad] int NOT NULL,
    [iddiaro] int NOT NULL,
    [IDTransaction] uniqueidentifier NULL,
    CONSTRAINT [PK_asientos_contable] PRIMARY KEY ([id])
);

GO

CREATE TABLE [BankTransaction] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Balance] decimal(18,2) NOT NULL,
    [Number] nvarchar(max) NULL,
    [IDBank] int NOT NULL,
    [TransactionType] int NOT NULL,
    [IDTransaction] uniqueidentifier NULL,
    CONSTRAINT [PK_BankTransaction] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [BoxBalances] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [Balance] decimal(18,2) NOT NULL,
    [MonthBalance] datetime2 NOT NULL,
    CONSTRAINT [PK_BoxBalances] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [BoxSmalls] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [Identidad] int NOT NULL,
    [Date] datetime2 NOT NULL,
    [Refund] decimal(18,2) NOT NULL,
    [Fund] decimal(18,2) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_BoxSmalls] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Catalogo] (
    [Id] int NOT NULL IDENTITY,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [Numero_cuenta] nvarchar(max) NOT NULL,
    [Nombre] nvarchar(max) NOT NULL,
    [Descripcion] nvarchar(max) NULL,
    CONSTRAINT [PK_Catalogo] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [CatalogoRoll] (
    [Id] int NOT NULL IDENTITY,
    [RollCatalogo] nvarchar(max) NULL,
    CONSTRAINT [PK_CatalogoRoll] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Catalogs] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [Account] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    [Movement] nvarchar(max) NULL,
    [Center] nvarchar(max) NULL,
    [Tax] nvarchar(max) NULL,
    CONSTRAINT [PK_Catalogs] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Contacto] (
    [id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NOT NULL,
    [rnc] nvarchar(max) NULL,
    [direccion] nvarchar(max) NULL,
    [email] nvarchar(max) NULL,
    [telefono1] nvarchar(max) NULL,
    [telefono2] nvarchar(max) NULL,
    [fax] nvarchar(max) NULL,
    [celular] nvarchar(max) NULL,
    [cliente] bit NOT NULL,
    [proveedor] bit NOT NULL,
    [idEntidad] int NOT NULL,
    [idRegCatalogo] int NOT NULL,
    CONSTRAINT [PK_Contacto] PRIMARY KEY ([id])
);

GO

CREATE TABLE [Diario] (
    [id] int NOT NULL IDENTITY,
    [fecha] datetime2 NOT NULL,
    [regerencia] nvarchar(max) NULL,
    [observaciones] nvarchar(max) NULL,
    [identidad] int NOT NULL,
    [idusuario] int NOT NULL,
    [Debito] decimal(18,2) NOT NULL,
    [Credito] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Diario] PRIMARY KEY ([id])
);

GO

CREATE TABLE [DocNumber] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [Document] nvarchar(max) NULL,
    CONSTRAINT [PK_DocNumber] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Employees] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [Cedula] char(11) NOT NULL,
    [Name] varchar(30) NOT NULL,
    [LastName] varchar(30) NOT NULL,
    [DiscountLoan] decimal(8,2) NULL,
    [HiringDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Files] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [DisplayName] nvarchar(max) NULL,
    [PhysicalName] nvarchar(max) NULL,
    [ContentType] nvarchar(max) NULL,
    [Folder] nvarchar(max) NULL,
    [SourceId] uniqueidentifier NULL,
    [Commentary] nvarchar(max) NULL,
    CONSTRAINT [PK_Files] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Formas_Pagos] (
    [id] int NOT NULL IDENTITY,
    [Forma_Pago] nvarchar(max) NOT NULL,
    [idEntidad] int NOT NULL,
    CONSTRAINT [PK_Formas_Pagos] PRIMARY KEY ([id])
);

GO

CREATE TABLE [JournaDetails] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [Contact] int NOT NULL,
    [Account] int NOT NULL,
    [Debit] decimal(18,2) NOT NULL,
    [Credit] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_JournaDetails] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Journals] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [Code] nvarchar(max) NULL,
    [Date] datetime2 NOT NULL,
    CONSTRAINT [PK_Journals] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [LedgerAccounts] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [Belongs] uniqueidentifier NULL,
    [Name] nvarchar(max) NULL,
    [Code] nvarchar(max) NULL,
    [Commentary] nvarchar(max) NULL,
    [Nature] int NOT NULL,
    [LocationStatusResult] int NOT NULL,
    CONSTRAINT [PK_LedgerAccounts] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Master] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [MenuTitle] nvarchar(max) NULL,
    [Head] nvarchar(max) NULL,
    [TableName] nvarchar(max) NULL,
    [ShowList] bit NOT NULL,
    [ShowForm] bit NOT NULL,
    [Api] nvarchar(max) NULL,
    [URL] nvarchar(max) NULL,
    [Type] int NOT NULL,
    [Icon] nvarchar(max) NULL,
    [ModuleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Master] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [MasterColumn] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [MasterId] uniqueidentifier NOT NULL,
    [ColumnType] int NOT NULL,
    [ColumnName] nvarchar(max) NULL,
    [ColumnTitle] nvarchar(max) NULL,
    [ColumnIndex] int NOT NULL,
    [AipSource] nvarchar(max) NULL,
    [SourceName] nvarchar(max) NULL,
    [SourceValue] nvarchar(max) NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_MasterColumn] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Module] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [Name] nvarchar(max) NULL,
    [Icon] nvarchar(max) NULL,
    CONSTRAINT [PK_Module] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Numeraciones] (
    [id] int NOT NULL IDENTITY,
    [idTipoDocumento] int NOT NULL,
    [Nombre] nvarchar(max) NOT NULL,
    [Automatica] bit NOT NULL,
    [Prefijo] nvarchar(max) NULL,
    [Secuencia] int NOT NULL,
    [Numero_final] int NOT NULL,
    [Desde] datetime2 NOT NULL,
    [Hasta] datetime2 NOT NULL,
    [Preferida] bit NOT NULL,
    [Sucursal] nvarchar(max) NULL,
    [Pie_Factura] nvarchar(max) NULL,
    [Grupo] int NOT NULL,
    [IdEntidad] int NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Numeraciones] PRIMARY KEY ([id])
);

GO

CREATE TABLE [Origen] (
    [id] int NOT NULL IDENTITY,
    [nombre] nvarchar(max) NULL,
    [origen] int NOT NULL,
    CONSTRAINT [PK_Origen] PRIMARY KEY ([id])
);

GO

CREATE TABLE [Pais] (
    [id] int NOT NULL IDENTITY,
    [descripcion] nvarchar(max) NOT NULL,
    [Moneda] nvarchar(max) NULL,
    [Entity] int NOT NULL,
    CONSTRAINT [PK_Pais] PRIMARY KEY ([id])
);

GO

CREATE TABLE [Rol] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_Rol] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [RollModule] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [RollId] uniqueidentifier NOT NULL,
    [ModuleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_RollModule] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Sisters] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NOT NULL,
    [Direccion] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Telefono1] nvarchar(max) NULL,
    [Telefono2] nvarchar(max) NULL,
    [Fax] nvarchar(max) NULL,
    [Celular] nvarchar(max) NULL,
    [Cliente] bit NOT NULL,
    [Proveedor] bit NOT NULL,
    [IdEntidad] int NOT NULL,
    [IdRegCatalogo] int NOT NULL,
    CONSTRAINT [PK_Sisters] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Tipo] (
    [id] int NOT NULL IDENTITY,
    [origenID] int NOT NULL,
    [cuenta] nvarchar(max) NULL,
    [nombre] nvarchar(max) NOT NULL,
    [tipoid] int NOT NULL,
    CONSTRAINT [PK_Tipo] PRIMARY KEY ([id])
);

GO

CREATE TABLE [UnidadMedida] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [Padre] uniqueidentifier NULL,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_UnidadMedida] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Usuario] (
    [id] int NOT NULL IDENTITY,
    [email] nvarchar(max) NOT NULL,
    [password] nvarchar(max) NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY ([id])
);

GO

CREATE TABLE [UsuarioEntidad] (
    [id] int NOT NULL IDENTITY,
    [idusuario] int NOT NULL,
    [identidad] int NOT NULL,
    CONSTRAINT [PK_UsuarioEntidad] PRIMARY KEY ([id])
);

GO

CREATE TABLE [WalletBox] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [Date] datetime2 NOT NULL,
    [Box] decimal(18, 2) NOT NULL,
    [UnderBox] decimal(18, 2) NOT NULL,
    [Description] nvarchar(150) NULL,
    CONSTRAINT [PK_WalletBox] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [BoxSmallDetails] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [Date] int NOT NULL,
    [Receipt] int NOT NULL,
    [Details] nvarchar(max) NULL,
    [Amount] decimal(18,2) NOT NULL,
    [AccountNumber] nvarchar(max) NULL,
    [BoxSmallId] uniqueidentifier NULL,
    CONSTRAINT [PK_BoxSmallDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BoxSmallDetails_BoxSmalls_BoxSmallId] FOREIGN KEY ([BoxSmallId]) REFERENCES [BoxSmalls] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [CatalogoRollGrupo] (
    [Id] int NOT NULL IDENTITY,
    [IdCatalogo] int NULL,
    [IdCatalogoRoll] int NULL,
    [Tipo] int NOT NULL,
    CONSTRAINT [PK_CatalogoRollGrupo] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CatalogoRollGrupo_Catalogo_IdCatalogo] FOREIGN KEY ([IdCatalogo]) REFERENCES [Catalogo] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CatalogoRollGrupo_CatalogoRoll_IdCatalogoRoll] FOREIGN KEY ([IdCatalogoRoll]) REFERENCES [CatalogoRoll] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Entidad] (
    [id] int NOT NULL IDENTITY,
    [IdCatalgoRoll] int NULL,
    [nombre] nvarchar(max) NOT NULL,
    [idpais] int NOT NULL,
    [pueblo] nvarchar(max) NULL,
    [Identificador_Fiscal] nvarchar(max) NULL,
    [direccion] nvarchar(max) NULL,
    [telefono] nvarchar(max) NULL,
    [Madre_amadora] nvarchar(max) NULL,
    [Cantidad_hermanas] int NOT NULL,
    [Cantidad_empleados] int NOT NULL,
    [Aporte_Mensual] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Entidad] PRIMARY KEY ([id]),
    CONSTRAINT [FK_Entidad_CatalogoRoll_IdCatalgoRoll] FOREIGN KEY ([IdCatalgoRoll]) REFERENCES [CatalogoRoll] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [EmployeePayrolls] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [GrossSalary] decimal(8,2) NOT NULL,
    [OccupationalHazard] decimal(8,2) NOT NULL,
    [Fase] decimal(8,2) NULL,
    [Date] datetime2 NOT NULL,
    [EmployeeId] uniqueidentifier NOT NULL,
    [InstituticionId] int NOT NULL,
    CONSTRAINT [PK_EmployeePayrolls] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EmployeePayrolls_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Currencies] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [Code] char(3) NOT NULL,
    [Divisa] varchar(40) NOT NULL,
    [Symbol] varchar(4) NOT NULL,
    [Countryid] int NULL,
    [Entity] int NOT NULL,
    CONSTRAINT [PK_Currencies] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Currencies_Pais_Countryid] FOREIGN KEY ([Countryid]) REFERENCES [Pais] ([id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Articulo] (
    [Id] int NOT NULL IDENTITY,
    [IdTipoArticulo] int NOT NULL,
    [Nombre] nvarchar(max) NOT NULL,
    [Cantidad] int NOT NULL,
    [Precio] decimal(18,2) NOT NULL,
    [Preciocompra] decimal(18,2) NULL,
    [Impuesto] decimal(18,2) NULL,
    [UnidadMedidaID] uniqueidentifier NULL,
    [Inventariable] bit NOT NULL,
    [CantidadMinima] int NULL,
    [CantidadMaxima] int NULL,
    [Decripcion] nvarchar(max) NULL,
    [IdEntidad] int NOT NULL,
    [IdCuentacontable] int NULL,
    [CodigoBarra] nvarchar(max) NULL,
    [Ubicacion] nvarchar(max) NULL,
    CONSTRAINT [PK_Articulo] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Articulo_UnidadMedida_UnidadMedidaID] FOREIGN KEY ([UnidadMedidaID]) REFERENCES [UnidadMedida] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [RollUser] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [RollID] uniqueidentifier NULL,
    [UsuarioID] int NOT NULL,
    CONSTRAINT [PK_RollUser] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RollUser_Rol_RollID] FOREIGN KEY ([RollID]) REFERENCES [Rol] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RollUser_Usuario_UsuarioID] FOREIGN KEY ([UsuarioID]) REFERENCES [Usuario] ([id]) ON DELETE CASCADE
);

GO

CREATE TABLE [WalletBoxDetalis] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [EntidadId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [WalletBoxId] uniqueidentifier NOT NULL,
    [Date] datetime2 NOT NULL,
    [Details] nvarchar(max) NULL,
    [Account] nvarchar(max) NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Receipt] nvarchar(max) NULL,
    CONSTRAINT [PK_WalletBoxDetalis] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WalletBoxDetalis_WalletBox_WalletBoxId] FOREIGN KEY ([WalletBoxId]) REFERENCES [WalletBox] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Payrolls] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [Number] int NOT NULL IDENTITY,
    [DiscountHealthInsurance] decimal(8,2) NOT NULL,
    [DiscountAFP] decimal(8,2) NOT NULL,
    [DiscountTotal] decimal(8,2) NOT NULL,
    [NetSalary] decimal(8,2) NOT NULL,
    [EmployerContributionHealthInsurance] decimal(8,2) NOT NULL,
    [EmployerContributionAFP] decimal(8,2) NOT NULL,
    [PayrollDate] datetime2 NOT NULL,
    [EmployeePayrollId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Payrolls] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Payrolls_EmployeePayrolls_EmployeePayrollId] FOREIGN KEY ([EmployeePayrollId]) REFERENCES [EmployeePayrolls] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [TransactionIn] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [Type] int NOT NULL,
    [IdComprobante] int NOT NULL,
    [Comprobante] nvarchar(max) NULL,
    [Proveedor] int NOT NULL,
    [NProveedor] nvarchar(max) NULL,
    [Fecha] datetime2 NOT NULL,
    [FechaVencimiento] datetime2 NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    [Descuento] decimal(18,2) NOT NULL,
    [Pagado] decimal(18,2) NOT NULL,
    [IdEntidad] int NOT NULL,
    [IdEstado] int NOT NULL,
    [Cuenta_Bancaria] int NOT NULL,
    [Formato_pago] int NOT NULL,
    [DocumentNumber] nvarchar(40) NULL,
    [CurrencyId] uniqueidentifier NULL,
    [TRKPagado] uniqueidentifier NULL,
    [Recibo] nvarchar(max) NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_TransactionIn] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TransactionIn_Currencies_CurrencyId] FOREIGN KEY ([CurrencyId]) REFERENCES [Currencies] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [ArticulosTransaction] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [IDArticulo] int NOT NULL,
    [Cantidad] int NOT NULL,
    [TipoArticulosTransaction] int NOT NULL,
    CONSTRAINT [PK_ArticulosTransaction] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ArticulosTransaction_Articulo_IDArticulo] FOREIGN KEY ([IDArticulo]) REFERENCES [Articulo] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Receipt] (
    [Id] uniqueidentifier NOT NULL,
    [LastModifiedBy] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    [LastModifiedDate] datetime2 NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [TranssacionID] uniqueidentifier NULL,
    [DocNumberID] uniqueidentifier NULL,
    [Monto] decimal(18,2) NOT NULL,
    [Descuento] decimal(18,2) NOT NULL,
    [Pagado] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Receipt] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Receipt_DocNumber_DocNumberID] FOREIGN KEY ([DocNumberID]) REFERENCES [DocNumber] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Receipt_TransactionIn_TranssacionID] FOREIGN KEY ([TranssacionID]) REFERENCES [TransactionIn] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [TransactionIndetells] (
    [Id] uniqueidentifier NOT NULL,
    [IdContepto] int NOT NULL,
    [NContepto] nvarchar(max) NULL,
    [Precio] decimal(18,2) NOT NULL,
    [Cantidad] int NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [Impuesto] decimal(18,2) NOT NULL,
    [IsActive] bit NOT NULL,
    [TransactionInId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_TransactionIndetells] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TransactionIndetells_TransactionIn_TransactionInId] FOREIGN KEY ([TransactionInId]) REFERENCES [TransactionIn] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Articulo_UnidadMedidaID] ON [Articulo] ([UnidadMedidaID]);

GO

CREATE INDEX [IX_ArticulosTransaction_IDArticulo] ON [ArticulosTransaction] ([IDArticulo]);

GO

CREATE INDEX [IX_BoxSmallDetails_BoxSmallId] ON [BoxSmallDetails] ([BoxSmallId]);

GO

CREATE INDEX [IX_CatalogoRollGrupo_IdCatalogo] ON [CatalogoRollGrupo] ([IdCatalogo]);

GO

CREATE INDEX [IX_CatalogoRollGrupo_IdCatalogoRoll] ON [CatalogoRollGrupo] ([IdCatalogoRoll]);

GO

CREATE INDEX [IX_Currencies_Countryid] ON [Currencies] ([Countryid]);

GO

CREATE INDEX [IX_EmployeePayrolls_EmployeeId] ON [EmployeePayrolls] ([EmployeeId]);

GO

CREATE INDEX [IX_Entidad_IdCatalgoRoll] ON [Entidad] ([IdCatalgoRoll]);

GO

CREATE INDEX [IX_Payrolls_EmployeePayrollId] ON [Payrolls] ([EmployeePayrollId]);

GO

CREATE INDEX [IX_Receipt_DocNumberID] ON [Receipt] ([DocNumberID]);

GO

CREATE INDEX [IX_Receipt_TranssacionID] ON [Receipt] ([TranssacionID]);

GO

CREATE INDEX [IX_RollUser_RollID] ON [RollUser] ([RollID]);

GO

CREATE INDEX [IX_RollUser_UsuarioID] ON [RollUser] ([UsuarioID]);

GO

CREATE INDEX [IX_TransactionIn_CurrencyId] ON [TransactionIn] ([CurrencyId]);

GO

CREATE INDEX [IX_TransactionIndetells_TransactionInId] ON [TransactionIndetells] ([TransactionInId]);

GO

CREATE INDEX [IX_WalletBoxDetalis_WalletBoxId] ON [WalletBoxDetalis] ([WalletBoxId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220221092437_start', N'3.1.8');

GO

ALTER TABLE [Journals] ADD [Commentary] nvarchar(max) NULL;

GO

ALTER TABLE [Journals] ADD [Reference] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220221213016_numeros', N'3.1.8');

GO

