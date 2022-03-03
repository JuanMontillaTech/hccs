export default {
    // Global page headers: https://go.nuxtjs.dev/config-head
    head: {
        title: 'HADA-ERP',

        htmlAttrs: {
            lang: 'es'
        },
        meta: [
            { charset: 'utf-8' },
            { name: 'viewport', content: 'width=device-width, initial-scale=1' },
            { hid: 'description', name: 'description', content: '' },
            { name: 'format-detection', content: 'telephone=no' }
        ],
        link: [
            { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }
        ],

    },


    // Global CSS: https://go.nuxtjs.dev/config-css
    css: [
        '@/assets/menuFiles/css/font-awesome.min.css',
        '@/assets/menuFiles/css/bootstrap.min.css',
        '@/assets/menuFiles/css/hoe-overlay-effect.css',
        '@/assets/menuFiles/css/hoe-push-effect.css',
        '@/assets/menuFiles/css/hoe-shrink-effect.css',
        '@/assets/menuFiles/css/hoe-rightsidebar.css',
        '@/assets/menuFiles/css/hoe-horizontal-navigation.css',
        '@/assets/menuFiles/css/hoe-theme-color.css',
        '@/assets/menuFiles/css/hoe.css',
        '@/assets/menuFiles/css/extra.css',
        '@/assets/menuFiles/css/header.css',

    ],

    // Plugins to run before rendering page: https://go.nuxtjs.dev/config-plugins
    plugins: [
        { src: '~/plugins/vuelidate' },
        { src: '~/plugins/chartkick', mode: 'client' },
        { src: '~/plugins/dataTable', mode: 'client' },
        { src: '~/plugins/vue-good-table', ssr: false }
    ],

    // Auto import components: https://go.nuxtjs.dev/config-components
    components: true,

    // Modules for dev and build (recommended): https://go.nuxtjs.dev/config-modules
    buildModules: [
        '@nuxtjs/fontawesome'
    ],

    fontawesome: {
        component: 'fa',
        icons: {
            solid: true,
            brands: true
        }
    },
    // Modules: https://go.nuxtjs.dev/config-modules
    modules: [
        // https://go.nuxtjs.dev/bootstrap
        'bootstrap-vue/nuxt',
        '@nuxtjs/axios',
    ],

    // Build Configuration: https://go.nuxtjs.dev/config-build
    build: {},
    /*
     ** Axios module configuration
     */
    axios: {
        // See https://github.com/nuxt-community/axios-module#options
        baseURL: 'https://localhost:44367/api',
    },
    env: {
        baseUrl: process.env.BASE_URL || 'https://localhost:44367/',
        devUrl: 'https://localhost:44367/',
        prodUrl: 'https://urlprod'
    }
}