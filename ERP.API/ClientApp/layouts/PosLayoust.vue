<script>
export default {
  mounted() {
    document.body.classList.add("authentication-bg");
  },
};
</script>

<template>
  <div >
    <div>
      <b-navbar toggleable="lg" type="dark" variant="warning">
        <b-navbar-brand href="#">MontillaSoft.com</b-navbar-brand>

        <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

        <b-collapse id="nav-collapse" is-nav>
          <b-navbar-nav>
            <router-link class="nav-link text-white" to="/ExpressForm/Pos"
              >Ordenes</router-link
            >

            <router-link class="nav-link text-white" to="/ExpressForm/FormBoard"
              >Comandas</router-link
            >
            <router-link class="nav-link text-white" to="/ExpressForm/FormBoardFull"
              >Ordenes y Pagar</router-link
            >
          </b-navbar-nav>
        </b-collapse>
      </b-navbar>
    </div>
    <div class="mb-2"  >
      <Nuxt />
    </div>
  </div>
</template>
