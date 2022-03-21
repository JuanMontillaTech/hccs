<template>
  <div>
    <!-- Scrollbar Custom CSS -->
    <link
      rel="stylesheet"
      href="https://cdnjs.cloudflare.com/ajax/libs/malihu-custom-scrollbar-plugin/3.1.5/jquery.mCustomScrollbar.min.css"
    />

    <link
      rel="stylesheet"
      href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.0/css/all.min.css"
    />

    <link
      rel="stylesheet"
      href="https://unpkg.com/vue-select@latest/dist/vue-select.css"
    />
    <div class="wrapper">
      <!-- Sidebar Holder -->
      <nav id="sidebar">
        <div class="sidebar-header">
          <h3>ERP SANCHA</h3>
        </div>

        <ul class="list-unstyled components">
          <li>
            <nuxt-link to="/Home">
              <fa icon="home"></fa>
              <span>INICIO</span>
            </nuxt-link>
          </li>
          <li>
            <a v-b-toggle href="#ingresos">
              <fa icon="inbox" class="ml-1"></fa>
              <span class="menu-text text-uppercase">ingresos</span>
            </a>
            <b-collapse id="ingresos">
              <ul class="list-unstyled">
                <li>
                  <nuxt-link to="/Ingresos/FacturasDeVenta">
                    <span class="menu-text">Facturas de venta</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Ingresos/FacturasDeVentaRecurrentes">
                    <span class="menu-text">Facturas recurrentes</span>
                    <span class="dot bg-danger ml-4"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Ingresos/PagosRecibidos">
                    <span class="menu-text">Pagos recibidos</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Ingresos/NotasDeCredito">
                    <span class="menu-text">Notas de crédito</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Ingresos/Cotizaciones">
                    <span class="menu-text">Cotizaciones</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Ingresos/Conduces">
                    <span class="menu-text">Conduces</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
              </ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#compras">
              <fa icon="cart-arrow-down" class="ml-1"></fa>
              <span class="menu-text text-uppercase">compras</span>
            </a>
            <b-collapse id="compras">
              <ul class="list-unstyled">
                <li>
                  <nuxt-link to="/Compras/FacturasDeProveedores">
                    <span class="menu-text">Facturas de proveedores</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Compras/PagosGastos">
                    <span class="menu-text">Pagos / Gastos</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Compras/GastosMenores">
                    <span class="menu-text">Gastos menores</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Compras/PagosRecurrentes">
                    <span class="menu-text">Pagos recurrentes</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Compras/NotasDebito">
                    <span class="menu-text">Notas débito</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Compras/OrdenesDeCompra">
                    <span class="menu-text">Órdenes de compra</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
              </ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#contactos">
              <fa icon="address-card" class="ml-1"></fa>
              <span class="menu-text text-uppercase">contactos</span>
            </a>
            <b-collapse id="contactos">
              <ul class="list-unstyled">
                <li>
                  <nuxt-link to="/Contactos/Todos">
                    <span>Todos</span>
                    <span class="dot bg-success ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Contactos/Clientes">
                    <span>Clientes</span>
                    <span class="dot bg-success ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Contactos/Proveedores">
                    <span>Proveedores</span>
                    <span class="dot bg-success ml-2"></span>
                  </nuxt-link>
                </li>
                <!-- <li>
                  <nuxt-link to="/Contactos/SolicitarActualizacion">
                    <span>Sol. de actualización</span>
                    <span class="dot bg-danger ml-4"></span>
                  </nuxt-link>
                </li> -->
              </ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#inventario">
              <fa icon="cart-flatbed" class="ml-1"></fa>
              <span class="menu-text text-uppercase">inventario</span>
            </a>
            <b-collapse id="inventario">
              <ul class="list-unstyled">
                <li>
                  <nuxt-link to="/Inventario/ProductosServicios">
                    <span class="menu-text">Productos y servicios</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Inventario/ValorDeInventario">
                    <span class="menu-text">Valor de inventario</span>
                    <span class="dot bg-danger ml-3"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Inventario/AjustesDeInventario">
                    <span class="menu-text">Ajustes de inventario</span>
                    <span class="dot bg-danger ml-1"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Inventario/GestionDeProductos">
                    <span class="menu-text">Gestión de productos</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Inventario/ListaDePrecios">
                    <span class="menu-text">Listas de precios</span>
                    <span class="dot bg-success ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Inventario/Almacenes">
                    <span class="menu-text">Almacenes</span>
                    <span class="dot bg-success ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Inventario/Categorias">
                    <span class="menu-text">Categorías</span>
                    <span class="dot bg-success ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Inventario/Atributos">
                    <span class="menu-text">Atributos</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Inventario/Concepto">
                    <span>Concepto</span>
                    <span class="dot bg-success ml-2"></span>
                  </nuxt-link>
                </li>
              </ul>
            </b-collapse>
          </li>

          <li>
            <nuxt-link to="/Bancos">
              <fa icon="building-columns" class="ml-1"></fa>
              <span class="menu-text text-uppercase">bancos</span>
            </nuxt-link>
          </li>

          <li>
            <a v-b-toggle href="#contabilidad">
              <fa icon="calculator" class="ml-1"></fa>
              <span class="menu-text text-uppercase">contabilidad</span>
            </a>
            <b-collapse id="contabilidad">
              <ul class="list-unstyled">
                <li>
                  <nuxt-link to="/Contabilidad/CatalogosDeCuentas">
                    <span class="menu-text">Catálogo de cuentas</span>
                    <span class="dot bg-success ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Contabilidad/EntradaDeDiario">
                    <span class="menu-text">Entradas de diario</span>
                    <span class="dot bg-success ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Contabilidad/LibroDiario">
                    <span class="menu-text">Libro diario</span>
                    <span class="dot bg-success ml-2"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Contabilidad/ReportesContables">
                    <span class="menu-text">Reportes contables</span>
                    <span class="dot bg-danger ml-2"></span>
                  </nuxt-link>
                </li>
              </ul>
            </b-collapse>
          </li>

          <li>
            <nuxt-link to="/Reportes">
              <fa icon="file-lines" class="ml-1"></fa>
              <span class="menu-text text-uppercase">reportes</span>
            </nuxt-link>
          </li>
        </ul>
      </nav>

      <!-- Page Content Holder -->
      <div id="content">
        <div>
          <Nuxt />
        </div>
      </div>
    </div>
    <script src="https://code.jquery.com/jquery-1.12.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/malihu-custom-scrollbar-plugin/3.1.5/jquery.mCustomScrollbar.concat.min.js"></script>

    <script type="text/javascript">
      $(document).ready(function () {
        $("#sidebar").mCustomScrollbar({
          theme: "minimal",
        });

        $("#sidebarCollapse").on("click", function () {
          $("#sidebar, #content").toggleClass("active");
          $(".collapse.in").toggleClass("in");
          $("a[aria-expanded=true]").attr("aria-expanded", "false");
        });
      });
    </script>
  </div>
</template>

<script>
import VueJwtDecode from "vue-jwt-decode";
export default {
  data() {
    return {
      token: "",
      username: "",
    };
  },
  mounted() {
    this.token = localStorage.getItem("token");
    const tokenDecoded = VueJwtDecode.decode(this.token);
    this.username = tokenDecoded.Name;
  },
  methods: {
    logout() {
      localStorage.removeItem("token");
      this.$router.push("/Login");
    },
  },
};
</script>

<style>
.dot {
  height: 10px;
  width: 10px;
  background-color: #bbb;
  border-radius: 50%;
  display: inline-block;
}
</style>
