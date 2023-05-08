import Vue from 'vue'
import vueselect from "vue-select";

//https://vue-select.org/
const components = { vueselect }

Object.entries(components).forEach(([name, component]) => {
    Vue.component(name, component)
})
