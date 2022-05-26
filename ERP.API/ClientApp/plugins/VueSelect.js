import Vue from 'vue'
import vueselect from "vue-select";


const components = { vueselect }

Object.entries(components).forEach(([name, component]) => {
    Vue.component(name, component)
})