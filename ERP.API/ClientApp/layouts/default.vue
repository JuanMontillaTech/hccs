 
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
      <header id="hoe-header" hoe-lpanel-effect="shrink">
        <div class="hoe-left-header">
          <img
            src="../static/assets/menuFiles/images/logofavicon.svg"
            alt="institution logo"
            style="width: 50px"
          />
          <a href="#"> <span class="pr-4">ERP SANCHE</span></a>
          <span class="hoe-sidebar-toggle"><a href="#"></a></span>
        </div>

        <div class="hoe-right-header" hoe-position-type="relative">
          <span class="hoe-sidebar-toggle"
            ><a href="#" class="color-white"></a
          ></span>
          <ul class="right-navbar">
            <li class="dropdown hoe-rheader-submenu hoe-header-profile">
              <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                <span
                  ><img class="rounded-circle" src="../static/assets/menuFiles/images/avatar-1.jpg"
                /></span>
                <span><b>UserName</b> <i class="fa fa-angle-down"></i></span>
              </a>
              <ul class="dropdown-menu">
                <li>
                  <a href="#"><i class="fa fa-user"></i>My Profile</a>
                </li>
                <li>
                  <a href="#"><i class="fa fa-calendar"></i>My Calendar</a>
                </li>
                <li>
                  <a href="#"
                    ><i class="fa fa-envelope"></i>My Inbox
                    <span class="badge badge-danger"> 3 </span></a
                  >
                </li>
                <li>
                  <a href="#"
                    ><i class="fa fa-rocket"></i>My Tasks
                    <span class="badge badge-success"> 7 </span></a
                  >
                </li>
                <li>
                  <a href="#"><i class="fa fa-lock"></i>Lock Screen</a>
                </li>
                <li>
                  <a href="#"><i class="fa fa-power-off"></i>Logout</a>
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
          <div class="profile-box">
            <div class="media">
              <div class="media-body">
                <h5 class="media-heading">
                  <!-- <span>Software de Planificacion de Recursos</span> -->
                </h5>
                <!-- <small>Software de Planificacion de Recursos</small> -->
              </div>
            </div>
          </div>
          <ul class="nav panel-list">
            <li class="nav-level">Menu de Navegacion</li>
            <li class="active hoe-has-menu">
              <a href="">
                <i class="fa fa-user"></i>
                <span class="menu-text">Contactos</span>
                <span class="selected"></span>
              </a>
              <!-- <ul class="hoe-sub-menu">
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text">Administracion</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li class="hoe-has-menu">
                  <a href="javascript:void(0)">
                    
                    <span class="menu-text">Recursos Humanos</span>
                    <span class="selected"></span>
                  </a>
                  <ul class="hoe-sub-menu">
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">ARS</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Posiciones Laborales</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Titulo Academico</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Departamentos</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Grupo Organizacional</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">AFP</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Tipo de Sangre</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Empleados</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Nivel Academico</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Direcciones</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                  </ul>
                </li>
              </ul> -->
            <!-- </li>
            <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <i class="fa fa-paper-plane"></i>
                <span class="menu-text">Transacciones</span>
                <span class="selected"></span>
              </a>
              <ul class="hoe-sub-menu">
                <li class="hoe-has-menu">
                  <a href="javascript:void(0)">
                    
                    <span class="menu-text">Administracion</span>
                    <span class="selected"></span>
                  </a>
                  <ul class="hoe-sub-menu">
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Recepcion de Correspondencia</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Agenda de Salones</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                  </ul>
                </li>
              </ul> -->
            </li>
            <!-- <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <i class="fa fa-file-text"></i>
                <span class="menu-text">Reportes</span>
                <span class="selected"></span>
              </a>
            </li> -->
            <!-- <li class="hoe-has-menu">
              <a href="javascript:void(0)">
                <i class="fa fa-gear"></i>
                <span class="menu-text">Configuracion</span>
                <span class="selected"></span>
              </a>
              <ul class="hoe-sub-menu">
                <li>
                  <a href="javascript:void(0)">
                    <span class="menu-text">Administracion</span>
                    <span class="selected"></span>
                  </a>
                </li>
                <li class="hoe-has-menu">
                  <a href="javascript:void(0)">
                    
                    <span class="menu-text">Recursos Humanos</span>
                    <span class="selected"></span>
                  </a>
                  <ul class="hoe-sub-menu">
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Estado Civil</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Tipo de Item</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Seccion de Campo</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                    <li>
                      <a href="javascript:void(0)">
                        <span class="menu-text">Tipo de Documentos</span>
                        <span class="selected"></span>
                      </a>
                    </li>
                  </ul>
                </li>
              </ul>
            </li> -->
          </ul>
        </aside>
        <section id="main-content">
        </section>
        <!-- <div id="styleSelector">
          <div class="selector-toggle">
            <a href="javascript:void(0)"></a>
          </div>
          <ul>
            <li>
              <p class="selector-title">Style Selector</p>
            </li>
            <li class="theme-option">
              <span class="sub-title">Header BG Color Option</span>
              <div id="theme-color">
                <a href="#" class="header-bg" hoe-color-type="header-bg1"
                  >&nbsp;</a
                >
                <a href="#" class="header-bg" hoe-color-type="header-bg2"
                  >&nbsp;</a
                >
                <a href="#" class="header-bg" hoe-color-type="header-bg3"
                  >&nbsp;</a
                >
                <a href="#" class="header-bg" hoe-color-type="header-bg4"
                  >&nbsp;</a
                >
                <a href="#" class="header-bg" hoe-color-type="header-bg5"
                  >&nbsp;</a
                >
                <a href="#" class="header-bg" hoe-color-type="header-bg6"
                  >&nbsp;</a
                >
                <a href="#" class="header-bg" hoe-color-type="header-bg7"
                  >&nbsp;</a
                >
                <a href="#" class="header-bg" hoe-color-type="header-bg8"
                  >&nbsp;</a
                >
                <a href="#" class="header-bg" hoe-color-type="header-bg9"
                  >&nbsp;</a
                >
              </div>
            </li>
            <li class="theme-option">
              <span class="sub-title">Left Panel BG Color Option</span>
              <div id="theme-color">
                <a href="#" class="lpanel-bg" hoe-color-type="lpanel-bg1"
                  >&nbsp;</a
                >
                <a href="#" class="lpanel-bg" hoe-color-type="lpanel-bg2"
                  >&nbsp;</a
                >
                <a href="#" class="lpanel-bg" hoe-color-type="lpanel-bg3"
                  >&nbsp;</a
                >
                <a href="#" class="lpanel-bg" hoe-color-type="lpanel-bg4"
                  >&nbsp;</a
                >
                <a href="#" class="lpanel-bg" hoe-color-type="lpanel-bg5"
                  >&nbsp;</a
                >
                <a href="#" class="lpanel-bg" hoe-color-type="lpanel-bg6"
                  >&nbsp;</a
                >
                <a href="#" class="lpanel-bg" hoe-color-type="lpanel-bg7"
                  >&nbsp;</a
                >
                <a href="#" class="lpanel-bg" hoe-color-type="lpanel-bg8"
                  >&nbsp;</a
                >
                <a href="#" class="lpanel-bg" hoe-color-type="lpanel-bg9"
                  >&nbsp;</a
                >
              </div>
            </li>
            <li class="theme-option">
              <span class="sub-title">Logo Color BG Option</span>
              <div id="theme-color">
                <a href="#" class="logo-bg" hoe-color-type="logo-bg1">&nbsp;</a>
                <a href="#" class="logo-bg" hoe-color-type="logo-bg2">&nbsp;</a>
                <a href="#" class="logo-bg" hoe-color-type="logo-bg3">&nbsp;</a>
                <a href="#" class="logo-bg" hoe-color-type="logo-bg4">&nbsp;</a>
                <a href="#" class="logo-bg" hoe-color-type="logo-bg5">&nbsp;</a>
                <a href="#" class="logo-bg" hoe-color-type="logo-bg6">&nbsp;</a>
                <a href="#" class="logo-bg" hoe-color-type="logo-bg7">&nbsp;</a>
                <a href="#" class="logo-bg" hoe-color-type="logo-bg8">&nbsp;</a>
                <a href="#" class="logo-bg" hoe-color-type="logo-bg9">&nbsp;</a>
              </div>
            </li>
            <li class="theme-option">
              <span class="sub-title">Theme Background BG Option</span>
              <div id="theme-color">
                <a href="#" class="theme-bg" hoe-themebg-type="bg1">&nbsp;</a>
                <a href="#" class="theme-bg" hoe-themebg-type="bg2">&nbsp;</a>
                <a href="#" class="theme-bg" hoe-themebg-type="bg3">&nbsp;</a>
                <a href="#" class="theme-bg" hoe-themebg-type="bg4">&nbsp;</a>
                <a href="#" class="theme-bg" hoe-themebg-type="bg5">&nbsp;</a>
                <a href="#" class="theme-bg" hoe-themebg-type="bg6">&nbsp;</a>
                <a href="#" class="theme-bg" hoe-themebg-type="bg7">&nbsp;</a>
                <a href="#" class="theme-bg" hoe-themebg-type="bg8">&nbsp;</a>
                <a href="#" class="theme-bg" hoe-themebg-type="bg9">&nbsp;</a>
              </div>
            </li>
            <li class="theme-option">
              <span class="sub-title">Theme Layout</span>
              <select id="theme-layout" class="form-control minimal input-sm">
                <option name="theme-layout" value="wide-layout">
                  Wide Layout
                </option>
                <option name="theme-layout" value="box-layout">
                  Boxed Layout
                </option>
              </select>
            </li>
            <li class="theme-option">
              <span class="sub-title">SideBar Effect</span>
              <select
                id="leftpanel-effect"
                class="form-control minimal input-sm"
              >
                <option name="lpanel-effect" value="shrink">Default</option>
                <option name="lpanel-effect" value="overlay">Overlay</option>
                <option name="lpanel-effect" value="push">Push</option>
              </select>
            </li>
            <li class="theme-option">
              <span class="sub-title">Navigation Type</span>
              <select
                id="navigation-type"
                class="form-control minimal input-sm"
              >
                <option name="navigation-type" value="vertical">
                  Vertical
                </option>
                <option name="navigation-type" value="vertical-compact">
                  Vertical Compact
                </option>
                <option name="navigation-type" value="horizontal">
                  Horizontal
                </option>
                <option name="navigation-type" value="horizontal-compact">
                  Horizontal compact
                </option>
              </select>
            </li>
            <li class="theme-option">
              <span class="sub-title">Navigation Side</span>
              <select
                id="navigation-side"
                class="form-control minimal input-sm"
              >
                <option name="navigation-side" value="leftside">Left</option>
                <option name="navigation-side" value="rightside">Right</option>
              </select>
            </li>
            <li class="theme-option">
              <span class="sub-title">Sidebar Position</span>
              <select
                id="sidebar-position"
                class="form-control minimal input-sm"
              >
                <option name="sidebar-position" value="default">Default</option>
                <option name="sidebar-position" value="fixed">Fixed</option>
              </select>
            </li>
          </ul>
        </div> -->
      </div>
    </div>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script
      type="application/javascript"
      defer
      src="assets/menuFiles/js/3.4.1.jquery.min.js"
    ></script>
    <!-- Include all compiled plugins (below), or inc~/assetslude individual files as needed -->
    <script
      type="application/javascript"
      defer
      src="assets/menuFiles/js/popper.min.js"
    ></script>
    <script
      type="application/javascript"
      defer
      src="assets/menuFiles/js/bootstrap.min.js"
    ></script>
    <script
      type="application/javascript"
      defer
      src="assets/menuFiles/js/hoe.js"
    ></script>
  </div>
</template>

<script>
export default {
  data() {
    return {};
  },
};
</script>

<style>
</style>