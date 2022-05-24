import Vue from 'vue';
import VueJwtDecode from 'vue-jwt-decode'

Vue.use(VueJwtDecode)
// like this
// VueJwtDecode.decode("<your jwt>")

// or like this
// Vue.jwtDec("<your jwt>")

// or in component
// this.$jwtDec("<your jwt>")