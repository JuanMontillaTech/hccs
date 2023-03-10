export default {
    // server: {
    //     port: 8000, // default: 3000
    //     host: '0.0.0.0' // default: localhost
    //nn   },
    // loading: "~/components/loading.vue",
    /*
     ** Nuxt rendering mode
     ** See https://nuxtjs.org/api/configuration-mode
     */
    ssr: false,
    /*
     ** Nuxt target
     ** See https://nuxtjs.org/api/configuration-target
     */
    target: "server",
    /*
     ** Headers of the page
     ** See https://nuxtjs.org/api/configuration-head
     */
    head: {
        meta: [
            { charset: "utf-8" },
            { name: "viewport", content: "width=device-width, initial-scale=1" },
            {
                hid: "description",
                name: "description",
                content: "Montilla Soft",
            }
        ],
        link: [{ rel: "icon", type: "image/x-icon", href: "/favicon.ico" }]
    },
    router: {
        // linkExactActiveClass: 'active'
    },

    /*
     ** Global CSS
     */
    css: ["~/assets/scss/app.scss"],
    /*
     ** Plugins to load before mounting the App
     ** https://nuxtjs.org/guide/plugins
     */
    plugins: [
        '~/plugins/fakeauth.js',
        "~/plugins/fireauth.js",
        '~/plugins/i18n.js',
        "~/plugins/simplebar.js",
        "~/plugins/vue-click-outside.js",
        "~/plugins/vue-apexcharts.js",
        "~/plugins/vuelidate.js",
        "~/plugins/vue-slidebar.js",
        "~/plugins/vue-lightbox.js",
        "~/plugins/vue-chartist.js",
        "~/plugins/vue-mask.js",
        "~/plugins/vue-googlemap.js",
        "~/plugins/VueSelect.js",
        "~/plugins/VSwitch.js",
        "~/plugins/vue-good-table.js",
        "~/plugins/vue-izitoast",
        "~/plugins/axios",
        "~/plugins/VeeValidate"
    ],
    /*
     ** Auto import components
     ** See https://nuxtjs.org/api/configuration-components
     */
    components: true,
    /*
     ** Nuxt.js dev-modules
     */
    buildModules: [],
    /*
     ** Nuxt.js modules
     */
    modules: [
        // Doc: https://bootstrap-vue.js.org
        "bootstrap-vue/nuxt",
        // Doc: https://github.com/nuxt/content
        "@nuxt/content",
        "@nuxtjs/axios"

    ],

    /*
     ** Content module configuration
     ** See https://content.nuxtjs.org/configuration
     */
    content: {},
    /*
     ** Build configuration
     ** See https://nuxtjs.org/api/configuration-build/
     */
    build: {},
    env: {
        auth: process.env.VUE_APP_DEFAULT_AUTH,
        apikey: process.env.VUE_APP_APIKEY,
        DEV_API:"https://localhost:44367/api/",
        PROD_API:  "https://api.administracionhccs.com/api/",
        authdomain: process.env.VUE_APP_AUTHDOMAIN,
        databaseurl: process.env.VUE_APP_DATABASEURL,
        projectid: process.env.VUE_APP_PROJECTId,
        storgebucket: process.env.VUE_APP_STORAGEBUCKET,
        message: process.env.VUE_APP_MESSAGINGSENDERID,
        appid: process.env.VUE_APP_APPId,
        measurement: process.env.VUE_APP_MEASUREMENTID,
    }
};
