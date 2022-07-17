export const menuItems = [{
    id: 1,
    label: "menuitems.menu.text",
    isTitle: true
},
{
    id: 6,
    label: "menuitems.ingresos.text",
    icon: "uil-store",
    subItems: [
        {
            id: 7,
            label: "menuitems.ingresos.list.FacturasDeVenta",
            link: "/Ingresos/FacturasDeVenta",
            parentId: 6
        },
        {
        id: "25F94E8C-8EA0-4EE0-ADF5-02149A0E072B",
        label: "Venta2",
        link: "/Ingresos/FacturasDeVenta?Form=25F94E8C-8EA0-4EE0-ADF5-02149A0E073B",
        parentId: 6
    },
    {
        id: 8,
        label: "menuitems.ingresos.list.Conduces",
        link: "/Ingresos/Conduces",
        parentId: 6
    },
    {
        id: 9,
        label: "menuitems.ingresos.list.NotasDeCredito",
        link: "/Ingresos/NotasDeCredito",
        parentId: 6
    },
    {
        id: 10,
        label: "menuitems.ingresos.list.PagosRecibidos",
        link: "/Ingresos/PagosRecibidos",
        //link: "/Formulario/Form?form=25f94e8c-8ea0-4ee0-adf5-02149a0e072b",
        parentId: 6
    },
    {
        id: 11,
        label: "menuitems.ingresos.list.Cotizaciones",
        link: "/Ingresos/Cotizaciones",
        parentId: 6
    },
    ],
},
{
    id: 12,
    label: "menuitems.compras.text",
    icon: "uil-envelope",
    subItems: [{
        id: 16,
        label: "menuitems.compras.list.ComprasPorConcepto",
        link: "/compras/ComprasPorConcepto",
        parentId: 15
    },
    {
        id: 17,
        label: "menuitems.compras.list.PagosGastos",
        link: "/compras/PagosGastos",
        parentId: 15
    },
    {
        id: 17,
        label: "menuitems.compras.list.NotasDebito",
        link: "/compras/NotasDebito",
        parentId: 15
    },
    {
        id: 17,
        label: "menuitems.compras.list.OrdenesDeCompra",
        link: "/compras/OrdenesDeCompra",
        parentId: 15
    },
    ]
},
{
    id: 13,
    label: "menuitems.contactos.text",
    icon: "uil-envelope",
    subItems: [{
        id: 16,
        label: "menuitems.contactos.list.Todos",
        link: "/contactos/Todos",
        parentId: 15
    },
    {
        id: 17,
        label: "menuitems.contactos.list.Clientes",
        link: "/contactos/Clientes",
        parentId: 15
    },
    {
        id: 18,
        label: "menuitems.contactos.list.Proveedores",
        link: "/contactos/Proveedores",
        parentId: 15
    },
    {
        id: 19,
        label: "menuitems.contactos.list.Empleados",
        link: "/contactos/Empleados",
        parentId: 15
    },
    {
        id: 20,
        label: "menuitems.contactos.list.Hermanas",
        link: "/contactos/Hermanas",
        parentId: 15
    },
    ]
},
{
    id: 14,
    label: "menuitems.inventario.text",
    icon: "uil-envelope",
    subItems: [{
        id: 16,
        label: "menuitems.inventario.list.Concepto",
        link: "/inventario/Concepto",
        parentId: 15
    },]
},
{
    id: 15,
    label: "menuitems.bancos.text",
    icon: "uil-calender",
    link: "/calendar"
},
{
    id: 16,
    label: "menuitems.contabilidad.text",
    icon: "uil-calender",
    subItems: [{
        id: 16,
        label: "menuitems.contabilidad.list.CatalogosDeCuentas",
        link: "/contabilidad/CatalogosDeCuentas",
        parentId: 15
    },
    {
        id: 17,
        label: "menuitems.contabilidad.list.EntradaDeDiario",
        link: "/contabilidad/EntradaDeDiario",
        parentId: 15
    },
    {
        id: 18,
        label: "menuitems.contabilidad.list.LibroDiario",
        link: "/contabilidad/LibroDiario",
        parentId: 15
    },

    ]
},
{
    id: 17,
    label: "menuitems.reportes.text",
    icon: "uil-file-graph",
    subItems: [

        {
            id: 94,
            label: "menuitems.reportes.list.BalanceComprobacion",
            link: "/Reportes/BalanceComprobacion",
            parentId: 17
        },
        {
            id: 95,
            label: "menuitems.reportes.list.mayor",
            link: "/Reportes/MayorGeneral",
            parentId: 17
        },
        {
            id: 96,
            label: "menuitems.reportes.list.estadoResultados",
            link: "/Reportes/EstadoResultados",
            parentId: 17
        }, {
            id: 96,
            label: "menuitems.reportes.list.Semestres1",
            link: "/Reportes/Semestres1",
            parentId: 17
        }, {
            id: 96,
            label: "menuitems.reportes.list.Semestres2",
            link: "/Reportes/Semestres2",
            parentId: 17
        },

    ],
},

{
    id: 18,
    label: "menuitems.cofiguracion.text",
    icon: "uil-calender",
    subItems: [

        {
            id: 1,
            label: "menuitems.cofiguracion.list.ConfiguracionReportes",
            link: "/ConfigurationReport",
            parentId: 17
        },
        {
            id: 232,
            label: "menuitems.cofiguracion.list.ConfiguracionVenta",
            link: "/Configuration/ConfiguracionVentas",
            parentId: 17
        },

    ]
},

];