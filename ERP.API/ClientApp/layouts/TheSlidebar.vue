<template>
  <div
    hoe-navigation-type="vertical-compact"
    hoe-nav-placement="left"
    theme-layout="wide-layout"
    theme-bg="bg1"
  >
    <link
      href="http://fonts.googleapis.com/css?family=Lato:100italic,100,300italic,300,400italic,400,700italic,700,900italic,900"
      rel="stylesheet"
      type="text/css"
    />
    <link
      href="https://fonts.googleapis.com/css?family=Poppins:100,200,300,400,500,600,700,800,900&display=swap"
      rel="stylesheet"
    />
    
    <div id="hoeapp-wrapper" class="hoe-hide-lpanel" hoe-device-type="desktop">
      <header id="hoe-header">
        <div class="hoe-left-header">
          <i class="fa fa-graduation-cap text-white"></i>
          <a href="#"> <span class="pr-4">ERP SANCHA</span></a>
          <span class="hoe-sidebar-toggle"><a href="#"></a></span>
        </div>

        <div class="hoe-right-header" hoe-position-type="relative">
          <span class="hoe-sidebar-toggle text-white"
            ><a href="#" class="color-white"></a
          ></span>
          <ul class="right-navbar">
            <li class="dropdown hoe-rheader-submenu hoe-header-profile">
              <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                <span>
                  <img class="rounded-circle" src="../assets/menuFiles/images/avatar-1.jpg"/>
                </span>
                <span><b>{{ username }}</b> <i class="fa fa-angle-down"></i></span>
              </a>
              <ul class="dropdown-menu">
                <li>
                  <a href="#"><i class="fa fa-user"></i>Mi Perfil</a>
                </li>
                <li>
                  <a @click="logout()"><i class="fa fa-power-off"></i>Salir</a>
                </li>
              </ul>
            </li>
          </ul>
        </div>
      </header>
      <div
        id="hoeapp-container"
        hoe-color-type="lpanel-bg2"
        hoe-lpanel-effect="shrink"
      >
        <aside id="hoe-left-panel" hoe-position-type="absolute">
          <ul class="nav panel-list">
            <li class="nav-level">Menu de Navegacion</li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <fa icon="home" class="ml-1"></fa>
                <span class="menu-text text-uppercase">inicio</span>
                <span class="selected"></span>
              </a>
            </li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <fa icon="inbox" class="ml-1"></fa>
                <span class="menu-text text-uppercase">ingresos</span>
                <span class="selected"></span>
              </a>
              <ul class="hoe-sub-menu">
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text">Facturas de venta</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text">Facturas recurrentes</span>
                    <span class="selected"></span>
                  </a>
                </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Pagos recibidos</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Notas de crédito</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Cotizaciones</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Conduces</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Punto de venta</span>
                        <span class="selected"></span>
                      </a>
                    </li>
              </ul>
            </li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <fa icon="cart-arrow-down" class="ml-1"></fa>
                <span class="menu-text text-uppercase">compras</span>
                <span class="selected"></span>
              </a>
              <ul class="hoe-sub-menu">
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text">Facturas de proveedores</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text">Pagos / Gastos</span>
                    <span class="selected"></span>
                  </a>
                </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Gastos menores</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Pagos recurrentes</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Notas débito</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Órdenes de compra</span>
                        <span class="selected"></span>
                      </a>
                    </li>
              </ul>
            </li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <fa icon="address-card" class="ml-1"></fa>
                <span class="menu-text text-uppercase">contactos</span>
                <span class="selected"></span>
              </a>
              <ul class="hoe-sub-menu">
                <li>
                  <nuxt-link to="/Contactos/Todos">
                    <a href="javascript:void(0)">
                      
                    <a> <span class="menu-text text-white">Todos</span></a>
                      <span class="selected"></span>
                    </a>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Contactos/Clientes">
                    <a href="javascript:void(0)">
                      <span class="menu-text text-white">Clientes</span>
                      <span class="selected"></span>
                    </a>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Contactos/Proveedores">
                    <a href="javascript:void(0)">
                      <span class="menu-text text-white">Proveedores</span>
                      <span class="selected"></span>
                    </a>
                  </nuxt-link>
                </li>
                <li>
                  <nuxt-link to="/Contactos/SolicitarActualizacion">
                    <a href="javascript:void(0)">
                      <span class="menu-text text-white">Solicitudes de actualización</span>
                      <span class="selected"></span>
                    </a>
                  </nuxt-link>
                </li>
              </ul>
            </li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <fa icon="cart-flatbed" class="ml-1"></fa>
                <span class="menu-text text-uppercase">inventario</span>
                <span class="selected"></span>
              </a>
              <ul class="hoe-sub-menu">
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text">Productos y servicios</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text">Valor de inventario</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text">Ajustes de inventario</span>
                    <span class="selected"></span>
                  </a>
                  </li>
                  <li>
                    <a href="javascript:void(0)">
                      <span class="menu-text">Gestión de productos</span>
                      <span class="selected"></span>
                    </a>
                  </li>
                  <li>
                    <a href="javascript:void(0)">
                      <span class="menu-text">Listas de precios</span>
                      <span class="selected"></span>
                    </a>
                  </li>
                  <li>
                    <a href="javascript:void(0)">
                      <span class="menu-text">Almacenes</span>
                      <span class="selected"></span>
                    </a>
                  </li>
                  <li>
                    <a href="javascript:void(0)">
                      <span class="menu-text">Categorías</span>
                      <span class="selected"></span>
                    </a>
                  </li>
                  <li>
                    <a href="javascript:void(0)">
                      <span class="menu-text">Atributos</span>
                      <span class="selected"></span>
                    </a>
                  </li>
              </ul>
            </li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <fa icon="building-columns" class="ml-1"></fa>
                <span class="menu-text text-uppercase">bancos</span>
                <span class="selected"></span>
              </a>
            </li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <fa icon="calculator" class="ml-1"></fa>
                <span class="menu-text text-uppercase">contabilidad</span>
                <span class="selected"></span>
              </a>
              <ul class="hoe-sub-menu">
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text"><a>Catálogo de cuentas</a></span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text"
                      ><a>Entradas de diario</a></span
                    >
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text">Libro diario</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text">Reportes contables</span>
                    <span class="selected"></span>
                  </a>
                </li>
              </ul>
            </li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <fa icon="file-lines" class="ml-1"></fa>
                <span class="menu-text text-uppercase">reportes</span>
                <span class="selected"></span>
              </a>
            </li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <fa icon="circle-check" class="ml-1"></fa>
                <span class="menu-text text-uppercase">pos</span>
                <span class="selected"></span>
              </a>
            </li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <fa icon="store" class="ml-1"></fa>
                <span class="menu-text text-uppercase">Tienda en línea</span>
                <span class="selected"></span>
              </a>
            </li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <fa icon="gear" class="ml-1"></fa>
                <span class="menu-text text-uppercase">Configuración</span>
                <span class="selected"></span>
              </a>
            </li>
          </ul>
        </aside>
        <section id="main-content">
          <Nuxt />
        </section>
      </div>
    </div>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
   <script src="https://cdn.jsdelivr.net/npm/numbro@2.3.6/dist/numbro.min.js"></script>
       <script src="https://unpkg.com/vue-select@latest"></script>
    <link rel="stylesheet" href="https://unpkg.com/vue-select@latest/dist/vue-select.css">
      <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <script
      type="application/javascript"
      defer
      src="../assets/menuFiles/js/3.4.1.jquery.min.js"
    ></script>
    
    <!-- Include all compiled plugins (below), or inc~/assetslude individual files as needed -->
    <script
      type="application/javascript"
      defer
      src="../assets/menuFiles/js/popper.min.js"
    ></script>
    <script
      type="application/javascript"
      defer
      src="../assets/menuFiles/js/bootstrap.min.js"
    ></script>
    <script
      type="application/javascript"
      defer
      src="../assets/menuFiles/js/hoe.js"
    ></script>
  </div>
</template>

<script>
import VueJwtDecode from 'vue-jwt-decode'
export default {
  data() {
    return {
      token: '',
      username: '',
    };
  },
  mounted(){
    this.token = localStorage.getItem('token');
    const tokenDecoded = VueJwtDecode.decode(this.token);
    this.username = tokenDecoded.Name;
  },
  methods: {
    logout(){
      localStorage.removeItem('token');
      this.$router.push('/Login');
    }
  }
};
</script>

<style>
</style>