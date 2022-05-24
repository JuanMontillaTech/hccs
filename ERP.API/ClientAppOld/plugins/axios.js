// import Vue from 'vue'
// import axios from "axios";
// Vue.use(axios)
// this.$axios.setToken('Prueba', 'Bearer')
export default function ({ $axios }) {
    const token = localStorage.getItem('token');
    $axios.setToken(`${token}`, 'Bearer');
    $axios.setHeader('Token', `Bearer ${token}`);
  }
  
