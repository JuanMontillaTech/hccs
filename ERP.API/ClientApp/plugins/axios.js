// import Vue from 'vue'
// import axios from "axios";
// Vue.use(axios)
// this.$axios.setToken('Prueba', 'Bearer')
export default function ({ $axios }) {
  let api = process.env.PROD_API;
  if (process.env.NODE_ENV === "development") {
    api = process.env.DEV_API;
  }
    $axios.setBaseURL(api);
    const token = localStorage.getItem("token");
    $axios.setToken(`${token}`, 'Bearer'); 
    $axios.setHeader("Authorization", token);
  }
  
