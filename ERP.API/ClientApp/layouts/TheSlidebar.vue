<template>
  <div>
    <!-- Scrollbar Custom CSS -->
    <link
      rel="stylesheet"
      href="https://cdnjs.cloudflare.com/ajax/libs/malihu-custom-scrollbar-plugin/3.1.5/jquery.mCustomScrollbar.min.css"
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
              <span class="selected"></span>
            </nuxt-link>
          </li>
          <!-- <li class="active">
                      <a>
                        <fa icon="home"></fa>
                        <span class="menu-text text-uppercase">inicio</span>
                        <span class="selected"></span>
                      </a>
                    </li> -->
          <li>
            <a v-b-toggle href="#ingresos">
              <fa icon="inbox" class="ml-1"></fa>
              <span class="menu-text text-uppercase">ingresos</span>
              <span class="selected"></span>
            </a>
            <b-collapse id="ingresos">
              <ul class="list-unstyled">
                <li>
                  <a>
                    <span class="menu-text">Facturas de venta</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Facturas recurrentes</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Pagos recibidos</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Notas de crédito</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Cotizaciones</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Conduces</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Punto de venta</span>
                    <span class="selected"></span>
                  </a>
                </li>
              </ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#compras">
              <fa icon="cart-arrow-down" class="ml-1"></fa>
              <span class="menu-text text-uppercase">compras</span>
              <span class="selected"></span>
            </a>
            <b-collapse id="compras">
              <ul class="list-unstyled">
                <li>
                  <a>
                    <span class="menu-text">Facturas de proveedores</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Pagos / Gastos</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Gastos menores</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Pagos recurrentes</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Notas débito</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Órdenes de compra</span>
                    <span class="selected"></span>
                  </a>
                </li>
              </ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#contactos">
              <fa icon="address-card" class="ml-1"></fa>
              <span class="menu-text text-uppercase">contactos</span>
              <span class="selected"></span>
            </a>
            <b-collapse id="contactos">
              <ul class="list-unstyled">
                <li>
                  <nuxt-link to="/Contactos/Todos">
                    <span>Todos</span>
                    <span class="selected"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Contactos/Clientes">
                    <span>Clientes</span>
                    <span class="selected"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Contactos/Proveedores">
                    <span>Proveedores</span>
                    <span class="selected"></span>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Contactos/SolicitarActualizacion">
                    <span>Solicitudes de actualización</span>
                    <span class="selected"></span>
                  </nuxt-link>
                </li>
              </ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#inventario">
              <fa icon="cart-flatbed" class="ml-1"></fa>
              <span class="menu-text text-uppercase">inventario</span>
              <span class="selected"></span>
            </a>
            <b-collapse id="inventario">
              <ul class="list-unstyled">
                <li>
                  <a>
                    <span class="menu-text">Productos y servicios</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Valor de inventario</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Ajustes de inventario</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Gestión de productos</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Listas de precios</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Almacenes</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Categorías</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Atributos</span>
                    <span class="selected"></span>
                  </a>
                </li>
              </ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#bancos">
              <fa icon="building-columns" class="ml-1"></fa>
              <span class="menu-text text-uppercase">bancos</span>
              <span class="selected"></span>
            </a>
            <b-collapse id="bancos">
              <ul class="list-unstyled"></ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#contabilidad">
              <fa icon="calculator" class="ml-1"></fa>
              <span class="menu-text text-uppercase">contabilidad</span>
              <span class="selected"></span>
            </a>
            <b-collapse id="contabilidad">
              <ul class="list-unstyled">
                <li>
                  <a>
                    <span class="menu-text">Catálogo de cuentas</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Entradas de diario</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Libro diario</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a>
                    <span class="menu-text">Reportes contables</span>
                    <span class="selected"></span>
                  </a>
                </li>
              </ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#reportes">
              <fa icon="file-lines" class="ml-1"></fa>
              <span class="menu-text text-uppercase">reportes</span>
              <span class="selected"></span>
            </a>
            <b-collapse id="reportes">
              <ul class="list-unstyled"></ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#pos">
              <fa icon="circle-check" class="ml-1"></fa>
              <span class="menu-text text-uppercase">pos</span>
              <span class="selected"></span>
            </a>
            <b-collapse id="pos">
              <ul class="list-unstyled"></ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#tienda-linea">
              <fa icon="store" class="ml-1"></fa>
              <span class="menu-text text-uppercase">Tienda en línea</span>
              <span class="selected"></span>
            </a>
            <b-collapse id="tienda-linea">
              <ul class="list-unstyled"></ul>
            </b-collapse>
          </li>

          <li>
            <a v-b-toggle href="#configuracion">
              <fa icon="gear" class="ml-1"></fa>
              <span class="menu-text text-uppercase">Configuración</span>
              <span class="selected"></span>
            </a>
            <b-collapse id="configuracion">
              <ul class="list-unstyled"></ul>
            </b-collapse>
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

<style></style>
